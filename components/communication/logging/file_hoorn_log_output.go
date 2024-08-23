package logging

import (
	"fmt"
	"io/ioutil"
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
	pathToLogTo     string
}

func NewFileHoornLogOutput(logDirectory string, maxLogsToKeep int) *FileHoornLogOutput {
	var fileHoornLogOutput = &FileHoornLogOutput{
		logDirectory:    filepath.Clean(logDirectory),
		maxLogsToKeep:   maxLogsToKeep,
		createDirectory: true,
		pathToLogTo:     "",
	}

	fileHoornLogOutput.initialize()

	return fileHoornLogOutput
}

func NewFileHoornLogOutputWithoutCreateDir(logDirectory string, maxLogsToKeep int) *FileHoornLogOutput {
	var fileHoornLogOutput = &FileHoornLogOutput{
		logDirectory:    filepath.Clean(logDirectory),
		maxLogsToKeep:   maxLogsToKeep,
		createDirectory: false,
		pathToLogTo:     "",
	}

	fileHoornLogOutput.initialize()

	return fileHoornLogOutput
}

func (fhl *FileHoornLogOutput) initialize() {
	fhl.pathToLogTo = fhl.getPathToLogTo()
	fhl.validateDirectory(fhl.createDirectory)
	fhl.incrementLogs()
}

func (fhl *FileHoornLogOutput) validateDirectory(createDirectory bool) error {
	_, err := os.Stat(fhl.logDirectory)
	if os.IsNotExist(err) {
		if createDirectory {
			errDir := os.MkdirAll(fhl.logDirectory, 0755)
			if errDir != nil {
				return errDir
			}
			return nil
		}
		return fmt.Errorf("log directory %v does not exist", fhl.logDirectory)
	}
	return nil
}

func (fhl *FileHoornLogOutput) incrementLogs() error {
	// Assuming getFileChildrenPaths is created as a separate function
	children, err := getFileChildrenPaths(fhl.logDirectory, ".txt")
	if err != nil {
		return err
	}

	sort.Sort(sort.Reverse(sort.StringSlice(children)))

	for _, file := range children {
		extension := filepath.Ext(file)
		name := strings.TrimSuffix(file, extension)
		splitName := strings.Split(name, "_")
		logNumber, err := strconv.Atoi(splitName[len(splitName)-1])
		if err != nil {
			return err
		}

		if logNumber+1 > fhl.maxLogsToKeep {
			err := os.Remove(file)
			if err != nil {
				return err
			}
			continue
		}

		err = os.Rename(file, filepath.Join(fhl.logDirectory, fmt.Sprintf("log_%v.txt", logNumber+1)))
		if err != nil {
			return err
		}
	}
	return nil
}

func (fhl *FileHoornLogOutput) getPathToLogTo() string {
	return filepath.Join(fhl.logDirectory, fmt.Sprintf("log_%v.txt", 1))
}

func getFileChildrenPaths(directory string, extension string) ([]string, error) {
	var files []string
	fileInfo, err := ioutil.ReadDir(directory)
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

	f, err := os.OpenFile(fhl.pathToLogTo, os.O_APPEND|os.O_CREATE|os.O_WRONLY, 0644)
	if err != nil {
		log.Fatal(err)
	}

	defer f.Close()

	if _, err := f.WriteString(formattedLog + "\n"); err != nil {
		log.Fatal(err)
	}
}
