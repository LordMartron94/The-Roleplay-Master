package logging

import "time"

type HoornLogFactory struct {
}

func (factory HoornLogFactory) CreateHoornLog(level LogLevel, message string, logSeparator string) HoornLog {
	var currentTime time.Time = time.Now()

	var formatters []HoornLogFormatterInterface = []HoornLogFormatterInterface{
		HoornLogTextFormatter{},
		NewHoornLogColorFormatter(),
	}

	var hoornLog = NewHoornLog(
		currentTime,
		level,
		message,
		message,
		logSeparator,
	)

	for _, formatter := range formatters {
		hoornLog.formattedMessage = formatter.Format(hoornLog)
	}

	return hoornLog
}
