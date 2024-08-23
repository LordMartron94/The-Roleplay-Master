package main

import (
	"./logging"
	"log"
	"os"
	"path/filepath"
)

func getLogger() logging.HoornLogger {
	var userConfigDir, err = os.UserHomeDir()
	if err != nil {
		log.Fatalf("Failed to get user config directory: %v", err)
	}

	var dir = filepath.Join(userConfigDir, "AppData", "Local")
	var logDir = dir + "\\The Roleplay Master\\logs\\communication_layer\\"

	return logging.NewHoornLogger(
		logging.INFO,
		logging.DefaultHoornLogOutput{},
		logging.NewFileHoornLogOutput(
			logDir,
			5))
}

func main() {
	var hoornLogger logging.HoornLogger = getLogger()
	hoornLogger.Debug("This is a debug message", false)
	hoornLogger.Info("This is an info message", false)
	hoornLogger.Warn("This is a warning message", false)
	hoornLogger.Error("This is an error message", false)
	hoornLogger.Critical("This is a very important critical message", false)
	hoornLogger.Debug("This debug message will always be logged", true)
}
