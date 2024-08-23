package main

import "./logging"

func main() {
	var hoornLogger logging.HoornLogger = logging.NewHoornLogger(logging.INFO)
	hoornLogger.Debug("This is a debug message", false)
	hoornLogger.Info("This is an info message", false)
	hoornLogger.Warn("This is a warning message", false)
	hoornLogger.Error("This is an error message", false)
	hoornLogger.Critical("This is a very important critical message", false)
	hoornLogger.Debug("This debug message will always be logged", true)
}
