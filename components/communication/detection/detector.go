package detection

import (
	"../logging"
	"sync"
)

type Detector struct {
	Logger        logging.HoornLogger
	ShutdownSigCh chan struct{}
}

func (d *Detector) StartDetectionLoop(wg *sync.WaitGroup, shutdownSigCh chan struct{}, port string, channelId string) {
	connectionManager, err := NewConnectionManager(d.Logger, shutdownSigCh, port, wg, channelId)

	if err != nil {
		d.Logger.Error(err.Error(), false)
		return
	}

	connectionManager.StartHandlingConnections()
}
