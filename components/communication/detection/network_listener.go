package detection

import (
	"../logging"
	"fmt"
	"net"
	"sync"
)

// NetworkListener sets up and controls the TCP Listener
type NetworkListener struct {
	Listener   net.Listener
	Logger     logging.HoornLogger
	ShutdownCh chan struct{}
}

func (nl *NetworkListener) StartListening(wg *sync.WaitGroup, port string) error {
	listener, err := net.Listen("tcp", port)

	if err == nil {
		nl.Logger.Info(fmt.Sprintf("Listening on port %s...", port), false)
	} else {
		nl.Logger.Error(fmt.Sprintf("Failed to start listener at port %s: %v", port, err), false)
		return err
	}

	nl.Listener = listener
	go nl.watchForShutdown(wg)
	return nil
}

func (nl *NetworkListener) watchForShutdown(wg *sync.WaitGroup) {
	defer wg.Done()
	<-nl.ShutdownCh
	nl.Logger.Info("Received shutdown signal. Shutting down Listener...", false)
	nl.Listener.Close()
}
