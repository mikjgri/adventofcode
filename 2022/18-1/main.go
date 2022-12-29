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

type Cube struct {
	x, y, z int
}

func getCubes(lines []string) []Cube {
	cubes := []Cube{}
	for _, line := range lines {
		var x, y, z int
		fmt.Sscanf(line, "%d,%d,%d", &x, &y, &z)
		cubes = append(cubes, Cube{x, y, z})
	}
	return cubes
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
	cubes := getCubes(lines)

	clearSides := 0
	for _, cube1 := range cubes {
		clear := [3][]bool{{true, true}, {true, true}, {true, true}}
		for _, cube2 := range cubes {
			if cube1 == cube2 {
				continue
			}
			if cube1.x == cube2.x && cube1.y == cube2.y {
				diffCheck(cube1.z, cube2.z, &clear[2])
			}
			if cube1.x == cube2.x && cube1.z == cube2.z {
				diffCheck(cube1.y, cube2.y, &clear[1])
			}
			if cube1.y == cube2.y && cube1.z == cube2.z {
				diffCheck(cube1.x, cube2.x, &clear[0])
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
