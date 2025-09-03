package configs

import "fmt"

var conf *config

// Instance de la configuration actuelle ; pourrait être chargée à partir d'un .env
func Instance() *config {
	if conf == nil {
		conf = &config{
			Protocol:   "http",
			Host:       "localhost",
			Port:       8007,
			ScreenSize: 16,
			Debug:      true,
		}
	}
	return conf
}

type config struct {
	Protocol   string `json:"protocol"`
	Host       string `json:"host"`
	Port       int    `json:"port"`
	ScreenSize int    `json:"screenSize"`
	Debug      bool   `json:"debug"`
}

// Retourne "{protocol}://{host}:{port}"
func (c *config) GetHost() string {
	return fmt.Sprintf("%s://%s:%d", c.Protocol, c.Host, c.Port)
}
