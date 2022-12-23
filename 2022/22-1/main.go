package main

import (
	"fmt"
	"io/ioutil"
	"reflect"
	"strconv"
	"strings"
	"unicode"
)

func readLinesAndTrim(file string) []string {
	content, _ := ioutil.ReadFile(file)
	rawLines := strings.Split(string(content), "\n")
	lines := []string{}
	for _, line := range rawLines {
		lines = append(lines, strings.Replace(line, "\r", "", -1))
	}
	return lines
}

func build2dArray(rows, columns int) [][]string {
	var result [][]string
	for row := 1; row <= rows; row++ {
		line := []string{}
		for column := 1; column <= columns; column++ {
			line = append(line, " ")
		}
		result = append(result, line)
	}
	return result
}

func getMaxColumnLength(lines []string) int {
	max := 0
	for _, line := range lines {
		if len(line) > max {
			max = len(line)
		}
	}
	return max
}
func getStartPosition(array [][]string) []int {
	for row, line := range array {
		for column, char := range line {
			if char == "." {
				return []int{row, column}
			}
		}
	}
	panic("No start position found!")
}
func isNumber(input interface{}) bool {
	return reflect.TypeOf(input).String() == "int"
}
func isString(input interface{}) bool {
	return reflect.TypeOf(input).String() == "string"
}
func getInstructions(instructions string) []interface{} {
	result := []interface{}{}

	for i := 0; i < len(instructions); {
		char := rune(instructions[i])
		if unicode.IsLetter(char) {
			result = append(result, string(char))
		} else {
			nextIsLetter := false
			numberString := string(char)
			for !nextIsLetter {
				if i+1 < len(instructions) {
					nextChar := rune(instructions[i+1])
					if unicode.IsLetter(nextChar) {
						nextIsLetter = true
					} else {
						numberString += string(nextChar)
						i++
					}
				} else {
					nextIsLetter = true
				}
			}
			numberInt, _ := strconv.Atoi(numberString)
			result = append(result, numberInt)
		}
		i++
	}
	return result
}
func getFirstValid(line []string, direction int) int {
	if direction == 1 {
		for i, char := range line {
			if char != " " {
				return i
			}
		}
	} else if direction == -1 {
		for i := len(line) - 1; i >= 0; i-- {
			char := line[i]
			if char != " " {
				return i
			}
		}
	}
	panic("No valid X found!")
}
func isInBounds(array [][]string, position []int) bool {
	if position[0] < 0 || position[0] > len(array)-1 || position[1] < 0 || position[1] > len(array[0])-1 || array[position[0]][position[1]] == " " {
		return false
	}
	return true
}
func getColumn(array [][]string, column int) []string {
	var result []string
	for _, line := range array {
		result = append(result, line[column])
	}
	return result
}
func main() {
	lines := readLinesAndTrim("input.txt")
	instructionString := lines[len(lines)-1:][0]
	instructions := getInstructions(instructionString)
	lines = lines[:len(lines)-2]

	array := build2dArray(len(lines), getMaxColumnLength(lines))

	//fill
	for row, line := range lines {
		for column, char := range line {
			array[row][column] = string(char)
		}
	}

	visualize := func() {
		return
		//talk the talk
		fmt.Print("\033[H\033[2J")
		for _, line := range array {
			for _, char := range line {
				fmt.Print(char)
			}
			fmt.Println()
		}
	}

	//walk the walk
	position := getStartPosition(array)
	facing := 90
	for _, instruction := range instructions {
		if isNumber(instruction) {
			steps := instruction.(int)
			for i := 0; i < steps; i++ {
				arrow := ""

				newPosition := []int{position[0], position[1]}
				switch facing {
				case 90:
					arrow = ">"
					newPosition[1]++
					if !isInBounds(array, newPosition) {
						newPosition[1] = getFirstValid(array[newPosition[0]], 1)
					}
				case 270:
					arrow = "<"
					newPosition[1]--
					if !isInBounds(array, newPosition) {
						newPosition[1] = getFirstValid(array[newPosition[0]], -1)
					}
				case 0:
					arrow = "^"
					newPosition[0]--
					if !isInBounds(array, newPosition) {
						newPosition[0] = getFirstValid(getColumn(array, newPosition[1]), -1)
					}
				case 180:
					arrow = "v"
					newPosition[0]++
					if !isInBounds(array, newPosition) {
						newPosition[0] = getFirstValid(getColumn(array, newPosition[1]), 1)
					}
				}
				if array[newPosition[0]][newPosition[1]] == "#" { // hit wall
					break
				} else {
					array[position[0]][position[1]] = arrow
					position = newPosition
					visualize()
				}
			}
		} else {
			direction := instruction.(string)
			switch direction {
			case "R":
				facing += 90
			case "L":
				facing -= 90
			}
			if facing < 0 {
				facing += 360
			} else if facing >= 360 {
				facing -= 360
			}
		}
	}
	if facing == 90 {
		facing = 0
	} else if facing == 180 {
		facing = 1
	} else if facing == 270 {
		facing = 2
	} else {
		facing = 3
	}
	position[0]++
	position[1]++
	score := 1000*position[0] + 4*position[1] + facing
	fmt.Println(score)
}
