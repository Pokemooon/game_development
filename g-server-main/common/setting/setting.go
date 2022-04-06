package setting

import (
	logger2 "g-server/common/logger"
	"github.com/spf13/viper"
	"os"
	"path/filepath"
)

func init() {
	setupSetting()
}

var logger = logger2.NewLogger()

var DatabaseSetting *DatabaseSettingS
var EmailSetting *EmailSettingS
var AppSetting *AppSettingS
var RedisSetting *RedisSettingS

func setupSetting() {
	setting, err := NewSetting()
	if err != nil {
		logger.Fatal(err)
	}
	err = setting.ReadSection("Database", &DatabaseSetting)
	if err != nil {
		logger.Fatal(err)
	}
	err = setting.ReadSection("Email", &EmailSetting)
	if err != nil {
		logger.Fatal(err)
	}
	err = setting.ReadSection("App", &AppSetting)
	if err != nil {
		logger.Fatal(err)
	}
	err = setting.ReadSection("Redis", &RedisSetting)
	if err != nil {
		logger.Fatal(err)
	}
}

type Setting struct {
	vp *viper.Viper
}

func NewSetting() (*Setting, error) {
	vp := viper.New()
	vp.SetConfigName("config")
	configDir := GetConfigPath()
	vp.AddConfigPath(configDir)
	vp.SetConfigType("yaml")
	err := vp.ReadInConfig()
	if err != nil {
		return nil, err
	}
	return &Setting{
		vp: vp,
	}, nil
}

func GetConfigPath() string {
	ex, err := os.Executable()
	if err != nil {
		panic(err)
	}
	exPath := filepath.Dir(ex)
	rootPath, err := filepath.EvalSymlinks(exPath)
	configPath := filepath.Join(rootPath, "config")
	if err != nil {
		logger.Fatal(err)
	}
	return configPath
}

func GetRootPath() string {
	ex, err := os.Executable()
	if err != nil {
		panic(err)
	}
	exPath := filepath.Dir(ex)
	rootPath, err := filepath.EvalSymlinks(exPath)
	if err != nil {
		logger.Fatal(err)
	}
	return rootPath
}
