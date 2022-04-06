package v1

import (
	"g-server/common"
	"g-server/common/cache"
	"g-server/common/errcode"
	logger2 "g-server/common/logger"
	"g-server/common/rdb"
	"g-server/common/response"
	"g-server/common/validate"
	"g-server/server/service"
	"g-server/utils/captcha"
	"g-server/utils/email"
	"github.com/gin-gonic/gin"
)

var logger = logger2.NewLogger()

func Login(c *gin.Context) {
	param := &service.LoginRequest{}
	r := response.NewResponse(c)
	valid, verr := validate.BindAndValid(c, param)
	if !valid {
		logger.Errorf("BindAndValid err: %v", verr)
		r.ToErrorResponse(errcode.InvalidParams.WithDetails(verr.Errors()...))
		return
	}
	if ok := email.ValidEmail(param.UsernameOrEmail); ok {
		//邮箱登录
	} else {
		//用户名登录
	}

	rdb.RDB.HSet(common.Ctx, "user", "1", "online")
}

// @Summary 获取验证码
// @Produce json
// @Param email body string true "邮箱地址"
// @Success 200
// @Router /api/v1/captcha [post]
func NeedCaptcha(c *gin.Context) {
	// TODO 验证邮箱是否存在
	param := &service.NeedCaptchaRequest{}
	r := response.NewResponse(c)
	valid, verr := validate.BindAndValid(c, param)
	emailAddr := param.Email
	if !valid {
		logger.Errorf("BindAndValid err: %v", verr)
		r.ToErrorResponse(errcode.InvalidParams.WithDetails(verr.Errors()...))
		return
	}
	captchaStr := captcha.GetCaptcha()
	go func() {
		err := email.SendEmail(emailAddr, "", "感谢您注册账号", "你的验证码是："+captchaStr, "游戏名字")
		if err != nil {
			logger.Errorf("SendEmail error : %v", err)
		}
	}()
	err := cache.Cache.Set(emailAddr, []byte(captchaStr))
	if err != nil {
		logger.Errorf("CacheSet error : %v", err)
	}
	r.ToResponse(gin.H{
		"msg": "验证码已发送",
	})
}

func ConfirmEmail(c *gin.Context) {
	param := &service.ConfirmEmailRequest{}
	valid, err := validate.BindAndValid(c, param)
	r := response.NewResponse(c)
	if !valid {
		logger.Errorf("BindAndValid err: %v", err)
		r.ToErrorResponse(errcode.InvalidParams.WithDetails(err.Errors()...))
		return
	}
	emailAddr := param.Email
	cCaptcha := param.Captcha
	if entry, err := cache.Cache.Get(emailAddr); err != nil {
		r.ToErrorResponse(errcode.GetCacheFail)
	} else if cCaptcha != string(entry) {
		r.ToErrorResponse(errcode.InvalidCaptcha)
	} else {
		svc := service.New(c.Request.Context())
		err := svc.CreateUser(param)
		if err != nil {
			logger.Errorf("svc.CreateUser err: %v", err)
			r.ToErrorResponse(errcode.ErrorCreateUserFail)
			return
		}
		r := response.NewResponse(c)
		r.ToSuccessResponse("注册成功")
		cache.Cache.Delete(emailAddr)
	}
}
