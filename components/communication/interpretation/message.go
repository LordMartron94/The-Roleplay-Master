package interpretation

type Request struct {
	source  string
	actions []Action
}

func NewRequest(source string, actions []Action) *Request {
	return &Request{
		source:  source,
		actions: actions,
	}
}
