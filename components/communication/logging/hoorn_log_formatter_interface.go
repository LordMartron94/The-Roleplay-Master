package logging

type HoornLogFormatterInterface interface {
	Format(hoornLog HoornLog) string
}
