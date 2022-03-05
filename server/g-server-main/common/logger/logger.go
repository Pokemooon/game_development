package logger

import (
	"github.com/sirupsen/logrus"
	"os"
)

func init() {
	setupLogger()
}

func setupLogger() {
	// Log as JSON instead of the default ASCII formatter.

	//logrus.SetFormatter(&logrus.JSONFormatter{})

	// Output to stdout instead of the default stderr
	// Can be any io.Writer, see below for File example
	logrus.SetOutput(os.Stdout)
	// Only logger the warning severity or above.
	//logger.SetLevel(logger.WarnLevel)
	logrus.SetReportCaller(true)
}

func NewLogger() *logrus.Logger {
	return logrus.New()
}
