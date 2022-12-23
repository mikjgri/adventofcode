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
			line = append(line, "")
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

	for i := range instructions {
		char := rune(instructions[i])
		if unicode.IsLetter(char) {
			result = append(result, string(char))
			i++
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
					}
				} else {
					nextIsLetter = true
				}
				if !nextIsLetter {
					i++
				}
			}
			numberInt, _ := strconv.Atoi(numberString)
			result = append(result, numberInt)
		}
	}
	return result
}
func main() {
	lines := readLinesAndTrim("example.txt")
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

	//walk the walk
	position := getStartPosition(array)
	facing := 90
	for _, instruction := range instructions {
		if isNumber(instruction) {
			steps := instruction.(int)
			for i := 0; i < steps; i++ {
				switch facing {
				case 90:
					position[1]++
				case 270:
					position[1]--
				case 0:
					position[0]--
				case 180:
					position[0]++
				}
				array[position[0]][position[1]] = "x"
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
	//talk the talk
	for _, line := range array {
		for _, char := range line {
			fmt.Print(char)
		}
		fmt.Println()
	}
	fmt.Println(instructions)
}
