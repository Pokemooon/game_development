package model

import (
	logger2 "g-server/common/logger"
	"time"
)

var logger = logger2.NewLogger()

type Hub struct {
	Clients    map[*Client]bool `json:"clients"`
	Broadcast  chan []byte      `json:"broadcast"`
	Register   chan *Client     `json:"register"`
	Unregister chan *Client     `json:"unregister"`
}

func NewHub() *Hub {
	return &Hub{
		Clients:    make(map[*Client]bool),
		Broadcast:  make(chan []byte),
		Register:   make(chan *Client),
		Unregister: make(chan *Client),
	}
}

func Run(room *Room) {
	tick := time.Tick(5 * time.Second)
	h := room.Hub

loop:
	for {
		select {
		case client := <-h.Register:
			h.Clients[client] = true
			logger.Infoln("client registered:", client.Username)
		case client := <-h.Unregister:
			if _, ok := h.Clients[client]; ok {
				logger.Infoln("client unregistered:", client.Username)
				delete(h.Clients, client)
				close(client.Send)
			}
		case message := <-h.Broadcast:
			for client := range h.Clients {
				select {
				case client.Send <- message:
				default:
					close(client.Send)
					delete(h.Clients, client)
				}
			}
			logger.Infof("message Broadcast from: %v\n", message)
		case t := <-tick:
			if len(h.Clients) == 0 {
				logger.Println("everyone has left this room, hub will stop")
				delete(RoomMap, room.Uuid)
				break loop
			}
			logger.Infof("hub alive: %v", t)
		}
	}
}
