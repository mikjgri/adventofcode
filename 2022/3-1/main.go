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

	for _, line := range lines {
		if len(strings.TrimSpace(line)) == 0 {
			continue
		}
		line = strings.Replace(line, "\r", "", -1)
		l := len(line)
		comp1, comp2 := line[:l/2], line[l/2:]

		checkedStrings := ""
		for _, item := range strings.Split(comp1, "") {
			if strings.Count(comp2, item) > 0 {
				if strings.Contains(checkedStrings, item) {
					continue
				}
				checkedStrings += item
				totalPriority += priorityBook[item]
			}
		}
	}
	fmt.Print(totalPriority)
}
