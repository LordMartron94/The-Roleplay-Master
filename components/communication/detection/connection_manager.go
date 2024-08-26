package detection

import (
	"../handling"
	"../interpretation"
	"../logging"
	"fmt"
	"io"
	"net"
	"sync"
)

// ConnectionManager accepts and processes new client connections.
type ConnectionManager struct {
	Listener      net.Listener
	Logger        logging.HoornLogger
	ShutdownSigCh chan struct{}
	ChannelId     string
	interpreter   *interpretation.Interpreter
	dataChannel   chan ConnData
	actionHandler *handling.ActionHandler
}

func NewConnectionManager(logger logging.HoornLogger, shutdownSigCh chan struct{}, port string, wg *sync.WaitGroup, channelId string) (*ConnectionManager, error) {
	networkListener := &NetworkListener{
		Logger:     logger,
		ShutdownCh: shutdownSigCh,
		ChannelId:  channelId,
	}

	if err := networkListener.StartListening(wg, port); err != nil {
		logger.Error(err.Error(), false)
		return nil, err
	}

	return &ConnectionManager{
		Listener:      networkListener.Listener,
		Logger:        logger,
		ShutdownSigCh: shutdownSigCh,
		ChannelId:     channelId,
		interpreter:   interpretation.NewInterpreter(logger),
		dataChannel:   make(chan ConnData),
		actionHandler: handling.NewActionHandler(logger, shutdownSigCh),
	}, nil
}

func (cm *ConnectionManager) StartHandlingConnections() {
	go cm.handleIncomingConnections()
	go cm.processDataChannel()
}

func (cm *ConnectionManager) handleIncomingConnections() {
	for {
		conn, err := cm.Listener.Accept()
		if err != nil {
			if opErr, ok := err.(*net.OpError); ok && opErr.Err.Error() == "use of closed network connection" {
				break
			} else {
				cm.Logger.Error(fmt.Sprintf("[%s] Error accepting connection: %v", cm.ChannelId, err), false)
			}
			continue
		}
		go cm.handleConnection(conn)
	}
}

func (cm *ConnectionManager) processDataChannel() {
	for connData := range cm.dataChannel {
		rawJson := string(connData.Data)

		cm.Logger.Info(fmt.Sprintf("[%s] Received JSON: %s", cm.ChannelId, rawJson), false)

		request, err := cm.interpreter.Interpret(rawJson)
		if err != nil {
			resp := interpretation.InvalidRequestFormat()
			cm.SendResponse(connData.Conn, resp.ToJson())
			continue
		}

		message := fmt.Sprintf("[%s] Request source %s with action %s", cm.ChannelId, request.Source, request.Actions[0].Name)
		cm.Logger.Debug(message, false)

		responses := cm.actionHandler.HandleRequest(&request)
		for _, resp := range responses {
			cm.SendResponse(connData.Conn, resp.ToJson())
		}
	}
}

func (cm *ConnectionManager) handleConnection(conn net.Conn) {
	defer conn.Close()
	buf := make([]byte, 1024)
	for {
		n, err := conn.Read(buf)
		if err != nil {
			if err == io.EOF {
				cm.Logger.Debug(fmt.Sprintf("[%s] Client disconnected.", cm.ChannelId), false)
				break
			} else {
				cm.Logger.Error(fmt.Sprintf("[%s] Error reading: %v", cm.ChannelId, err), false)
			}
			break
		}

		cm.dataChannel <- ConnData{Conn: conn, Data: buf[:n]}
	}
}

func (cm *ConnectionManager) SendResponse(conn net.Conn, message string) {
	response := []byte(message + "\n")
	_, err := conn.Write(response)
	if err != nil {
		cm.Logger.Error(fmt.Sprintf("[%s] Error writing: %v", cm.ChannelId, err), false)
	}
}
