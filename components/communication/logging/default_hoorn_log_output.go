package logging

import "fmt"

type DefaultHoornLogOutput struct {
}

func (o DefaultHoornLogOutput) Output(log HoornLog) {
	fmt.Println(fmt.Sprintf("[%s] %s", log.logSeparator, log.GetFormattedMessage()))
}
