package detection

import (
	"../interpretation"
	"../logging"
	"fmt"
	"io"
	"net"
)

// ConnectionManager accepts and processes new client connections.
type ConnectionManager struct {
	Listener      net.Listener
	Logger        logging.HoornLogger
	Interpreter   *interpretation.Interpreter
	DataChannel   chan ConnData
	ShutdownSigCh chan struct{}
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
				cm.Logger.Error(fmt.Sprintf("Error accepting connection: %v", err), false)
			}
			continue
		}
		go cm.handleConnection(conn)
	}
}

func (cm *ConnectionManager) processDataChannel() {
	for connData := range cm.DataChannel {
		rawJson := string(connData.Data)

		cm.Logger.Info(fmt.Sprintf("Received JSON: %s", rawJson), false)

		interpreted, err := cm.Interpreter.Interpret(rawJson)
		if err != nil {
			continue
		}

		message := fmt.Sprintf("Request source %s with action %s", interpreted.Source, interpreted.Actions[0].Name)

		cm.Logger.Info(message, false)

		if interpreted.Actions[0].Name == "Shutdown" {
			cm.Logger.Info("Received shutdown request.", false)
			close(cm.ShutdownSigCh)
		}

		resp := "This is the response after interpretation."
		cm.SendResponse(connData.Conn, resp)
	}
}

func (cm *ConnectionManager) handleConnection(conn net.Conn) {
	defer conn.Close()
	buf := make([]byte, 1024)
	for {
		n, err := conn.Read(buf)
		if err != nil {
			if err == io.EOF {
				cm.Logger.Debug("Client disconnected.", false)
				break
			} else {
				cm.Logger.Error(fmt.Sprintf("Error reading: %v", err), false)
			}
			break
		}

		cm.DataChannel <- ConnData{Conn: conn, Data: buf[:n]}
	}
}

func (cm *ConnectionManager) SendResponse(conn net.Conn, message string) {
	response := []byte(message + "\n")
	_, err := conn.Write(response)
	if err != nil {
		cm.Logger.Error(fmt.Sprintf("Error writing: %v", err), false)
	}
}
