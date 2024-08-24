package interpretation

import "errors"

type Action struct {
	Name string `json:"name"`
	Data string `json:"data"`
}

func (a *Action) validateAction() error {
	if a.Name == "" {
		return errors.New("action name cannot be empty")
	}

	return nil
}

func NewAction(name, data string) *Action {
	return &Action{name, data}
}
