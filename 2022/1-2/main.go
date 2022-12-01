package main

import (
	"fmt"
	"io/ioutil"
	"sort"
	"strconv"
	"strings"
)

func main() {
	content, _ := ioutil.ReadFile("input.txt")
	lines := strings.Split(string(content), "\n")

	elves := []int{0}

	for _, line := range lines {
		if len(strings.TrimSpace(line)) == 0 {
			elves = append(elves, 0)
			continue
		}
		calories, _ := strconv.Atoi(line)
		elves[len(elves)-1] += calories
	}
	sort.Ints(elves)
	topPerformers := elves[len(elves)-3:]
	sum := 0
	for _, cal := range topPerformers {
		sum += cal
	}
	fmt.Print(sum)
}
