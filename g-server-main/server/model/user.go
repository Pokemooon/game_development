package model

type User struct {
	*Model
	Uuid     string `json:"uuid"`
	Username string `json:"username"`
	Password string `json:"password"`
	Status   string `json:"status"`
	Email    string `json:"email"`
	Avatar   string `json:"avatar"`
}
