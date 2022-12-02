package main

import (
	"fmt"
	"io/ioutil"
	"strings"
)

func main() {
	content, _ := ioutil.ReadFile("input.txt")
	lines := strings.Split(string(content), "\n")

	score, rulebook, outcomebook := 0, map[interface{}]int{
		"A": 1, //rock
		"B": 2, //paper
		"C": 3, //scissor
	}, map[interface{}]int{
		"X": -1, //lose
		"Y": 0,  //draw
		"Z": 1,  //win
	}

	for _, line := range lines {
		chars := strings.Split(line, "")
		opponent, outcome := rulebook[chars[0]], outcomebook[chars[2]]
		cheater := 0
		if outcome == 0 {
			score += 3
			cheater = opponent
		} else {
			if outcome == 1 {
				score += 6
			}
			cheater = opponent + outcome
			if cheater < 1 {
				cheater = 3
			} else if cheater > 3 {
				cheater = 1
			}
		}
		score += cheater
	}
	fmt.Print(score)
}
