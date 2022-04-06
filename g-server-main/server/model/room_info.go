package model

type RoomInfo struct {
	Uuid  string  `json:"uuid"`
	State string  `json:"state"`
	Count int     `json:"count"`
	Users []*User `json:"users"`
}
