package logging

import "time"

type HoornLogFactory struct {
}

func (factory HoornLogFactory) CreateHoornLog(level LogLevel, message string) HoornLog {
	var currentTime time.Time = time.Now()

	var formatters []HoornLogFormatterInterface = []HoornLogFormatterInterface{
		HoornLogTextFormatter{},
		NewHoornLogColorFormatter(),
	}

	var hoornLog HoornLog = HoornLog{
		logTime:          currentTime,
		logLevel:         level,
		logMessage:       message,
		formattedMessage: message,
	}

	for _, formatter := range formatters {
		hoornLog.formattedMessage = formatter.Format(hoornLog)
	}

	return hoornLog
}
