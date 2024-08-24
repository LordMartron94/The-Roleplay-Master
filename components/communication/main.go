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

	shutdownSigCh := make(chan struct{})

	var wg sync.WaitGroup
	wg.Add(1)

	var detector = detection.Detector{Logger: hoornLogger}

	go detector.StartDetectionLoop(&wg, shutdownSigCh)

	c := make(chan os.Signal, 2)
	signal.Notify(c, os.Interrupt, syscall.SIGTERM)

	select {
	case <-c:
		hoornLogger.Info("OS signal catched, Terminating communication layer...", false)
	case <-shutdownSigCh:
		hoornLogger.Info("Shutdown signal received. Terminating communication layer...", false)
	}

	close(shutdownSigCh)
	wg.Wait()
}
