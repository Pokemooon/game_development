package dao

import (
	"g-server/common/db"
	"g-server/server/model"
)

func (d *Dao) CreateUser(user *model.User) error {
	return user.Create(db.DB)
}
