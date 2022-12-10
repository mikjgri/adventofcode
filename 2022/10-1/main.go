package main

import (
	"fmt"
	"io/ioutil"
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

	x := 1
	sum := 0
	for i := 1; i <= execCycle; i++ {
		if (i-20)%40 == 0 {
			fmt.Printf("%d", x*i)
			fmt.Println()
			sum += x * i
		}
		mods := cycles[i]
		for _, mod := range mods {
			x += mod
		}
	}

	fmt.Println(sum)
}
