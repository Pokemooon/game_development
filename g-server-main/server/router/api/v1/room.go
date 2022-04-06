package v1

import (
	"g-server/common/response"
	"g-server/server/model"
	"g-server/server/service"
	"g-server/server/ws"
	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
)

func CreateRoom(c *gin.Context) {
	svc := &service.Service{}
	param := &service.GetUserByUsernameRequest{}
	param.Username = c.Param("username")
	curUser, _ := svc.GetUserByUsername(param)
	// 创建房间，进入缓存
	ruuid := uuid.New().String()
	// 为房间创建一个Hub
	hub := model.NewHub()
	room := &model.Room{
		Uuid:  ruuid,
		State: "ready",
		Count: 1,
		Users: []*model.User{curUser},
		Hub:   hub,
	}
	go model.Run(room)
	model.RoomMap[ruuid] = room
	// WS 开始
	ws.RoomHandler(room.Hub, c.Writer, c.Request, curUser)
}

func GetRoomList(c *gin.Context) {
	roomList := make([]*model.RoomInfo, 0)
	for _, v := range model.RoomMap {
		var v = &model.RoomInfo{
			Uuid:  v.Uuid,
			State: v.State,
			Count: v.Count,
			Users: v.Users,
		}
		roomList = append(roomList, v)
	}
	r := response.NewResponse(c)
	r.ToResponse(roomList)
}

func EnterRoom(c *gin.Context) {
	//TODO: 不能重复进入房间
	svc := &service.Service{}
	// 根据房间的uuid拿到要加入的房间
	ruuid := c.Param("ruuid")
	param := &service.GetUserByUsernameRequest{}
	param.Username = c.Param("username")
	curUser, _ := svc.GetUserByUsername(param)
	var room *model.Room
	room = model.RoomMap[ruuid]
	room.Users = append(room.Users, curUser)
	// 创建Client
	ws.RoomHandler(room.Hub, c.Writer, c.Request, curUser)
}
