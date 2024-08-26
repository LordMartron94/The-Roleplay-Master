package interpretation

import "../logging"

type Interpreter struct {
	logger    logging.HoornLogger
	channelId string
}

func NewInterpreter(logger logging.HoornLogger, channelId string) *Interpreter {
	return &Interpreter{logger, channelId}
}

// Interpret takes a JSON string as input and returns a Request object.
//
// - If the JSON data is invalid, an error is returned.
func (i *Interpreter) Interpret(jsonData string) (Request, error) {
	i.logger.Info("Interpreting JSON data: "+jsonData, false, i.channelId)

	request, err := RequestFromJsonString(jsonData)

	if err != nil {
		i.logger.Error("Failed to interpret JSON data: "+err.Error(), false, i.channelId)
		return Request{}, err
	}

	return *request, nil
}
