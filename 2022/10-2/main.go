package main

import (
	"fmt"
	"io/ioutil"
	"math"
	"strconv"
	"strings"
)

func main() {
	content, _ := ioutil.ReadFile("input.txt")
	lines := strings.Split(string(content), "\n")

	cycles := map[int][]int{}

	execCycle := 1
	for _, line := range lines {
		line = strings.TrimSpace(strings.Replace(line, "\r", "", -1))
		split := strings.Split(line, " ")
		instr := split[0]
		execCycle++
		if instr == "addx" {
			val, _ := strconv.Atoi(split[1])
			cycles[execCycle] = append(cycles[execCycle], val)
			execCycle++
		}
	}

	crtPos := 0
	x := 1
	for i := 1; i < execCycle; i++ {
		xDiff := math.Abs(float64(x - crtPos))
		if xDiff < 2 {
			fmt.Print("#")
		} else {
			fmt.Print(".")
		}
		crtPos++
		if i%40 == 0 {
			crtPos = 0
			fmt.Println()
		}
		mods := cycles[i]

		for _, mod := range mods {
			x += mod
		}
	}

	fmt.Println()
}
