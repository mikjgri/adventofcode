package main

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
)

func main() {
	content, _ := ioutil.ReadFile("input.txt")
	var lines = strings.Split(string(content), "\n")
	var (
		highscore int = 0
		lastscore int = 0
	)
	for _, line := range lines {
		if len(strings.TrimSpace(line)) == 0 {
			lastscore = 0
			continue
		}
		var calories, _ = strconv.Atoi(line)
		lastscore += calories
		if lastscore > highscore {
			highscore = lastscore
		}
	}
	fmt.Println(highscore)
}
