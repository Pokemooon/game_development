package rdb

import (
	"g-server/common/setting"
	"github.com/go-redis/redis/v8"
)

var RDB = NewRdb()

func NewRdb() *redis.Client {
	return redis.NewClient(&redis.Options{
		Addr:     setting.RedisSetting.Addr,
		Password: setting.RedisSetting.Password, // no password set
		DB:       setting.RedisSetting.DB,       // use default DB
	})
}
