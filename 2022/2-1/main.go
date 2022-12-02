package main

import (
	"fmt"
	"io/ioutil"
	"strings"
)

func main() {
	content, _ := ioutil.ReadFile("input.txt")
	lines := strings.Split(string(content), "\n")

	score, rulebook := 0, map[interface{}]int{
		"A": 1, //rock
		"B": 2, //paper
		"C": 3, //scissor
		"X": 1, //rock
		"Y": 2, //paper
		"Z": 3, //scissor
	}

	for _, line := range lines {
		chars := strings.Split(line, "")
		opponent, cheater := rulebook[chars[0]], rulebook[chars[2]]

		score += cheater

		difference := cheater - opponent
		if difference == 0 {
			score += 3
		} else {
			if (difference % 3) == 1 {
				score += 6
			}
		}
	}
	fmt.Print(score)
}
