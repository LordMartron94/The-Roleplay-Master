package detection

import (
	"../logging"
	"sync"
)

type Detector struct {
	Logger            logging.HoornLogger
	DataChannel       chan []byte
	ShutdownCh        chan struct{}
	NetworkListener   *NetworkListener
	ConnectionManager *ConnectionManager
}

func (d *Detector) StartDetectionLoop(wg *sync.WaitGroup) {
	port := ":8080"

	d.DataChannel = make(chan []byte)

	d.NetworkListener = &NetworkListener{
		Logger:     d.Logger,
		ShutdownCh: d.ShutdownCh,
	}

	d.ConnectionManager = &ConnectionManager{
		DataChannel: d.DataChannel,
		Logger:      d.Logger,
	}

	if err := d.NetworkListener.StartListening(wg, port); err != nil {
		d.Logger.Error(err.Error(), false)
		return
	}

	d.ConnectionManager.Listener = d.NetworkListener.Listener
	d.ConnectionManager.StartHandlingConnections()
}
