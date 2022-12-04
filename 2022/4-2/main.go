package main

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
)

type WorkOrder struct {
	FromSection int
	ToSection   int
}

func getWorkRange(input string) WorkOrder {
	split := strings.Split(input, "-")
	s1, _ := strconv.Atoi(split[0])
	s2, _ := strconv.Atoi(split[1])
	return WorkOrder{
		FromSection: s1,
		ToSection:   s2,
	}
}
func main() {
	content, _ := ioutil.ReadFile("input.txt")
	lines := strings.Split(string(content), "\n")

	overlappingPairs := 0
	for _, line := range lines {
		line = strings.Replace(line, "\r", "", -1)
		elves := strings.Split(line, ",")
		elf1, elf2 := getWorkRange(elves[0]), getWorkRange(elves[1])
		if elf1.FromSection <= elf2.ToSection && elf1.ToSection >= elf2.FromSection {
			overlappingPairs++
			continue
		}
	}
	fmt.Print(overlappingPairs)
}
