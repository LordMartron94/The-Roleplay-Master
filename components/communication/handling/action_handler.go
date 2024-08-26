package handling

import (
	"../interpretation"
	"../logging"
	"fmt"
)

type ActionHandler struct {
	Logger           logging.HoornLogger
	ShutdownSigCh    chan struct{}
	ChannelId        string
	actionHandlerMap map[string]func() *interpretation.Response
}

func NewActionHandler(logger logging.HoornLogger, shutdownSigCh chan struct{}, channelId string) *ActionHandler {
	actionHandlerMap := make(map[string]func() *interpretation.Response)
	shutdownHandler := &ShutdownHandler{
		ShutdownSigCh: shutdownSigCh,
	}
	actionHandlerMap["Shutdown"] = shutdownHandler.Shutdown

	return &ActionHandler{
		Logger:           logger,
		ShutdownSigCh:    shutdownSigCh,
		ChannelId:        channelId,
		actionHandlerMap: actionHandlerMap,
	}
}

func (h *ActionHandler) HandleRequest(request *interpretation.Request) []*interpretation.Response {
	h.Logger.Debug(fmt.Sprintf("Handling request for source %s", request.Source), false, h.ChannelId)

	responses := make([]*interpretation.Response, 0)

	for actionName, actionFunc := range h.actionHandlerMap {
		for _, action := range request.Actions {
			if action.Name == actionName {
				resp := actionFunc()
				responses = append(responses, resp)
			} else {
				h.Logger.Debug(fmt.Sprintf("Received unsupported action %s for source %s", action.Name, request.Source), false, h.ChannelId)
				resp := interpretation.InvalidAction(action.Name)
				responses = append(responses, resp)
			}
		}
	}

	return responses
}
