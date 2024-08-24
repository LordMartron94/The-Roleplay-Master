package interpretation

import (
	"encoding/json"
	"errors"
)

type Request struct {
	Source  string   `json:"source"`
	Actions []Action `json:"actions"`
}

func validateRequest(request *Request) error {
	if request.Source == "" {
		return errors.New("source field is required")
	}
	if len(request.Actions) == 0 {
		return errors.New("at least one action is required")
	}

	for _, action := range request.Actions {
		err := action.validateAction()

		if err != nil {
			return err
		}
	}

	return nil
}

func RequestFromJsonString(jsonStr string) (*Request, error) {
	var request Request
	err := json.Unmarshal([]byte(jsonStr), &request)
	if err != nil {
		return nil, err
	}
	err = validateRequest(&request)
	if err != nil {
		return nil, err
	}
	return &request, nil
}

func NewRequest(source string, actions []Action) *Request {
	return &Request{
		Source:  source,
		Actions: actions,
	}
}
