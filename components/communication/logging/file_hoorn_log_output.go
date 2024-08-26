package logging

import (
	"fmt"
	"log"
	"os"
	"path/filepath"
	"sort"
	"strconv"
	"strings"
)

type FileHoornLogOutput struct {
	logDirectory    string
	maxLogsToKeep   int
	createDirectory bool
}

func NewFileHoornLogOutput(logDirectory string, maxLogsToKeep int) *FileHoornLogOutput {
	var fileHoornLogOutput = &FileHoornLogOutput{
		logDirectory:    filepath.Clean(logDirectory),
		maxLogsToKeep:   maxLogsToKeep,
		createDirectory: true,
	}

	fileHoornLogOutput.initialize()

	return fileHoornLogOutput
}

func NewFileHoornLogOutputWithoutCreateDir(logDirectory string, maxLogsToKeep int) *FileHoornLogOutput {
	var fileHoornLogOutput = &FileHoornLogOutput{
		logDirectory:    filepath.Clean(logDirectory),
		maxLogsToKeep:   maxLogsToKeep,
		createDirectory: false,
	}

	fileHoornLogOutput.initialize()

	return fileHoornLogOutput
}

func (fhl *FileHoornLogOutput) initialize() {
	fhl.validateDirectory(fhl.logDirectory)
	fhl.incrementLogs()
}

func (fhl *FileHoornLogOutput) validateDirectory(directory string) error {
	_, err := os.Stat(directory)
	if os.IsNotExist(err) {
		if fhl.createDirectory {
			errDir := os.MkdirAll(directory, 0755)
			if errDir != nil {
				return errDir
			}
			return nil
		}
		return fmt.Errorf("log directory %v does not exist", directory)
	}
	return nil
}

// getSubDirectories will retrieve a list of subdirectories
func (fhl *FileHoornLogOutput) getSubDirectories() ([]os.DirEntry, error) {
	return os.ReadDir(fhl.logDirectory)
}

// handleLogFiles will handle each log file
func handleLogFile(dirPath, file string, maxLogsToKeep int) error {
	extension := filepath.Ext(file)
	name := strings.TrimSuffix(file, extension)
	splitName := strings.Split(name, "_")
	logNumber, err := strconv.Atoi(splitName[len(splitName)-1])
	if err != nil {
		return err
	}

	if logNumber+1 > maxLogsToKeep {
		err := os.Remove(file)
		if err != nil {
			return err
		}
		return nil
	}

	err = os.Rename(file, filepath.Join(dirPath, fmt.Sprintf("log_%v.txt", logNumber+1)))
	if err != nil {
		return err
	}

	return nil
}

// incrementLogs will call smaller functions to increment logs
func (fhl *FileHoornLogOutput) incrementLogs() error {
	subDirectories, err := fhl.getSubDirectories()
	if err != nil {
		return err
	}

	for _, subDir := range subDirectories {
		if subDir.IsDir() {
			dirPath := filepath.Join(fhl.logDirectory, subDir.Name())
			children, err := getFileChildrenPaths(dirPath, ".txt")
			if err != nil {
				return err
			}

			sort.Sort(sort.Reverse(sort.StringSlice(children)))

			for _, file := range children {
				err := handleLogFile(dirPath, file, fhl.maxLogsToKeep)
				if err != nil {
					return err
				}
			}
		}
	}

	return nil
}

func (fhl *FileHoornLogOutput) getPathToLogTo(logSeparator string) string {
	var directory = filepath.Join(fhl.logDirectory, logSeparator)
	fhl.validateDirectory(directory)

	return filepath.Join(directory, fmt.Sprintf("log_%v.txt", 1))
}

func getFileChildrenPaths(directory string, extension string) ([]string, error) {
	var files []string
	fileInfo, err := os.ReadDir(directory)
	if err != nil {
		return files, err
	}
	for _, file := range fileInfo {
		if !file.IsDir() && filepath.Ext(file.Name()) == extension {
			files = append(files, filepath.Join(directory, file.Name()))
		}
	}
	return files, nil
}

func (fhl *FileHoornLogOutput) Output(hoornLog HoornLog) {
	var formatter = HoornLogTextFormatter{}
	formattedLog := formatter.Format(hoornLog)

	var logDirectory = fhl.getPathToLogTo(hoornLog.logSeparator)

	f, err := os.OpenFile(logDirectory, os.O_APPEND|os.O_CREATE|os.O_WRONLY, 0644)
	if err != nil {
		log.Fatal(err)
	}

	defer f.Close()

	if _, err := f.WriteString(formattedLog + "\n"); err != nil {
		log.Fatal(err)
	}
}
