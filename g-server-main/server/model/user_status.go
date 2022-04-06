package model

import "time"

type UserStatus struct {
	*User
	LoginTime time.Time
	State     string // online in_room in_game
}
