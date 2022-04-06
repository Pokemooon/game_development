package model

type Room struct {
	Uuid  string  `json:"uuid"`
	State string  `json:"state"`
	Count int     `json:"count"`
	Users []*User `json:"users"`
	Hub   *Hub    `json:"hub"`
}
