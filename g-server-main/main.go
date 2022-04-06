package main

import (
	_ "g-server/common/setting"
	"g-server/server"
	_ "g-server/server/model"
)

func main() {
	server.Run()
}
