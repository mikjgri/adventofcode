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
func getMax(cubes []Cube) (int, int, int) {
	maxX, maxY, maxZ := 0, 0, 0
	for _, cube := range cubes {
		if cube.x > maxX {
			maxX = cube.x
		}
		if cube.y > maxY {
			maxY = cube.y
		}
		if cube.z > maxZ {
			maxZ = cube.z
		}
	}
	return maxX, maxY, maxZ
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
	maxX, maxY, maxZ := getMax(cubes)

	canEscape := func(position [3]int, cubes []Cube) bool {
		visitedPositions := map[[3]int]bool{}
		var move func([3]int) bool
		move = func(position [3]int) bool {
			if visitedPositions[position] {
				return false
			}
			visitedPositions[position] = true
			for _, cube2 := range cubes {
				if position[0] == cube2.x && position[1] == cube2.y && position[2] == cube2.z {
					return false
				}
			}
			for _, pos := range position {
				if pos <= 0 {
					return true
				}
			}
			if position[0] >= maxX || position[1] >= maxY || position[2] >= maxZ {
				return true
			}
			return move([3]int{position[0] + 1, position[1], position[2]}) ||
				move([3]int{position[0], position[1] + 1, position[2]}) ||
				move([3]int{position[0], position[1], position[2] + 1}) ||
				move([3]int{position[0] - 1, position[1], position[2]}) ||
				move([3]int{position[0], position[1] - 1, position[2]}) ||
				move([3]int{position[0], position[1], position[2] - 1})
		}
		return move(position)
	}

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
		for i, axis := range clear {
			for s, side := range axis {
				if side {
					val := 0
					if s == 0 {
						val = -1
					} else {
						val = 1
					}
					position := [3]int{cube1.x, cube1.y, cube1.z}
					if i == 0 {
						position[0] += val
					} else if i == 1 {
						position[1] += val
					} else if i == 2 {
						position[2] += val
					}
					if canEscape(position, cubes) {
						clearSides++
					}
				}
			}
		}
	}

	fmt.Println(clearSides)
}
