package main

import (
	"fmt"
	"io/ioutil"
	"math"
	"strconv"
	"strings"
)

func readLinesAndTrim(file string) []string {
	content, _ := ioutil.ReadFile(file)
	rawLines := strings.Split(string(content), "\n")
	lines := []string{}
	for _, line := range rawLines {
		lines = append(lines, strings.TrimSpace(strings.Replace(line, "\r", "", -1)))
	}
	return lines
}
func build2dArray(minX, minY, maxX, maxY int) [][]string {
	var result [][]string
	for y := minY; y <= maxY; y++ {
		line := []string{}
		for x := minX; x <= maxX; x++ {
			line = append(line, ".")
		}
		result = append(result, line)
	}
	return result
}

type Stone struct {
	x, y int
}

func getStoneStructures(lines []string) [][]Stone {
	var result [][]Stone
	for _, line := range lines {
		stoneStructure := []Stone{}
		split := strings.Split(line, "->")
		for _, stone := range split {
			stone = strings.TrimSpace(stone)
			sSplit := strings.Split(stone, ",")
			x, _ := strconv.Atoi(sSplit[0])
			y, _ := strconv.Atoi(sSplit[1])
			stoneStructure = append(stoneStructure, Stone{x: x, y: y})
		}
		result = append(result, stoneStructure)
	}
	return result
}
func getMinMaxXY(stoneStructures [][]Stone) (int, int, int, int) {
	minX, minY := math.MaxInt, math.MaxInt
	maxX, maxY := math.MinInt, math.MinInt
	for _, stoneStructure := range stoneStructures {
		for _, stone := range stoneStructure {
			if stone.x > maxX {
				maxX = stone.x
			}
			if stone.x < minX {
				minX = stone.x
			}
			if stone.y > maxY {
				maxY = stone.y
			}
			if stone.y < minY {
				minY = stone.y
			}
		}
	}
	return minX, minY, maxX, maxY
}

func main() {
	lines := readLinesAndTrim("input.txt")

	stoneStructures := getStoneStructures(lines)
	_, _, maxX, maxY := getMinMaxXY(stoneStructures)

	array := build2dArray(maxX*-1, 0, maxX*2, maxY+2)
	for i := 0; i <= len(array[0])-1; i++ {
		array[len(array)-1][i] = "#"
	}
	array[0][500] = "+"
	for _, stoneStructure := range stoneStructures {
		var prevStone Stone
		for _, stone := range stoneStructure {
			array[stone.y][stone.x] = "#"
			if prevStone != (Stone{}) {
				xDiff, yDiff := stone.x-prevStone.x, stone.y-prevStone.y
				for xDiff != 0 {
					array[prevStone.y][prevStone.x+xDiff] = "#"
					if xDiff > 0 {
						xDiff--
					} else {
						xDiff++
					}
				}
				for yDiff != 0 {
					array[prevStone.y+yDiff][prevStone.x] = "#"
					if yDiff > 0 {
						yDiff--
					} else {
						yDiff++
					}
				}
			}
			prevStone = stone
		}
	}
	grainsLaidToRest := 0
	visualizeArray := func() {
		fmt.Println("\033[2J")
		for y := 0; y <= len(array)-1; y++ {
			for x := 500 - len(array); x <= 500+len(array); x++ {
				fmt.Print(array[y][x])
			}
			fmt.Println()
		}
		fmt.Println()
		fmt.Printf("Sand grains laid to rest: %d", grainsLaidToRest)
	}
	visualizeArray()
	spring := Stone{x: 500, y: 0}
	sandCoveringSpring := false
	for !sandCoveringSpring {
		grainsLaidToRest++
		sand := spring
		sandMoving := true
		prevSand := sand
		for sandMoving {
			if array[sand.y+1][sand.x] == "." {
				sand.y++
			} else if array[sand.y+1][sand.x-1] == "." {
				sand.y++
				sand.x--
			} else if array[sand.y+1][sand.x+1] == "." {
				sand.y++
				sand.x++
			}
			if sand == spring {
				sandCoveringSpring = true
				break
			}

			if sand == prevSand {
				sandMoving = false
			} else {
				if array[prevSand.y][prevSand.x] != "+" { // don't overwrite the spring
					array[prevSand.y][prevSand.x] = "."
				}
				array[sand.y][sand.x] = "o"
				prevSand = sand
			}
		}
	}
	visualizeArray()
}
