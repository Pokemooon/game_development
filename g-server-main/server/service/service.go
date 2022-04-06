package service

import (
	"context"
	"g-server/common/db"
	"g-server/server/dao"
)

type Service struct {
	ctx context.Context
	dao *dao.Dao
}

func New(ctx context.Context) Service {
	svc := Service{ctx: ctx}
	svc.dao = dao.New(db.DB)
	return svc
}
