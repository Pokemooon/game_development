package server

import (
	logger2 "g-server/common/logger"
	"g-server/server/router"
	"github.com/gin-gonic/gin"
)

var logger = logger2.NewLogger()

func Run() {
	r := router.NewRouter()
	gin.SetMode(gin.DebugMode)
	err := r.Run(":8080")
	if err != nil {
		logger.Fatal(err)
	}
}
