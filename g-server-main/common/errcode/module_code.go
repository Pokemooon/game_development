package errcode

var (
	ErrorCreateUserFail = NewError(20000001, "创建用户失败")
	InvalidCaptcha      = NewError(10000008, "验证码错误")
	GetCacheFail        = NewError(30000001, "获取缓存失败")
)
