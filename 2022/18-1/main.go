package main

import (
	"fmt"
	"os"
	"strings"
)

func readLinesAndTrim(file string) []string {
	content, _ := os.ReadFile(file)
	rawLines := strings.Split(string(content), "\n")
	lines := []string{}
	for _, line := range rawLines {
		lines = append(lines, strings.TrimSpace(strings.Replace(line, "\r", "", -1)))
	}
	return lines
}

type Droplet struct {
	x, y, z int
}

func getDroplets(lines []string) []Droplet {
	droplets := []Droplet{}
	for _, line := range lines {
		var x, y, z int
		fmt.Sscanf(line, "%d,%d,%d", &x, &y, &z)
		droplets = append(droplets, Droplet{x, y, z})
	}
	return droplets
}
func diffCheck(a, b int, res *[]bool) {
	diff := a - b
	if diff == 1 {
		(*res)[0] = false
	} else if diff == -1 {
		(*res)[1] = false
	}
}

func main() {
	lines := readLinesAndTrim("input.txt")
	droplets := getDroplets(lines)

	clearSides := 0
	for _, dropletToCheck := range droplets {
		clear := [3][]bool{{true, true}, {true, true}, {true, true}}
		for _, otherDroplet := range droplets {
			if dropletToCheck == otherDroplet {
				continue
			}
			if dropletToCheck.x == otherDroplet.x && dropletToCheck.y == otherDroplet.y {
				diffCheck(dropletToCheck.z, otherDroplet.z, &clear[2])
			}
			if dropletToCheck.x == otherDroplet.x && dropletToCheck.z == otherDroplet.z {
				diffCheck(dropletToCheck.y, otherDroplet.y, &clear[1])
			}
			if dropletToCheck.y == otherDroplet.y && dropletToCheck.z == otherDroplet.z {
				diffCheck(dropletToCheck.x, otherDroplet.x, &clear[0])
			}
		}
		for _, axis := range clear {
			for _, side := range axis {
				if side {
					clearSides++
				}
			}
		}
	}

	fmt.Println(clearSides)
}
