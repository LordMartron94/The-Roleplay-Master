package main

import (
	"./detection"
	"./logging"
	"log"
	"os"
	"os/signal"
	"path/filepath"
	"sync"
	"syscall"
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
	var hoornLogger = getLogger()
	hoornLogger.Info("Starting communication layer...", false)

	shutdownCh := make(chan struct{})

	var wg sync.WaitGroup
	wg.Add(1)

	var detector = detection.Detector{Logger: hoornLogger, ShutdownCh: shutdownCh}

	go detector.StartDetectionLoop(&wg)

	c := make(chan os.Signal, 1)
	signal.Notify(c, os.Interrupt, syscall.SIGTERM)

	<-c

	hoornLogger.Info("Terminating communication layer...", false)
	close(shutdownCh)

	wg.Wait()
}
