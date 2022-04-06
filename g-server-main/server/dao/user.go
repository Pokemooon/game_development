package dao

import (
	"g-server/common/db"
	"g-server/server/model"
)

func (d *Dao) CreateUser(user *model.User) error {
	return db.DB.Create(user).Error
}

func (d *Dao) SelectUserByEmail(email string) (*model.User, error) {
	user := &model.User{}
	err := db.DB.Where("email = ?", email).Where("is_del", 0).Find(user).Error
	return user, err
}

func (d *Dao) SelectUserByUsername(username string) (*model.User, error) {
	user := &model.User{}
	err := db.DB.Where("username = ?", username).Find(user).Error
	return user, err
}
