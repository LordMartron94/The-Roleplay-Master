package logging

import (
	"fmt"
	"strings"
	"time"
)

type HoornLogTextFormatter struct {
}

func getLongestLogLevelLength() int {
	var logLevels []LogLevel = GetAllLogLevels()

	var longestLogLevelLength int = 0
	for _, logLevel := range logLevels {
		if len(logLevel.StringifyLogLevel()) > longestLogLevelLength {
			longestLogLevelLength = len(logLevel.StringifyLogLevel())
		}
	}

	return longestLogLevelLength
}

func (formatter HoornLogTextFormatter) Format(log HoornLog) string {
	var logLevel string = log.GetLogLevelString()

	var formattedMessage string = "[" + log.GetLogTime().Format(time.RFC3339) + "] " + logLevel + " : " + log.GetFormattedMessage()
	formattedMessage = strings.Replace(formattedMessage, logLevel, fmt.Sprintf("%-*s", getLongestLogLevelLength(), logLevel), -1)

	return formattedMessage
}
