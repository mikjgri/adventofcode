package main

import (
	"fmt"
	"io/ioutil"
	"math"
	"reflect"
	"sort"
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

type Blizzard struct {
	x, y      int
	direction string
}

func buildMap(lines []string) ([][]string, []Blizzard) {
	var resultMap [][]string
	var blizzards []Blizzard
	for row, line := range lines {
		rowR := []string{}
		for column, char := range line {
			if string(char) == ">" || string(char) == "<" || string(char) == "^" || string(char) == "v" {
				blizzards = append(blizzards, Blizzard{x: column, y: row, direction: string(char)})
				char = '.'
			}
			rowR = append(rowR, string(char))
		}
		resultMap = append(resultMap, rowR)
	}
	return resultMap, blizzards
}
func getStartPosition(array [][]string) []int {
	for column, char := range array[0] {
		if char == "." {
			return []int{0, column}
		}
	}
	panic("No start position found!")
}
func getEndPosition(array [][]string) []int {
	for column, char := range array[len(array)-1] {
		if char == "." {
			return []int{len(array) - 1, column}
		}
	}
	panic("No end position found!")
}
func getVisitedArray(rows, columns int) [][]int {
	var result [][]int
	for row := 1; row <= rows; row++ {
		line := []int{}
		for column := 1; column <= columns; column++ {
			line = append(line, 0)
		}
		result = append(result, line)
	}
	return result
}

func nextBlizzardSequence(blizzards []Blizzard, maxRow, maxColumn int) []Blizzard {
	result := make([]Blizzard, len(blizzards))
	copy(result, blizzards)
	for i, blizzard := range result {
		switch blizzard.direction {
		case ">":
			result[i].x++
			if result[i].x == maxColumn-1 {
				result[i].x = 1
			}
		case "v":
			result[i].y++
			if result[i].y == maxRow-1 {
				result[i].y = 1
			}
		case "<":
			result[i].x--
			if result[i].x == 0 {
				result[i].x = maxColumn - 2
			}
		case "^":
			result[i].y--
			if result[i].y == 0 {
				result[i].y = maxRow - 2
			}
		}
	}
	return result
}
func getManhatten(position, end []int) int {
	xDiff, yDiff := position[1]-end[1], position[0]-end[0]
	return int(math.Abs(float64(xDiff)) + math.Abs(float64(yDiff)))
}
func sortByMannhattanDistance(positions [][]int, current []int, end []int) [][]int {
	sort.Slice(positions, func(i, j int) bool {
		p1, p2 := getManhatten(positions[i], end), getManhatten(positions[j], end)
		return p1 < p2
	})
	return positions
}
func getAllBlizzardStates(initialBlizzards []Blizzard, maxRow int, maxCol int) [][]Blizzard {
	blizzardStates := [][]Blizzard{initialBlizzards}
	stateNotSeen := true
	for stateNotSeen {
		nextBlizzards := nextBlizzardSequence(blizzardStates[len(blizzardStates)-1], maxRow, maxCol)
		for _, blizzardState := range blizzardStates {
			if reflect.DeepEqual(blizzardState, nextBlizzards) {
				stateNotSeen = false
				break
			}
		}
		blizzardStates = append(blizzardStates, nextBlizzards)
	}
	return blizzardStates
}
func main() {
	lines := readLinesAndTrim("input.txt")
	array, blizzards := buildMap(lines)
	blizzardStates := getAllBlizzardStates(blizzards, len(array), len(array[0]))

	moveFromStartToEnd := func(start, end []int, minutes int) int {
		visitedArrays := [][][]int{}
		for i := 0; i < len(blizzardStates); i++ {
			visitedArrays = append(visitedArrays, getVisitedArray(len(array), len(array[0])))
		}
		quickestSolution := math.MaxInt32
		var tryDoMove func(position []int, minutes int) (bool, int)
		tryDoMove = func(position []int, minutes int) (bool, int) {
			if minutes > quickestSolution {
				return false, minutes
			}
			if position[0] < 0 || position[1] < 0 || position[0] > len(array)-1 || position[1] > len(array[0])-1 { //out of bounds
				return false, minutes
			}
			xDiff, yDiff := position[1]-end[1], position[0]-end[0]
			manhattanDistance := int(math.Abs(float64(xDiff)) + math.Abs(float64(yDiff)))
			if manhattanDistance > quickestSolution-minutes { //too far away
				return false, minutes
			}

			if position[0] == end[0] && position[1] == end[1] { //reached goal
				if minutes < quickestSolution {
					quickestSolution = minutes
					// fmt.Printf("Found quicker solution: %d\n", quickestSolution)
				}
				return true, minutes
			}
			if array[position[0]][position[1]] == "#" { //hit wall
				return false, minutes
			}
			state := minutes % len(blizzardStates) // hits blizzard
			for _, blizzard := range blizzardStates[state] {
				if blizzard.x == position[1] && blizzard.y == position[0] {
					return false, minutes
				}
			}
			if visitedArrays[state][position[0]][position[1]] != 0 && visitedArrays[state][position[0]][position[1]] <= minutes { //already visited with quicker path
				return false, minutes
			}
			visitedArrays[state][position[0]][position[1]] = minutes
			possibleMoves := [][]int{
				{position[0] - 1, position[1]}, //up
				{position[0], position[1] - 1}, //left
				{position[0] + 1, position[1]}, //down
				{position[0], position[1] + 1}, //right
				{position[0], position[1]},     //stand still
			}
			possibleMoves = sortByMannhattanDistance(possibleMoves, position, end)
			shortest := 0
			for _, move := range possibleMoves {
				success, minutesSpent := tryDoMove(move, minutes+1)
				if success {
					if shortest == 0 || minutesSpent < shortest {
						shortest = minutesSpent
					}
				}
			}
			if shortest != 0 {
				return true, shortest
			}
			return false, minutes
		}
		success, minutes := tryDoMove(start, minutes)
		if success {
			fmt.Println(minutes)
		} else {
			panic("No solution found!")
		}
		return minutes
	}

	start, end := getStartPosition(array), getEndPosition(array)
	fmt.Println("Moving from start to end...")
	totalTime := moveFromStartToEnd(start, end, 0)
	fmt.Printf("Minutes after first move %d\n", totalTime)
	fmt.Println("Moving from end to start...")
	totalTime = moveFromStartToEnd(end, start, totalTime)
	fmt.Printf("Minutes after second move %d \n", totalTime)
	fmt.Println("Moving from start to end...")
	totalTime = moveFromStartToEnd(start, end, totalTime)
	fmt.Printf("Minutes after third move %d \n", totalTime)
}
