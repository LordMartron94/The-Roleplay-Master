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
	ChannelId  string
}

func (nl *NetworkListener) StartListening(wg *sync.WaitGroup, port string) error {
	listener, err := net.Listen("tcp", port)

	if err == nil {
		nl.Logger.Info(fmt.Sprintf("[%s] Listening on port %s...", nl.ChannelId, port), false)
	} else {
		nl.Logger.Error(fmt.Sprintf("[%s] Failed to start listener at port %s: %v", nl.ChannelId, port, err), false)
		return err
	}

	nl.Listener = listener
	go nl.watchForShutdown(wg)
	return nil
}

func (nl *NetworkListener) watchForShutdown(wg *sync.WaitGroup) {
	defer wg.Done()
	<-nl.ShutdownCh
	nl.Logger.Info(fmt.Sprintf("[%s] Received shutdown signal. Shutting down Listener...", nl.ChannelId), false)
	nl.Listener.Close()
}
