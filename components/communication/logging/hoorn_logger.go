package logging

type HoornLogger struct {
	// outputs is a list of HoornLogOutputInterface objects that will handle the logging.
	outputs []HoornLogOutputInterface
	// minLevel is the minimum log level required for a message to be logged.
	minLevel LogLevel

	// hoornLogFactory is a HoornLogFactoryInterface object that will be used to create new HoornLog objects.
	hoornLogFactory HoornLogFactory
}

func NewHoornLogger(minLevel LogLevel, outputs ...HoornLogOutputInterface) HoornLogger {
	if len(outputs) == 0 {
		outputs = []HoornLogOutputInterface{DefaultHoornLogOutput{}}
	}

	return HoornLogger{
		minLevel:        minLevel,
		outputs:         outputs,
		hoornLogFactory: HoornLogFactory{},
	}
}

func (hL HoornLogger) canOutput(level LogLevel) bool {
	return level >= hL.minLevel
}

func (hL HoornLogger) log(level LogLevel, message string, forceShow bool) {
	if !hL.canOutput(level) && !forceShow {
		return
	}

	var hoornLog = hL.hoornLogFactory.CreateHoornLog(level, message)

	for _, output := range hL.outputs {
		output.Output(hoornLog)
	}
}

func (hL HoornLogger) SetMinLevel(level LogLevel) {
	hL.minLevel = level
}

func (hL HoornLogger) Debug(message string, forceShow bool) {
	hL.log(DEBUG, message, forceShow)
}

func (hL HoornLogger) Info(message string, forceShow bool) {
	hL.log(INFO, message, forceShow)
}

func (hL HoornLogger) Warn(message string, forceShow bool) {
	hL.log(WARNING, message, forceShow)
}

func (hL HoornLogger) Error(message string, forceShow bool) {
	hL.log(ERROR, message, forceShow)
}

func (hL HoornLogger) Critical(message string, forceShow bool) {
	hL.log(CRITICAL, message, forceShow)
}
