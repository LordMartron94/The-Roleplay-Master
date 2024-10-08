package logging

import (
	"time"
)

type HoornLog struct {
	// logTime is the timestamp of the log message.
	logTime time.Time

	// logLevel is the severity level of the log message.
	logLevel LogLevel

	// logMessage is the raw log message.
	logMessage string

	// formattedMessage is a pre-formatted string for Output. Optional.
	formattedMessage string
}

func (log HoornLog) GetLogLevel() LogLevel {
	return log.logLevel
}

func (log HoornLog) GetLogLevelString() string {
	return log.logLevel.StringifyLogLevel()
}

func (log HoornLog) GetLogTime() time.Time {
	return log.logTime
}

func (log HoornLog) GetLogMessage() string {
	return log.logMessage
}

func (log HoornLog) GetFormattedMessage() string {
	return log.formattedMessage
}
