package run

import "g-server/server/router"

func Run() {
	r := router.NewRouter()
	r.Run(":8080")
}
