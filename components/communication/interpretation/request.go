package interpretation

import "encoding/json"

type Request struct {
	Source  string   `json:"source"`
	Actions []Action `json:"actions"`
}

func RequestFromJsonString(jsonStr string) (*Request, error) {
	var request Request
	err := json.Unmarshal([]byte(jsonStr), &request)
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
