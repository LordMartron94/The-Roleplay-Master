package interpretation

type Action struct {
	Name string `json:"name"`
	Data string `json:"data"`
}

func NewAction(name, data string) *Action {
	return &Action{name, data}
}
