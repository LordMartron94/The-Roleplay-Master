package interpretation

type Response struct {
	message string
	code    int
}

func SuccessResponse() *Response {
	return &Response{
		message: "Success",
		code:    200,
	}
}

func InvalidRequestFormat() *Response {
	return &Response{
		message: "Invalid request format. Please check the documentation and provide the required data.",
		code:    400,
	}
}

func ErrorResponseDefault() *Response {
	return &Response{
		message: "An unknown error has occurred, please report this to the developer immediately along with the logs.\nYou can find the log location in the software documentation.",
		code:    -1,
	}
}

func ErrorResponse(message string, code int) *Response {
	return &Response{
		message: message,
		code:    code,
	}
}
