package main

import (
	"fmt"
	"io/ioutil"
	"strings"
)

func toChar(i int) rune {
	return rune('A' - 1 + i)
}
func main() {
	content, _ := ioutil.ReadFile("input.txt")
	lines := strings.Split(string(content), "\n")

	totalPriority, priorityBook := 0, map[string]int{}

	alphabet := "abcdefghijklmnopqrstuvwxyz"
	for i, letter := range strings.Split(alphabet, "") {
		priorityBook[letter] = i + 1
		priorityBook[strings.ToUpper(letter)] = i + 27
	}

	buffer := [3]string{}
	for i, line := range lines {
		if len(strings.TrimSpace(line)) == 0 {
			continue
		}
		line = strings.Replace(line, "\r", "", -1)
		mod := (i + 1) % 3
		buffer[mod] = line
		if mod == 0 {
			for _, item := range strings.Split(buffer[0], "") {
				if strings.Count(buffer[1], item) > 0 && strings.Count(buffer[2], item) > 0 {
					totalPriority += priorityBook[item]
					break
				}
			}
		}
	}
	fmt.Print(totalPriority)
}
