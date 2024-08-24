package interpretation

import "../logging"

type Interpreter struct {
	logger logging.HoornLogger
}

func NewInterpreter(logger logging.HoornLogger) *Interpreter {
	return &Interpreter{logger}
}

// Interpret takes a JSON string as input and returns a Request object.
//
// - If the JSON data is invalid, an error is returned.
func (i *Interpreter) Interpret(jsonData string) (Request, error) {
	i.logger.Info("Interpreting JSON data: "+jsonData, false)

	request, err := RequestFromJsonString(jsonData)

	if err != nil {
		i.logger.Error("Failed to interpret JSON data: "+err.Error(), false)
		return Request{}, err
	}

	return *request, nil
}
