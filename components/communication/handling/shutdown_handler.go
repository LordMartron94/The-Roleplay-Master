package handling

import "../interpretation"

type ShutdownHandler struct {
	ShutdownSigCh chan struct{}
}

func (sh *ShutdownHandler) Shutdown() *interpretation.Response {
	close(sh.ShutdownSigCh)
	return interpretation.SuccessResponse()
}
