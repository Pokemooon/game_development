package main

import (
	_ "g-server/common/setting"
	"g-server/server"
)

func main() {
	/*	db := db2.DB
		db.Create(&model.User{
			Model:    &model.Model{},
			Uuid:     "",
			Username: "a",
			Password: "b",
			Status:   "",
			Email:    "",
			Avatar:   "",
		})*/
	server.Run()
}
