package logging

import "fmt"

type DefaultHoornLogOutput struct {
}

func (o DefaultHoornLogOutput) Output(log HoornLog) {
	fmt.Println(log.GetFormattedMessage())
}
