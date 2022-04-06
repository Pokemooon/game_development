package router

import (
	"g-server/common/middleware"
	v1 "g-server/server/router/api/v1"
	"g-server/server/ws"
	"github.com/gin-gonic/gin"
	swaggerFiles "github.com/swaggo/files"
	ginSwagger "github.com/swaggo/gin-swagger"
)

func NewRouter() *gin.Engine {
	r := gin.New()
	r.Use(gin.Logger())
	r.Use(gin.Recovery())
	r.Use(middleware.Translations())

	r.GET("/", ws.Home)
	r.GET("/echo", ws.Echo)
	/*		r.GET("/echo", func(c *gin.Context) {
			ws.HttpController(c, h)
		})*/
	r.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))
	apiv1 := r.Group("/api/v1")
	{
		apiv1.POST("/captcha", v1.NeedCaptcha)
		apiv1.POST("/confirm", v1.ConfirmEmail)
		apiv1.POST("/login", v1.Login)

		apiv1.GET("create_room/:username", v1.CreateRoom)
		apiv1.GET("room", v1.GetRoomList)
		apiv1.GET("enter_room/:username/:ruuid", v1.EnterRoom)
	}
	return r
}
