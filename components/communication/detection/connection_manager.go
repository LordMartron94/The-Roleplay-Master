package detection

import (
	"../logging"
	"fmt"
	"io"
	"net"
)

// ConnectionManager accepts and processes new client connections.
type ConnectionManager struct {
	Listener      net.Listener
	Logger        logging.HoornLogger
	DataChannel   chan []byte
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
	for data := range cm.DataChannel {
		cm.Logger.Info(fmt.Sprintf("Received JSON: %s", string(data)), false)

		if string(data) == `{"action": "shutdown"}` {
			cm.Logger.Info("Received shutdown request.", false)

			close(cm.ShutdownSigCh)
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
				cm.Logger.Debug("Client disconnected.", false)
				break
			} else {
				cm.Logger.Error(fmt.Sprintf("Error reading: %v", err), false)
			}
			break
		}
		cm.DataChannel <- buf[:n]
	}
}
