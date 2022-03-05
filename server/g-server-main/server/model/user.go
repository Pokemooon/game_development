package model

import "github.com/jinzhu/gorm"

type User struct {
	*Model
	Uuid     string `json:"uuid"`
	Username string `json:"username"`
	Password string `json:"password"`
	Status   string `json:"status"`
	Email    string `json:"email"`
	Avatar   string `json:"avatar"`
}

func (u User) Create(db *gorm.DB) error {
	return db.Create(&u).Error
}
