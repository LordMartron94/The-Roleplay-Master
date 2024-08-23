package logging

type Color struct {
	Text       string
	Background *string
}

type HoornLogColorFormatter struct {
	colorDict   map[LogLevel]Color
	colorHelper ColorHelper
}

func getPointer(str string) *string {
	return &str
}

func NewHoornLogColorFormatter() *HoornLogColorFormatter {
	return &HoornLogColorFormatter{
		colorDict: map[LogLevel]Color{
			DEBUG:    {Text: "#079B00", Background: nil},
			INFO:     {Text: "#9B9B9B", Background: nil},
			WARNING:  {Text: "#FFA300", Background: nil},
			ERROR:    {Text: "#FF0000", Background: nil},
			CRITICAL: {Text: "#FFFFFF", Background: getPointer("#FF0000")},
			DEFAULT:  {Text: "#9B9B9B", Background: nil},
		},
	}
}

func (f HoornLogColorFormatter) Format(hoornLog HoornLog) string {
	var logLevel = hoornLog.GetLogLevel()

	color, found := f.colorDict[logLevel]
	if !found {
		logLevel = DEFAULT
	}

	var textColorHex = color.Text
	var backgroundColorHex *string = color.Background

	var colorizedString string
	if backgroundColorHex != nil {
		colorizedString = f.colorHelper.ColorizeString(hoornLog.GetFormattedMessage(), textColorHex, *backgroundColorHex)
	} else {
		colorizedString = f.colorHelper.ColorizeString(hoornLog.GetFormattedMessage(), textColorHex, "")
	}

	return colorizedString + "\x1b[0m"
}
