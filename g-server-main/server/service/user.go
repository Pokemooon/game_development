package service

import (
	"g-server/server/model"
	"github.com/google/uuid"
)

type CreateUserRequest struct {
	Username string `form:"username" binding:"required"`
	Password string `form:"password" binding:"required"`
	Email    string `form:"email" binding:"required"`
}

type ConfirmEmailRequest struct {
	Email    string `form:"email" binding:"required"`
	Captcha  string `form:"captcha" binding:"required"`
	Username string `form:"username" binding:"required"`
	Password string `form:"password" binding:"required"`
}

type NeedCaptchaRequest struct {
	Email string `form:"email" binding:"required"`
}

type LoginRequest struct {
	UsernameOrEmail string `form:"usernameOrEmail" binding:"required"`
	Password        string `form:"password" binding:"required"`
}

type GetUserByUsernameRequest struct {
	Username string
}

func (s *Service) CreateUser(param *ConfirmEmailRequest) error {
	guuid := uuid.New().String()
	curUser := &model.User{
		Model:    &model.Model{},
		Username: param.Username,
		Password: param.Password,
		Email:    param.Email,
		Uuid:     guuid,
	}
	return s.dao.CreateUser(curUser)
}

func (s *Service) GetUserByUsername(param *GetUserByUsernameRequest) (*model.User, error) {
	return s.dao.SelectUserByUsername(param.Username)
}
