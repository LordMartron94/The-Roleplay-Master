package detection

import "net"

type ConnData struct {
	Conn net.Conn
	Data []byte
}
