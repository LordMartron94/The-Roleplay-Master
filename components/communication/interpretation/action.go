package interpretation

type Action struct {
	name string
	data string
}

func NewAction(name, data string) *Action {
	return &Action{name, data}
}
