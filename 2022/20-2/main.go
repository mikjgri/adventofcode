package main

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"

	"golang.org/x/exp/slices"
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
func getListOfNumbersWithIds(lines []string) []NumberWithId {
	numbers := []NumberWithId{}
	id := 0
	for _, line := range lines {
		number, _ := strconv.Atoi(line)
		numbers = append(numbers, NumberWithId{id, number * 811589153})
		id++
	}
	return numbers
}

type NumberWithId struct {
	id     int
	number int
}

func getNumberWithId(id int, numbers []NumberWithId) NumberWithId {
	for _, number := range numbers {
		if number.id == id {
			return number
		}
	}
	panic("Number not found")
}
func insertAtIndex(index int, number NumberWithId, numbers []NumberWithId) []NumberWithId {
	front, back := numbers[:index], numbers[index:]
	var ret []NumberWithId
	ret = append(ret, front...)
	ret = append(ret, number)
	ret = append(ret, back...)
	return ret
}
func modNeg(a, b int) int {
	return (a%b + b) % b
}
func getNumberAtIndex(index int, numbers []NumberWithId) NumberWithId {
	index = modNeg(index, len(numbers))
	return numbers[index]
}

func main() {
	lines := readLinesAndTrim("input.txt")
	numbers := getListOfNumbersWithIds(lines)

	for round := 0; round < 10; round++ {
		for i := 0; i < len(numbers); i++ {
			number := getNumberWithId(i, numbers)
			indexOfNumber := slices.IndexFunc(numbers, func(c NumberWithId) bool { return c.id == number.id })
			newIndex := modNeg(indexOfNumber+number.number, len(numbers)-1)
			if newIndex == 0 {
				newIndex = len(numbers) - 1
			}
			front, back := numbers[:indexOfNumber], numbers[indexOfNumber+1:]

			//removed number in question
			numbers = append(front, back...)
			numbers = insertAtIndex(newIndex, number, numbers)

			// for _, number := range numbers {
			// 	fmt.Printf("%d,", number.number)
			// }
			// fmt.Println("\n-----")
		}
	}
	indexOf0 := slices.IndexFunc(numbers, func(c NumberWithId) bool { return c.number == 0 })

	v1, v2, v3 := getNumberAtIndex(indexOf0+1000, numbers), getNumberAtIndex(indexOf0+2000, numbers), getNumberAtIndex(indexOf0+3000, numbers)
	fmt.Println(v1.number + v2.number + v3.number)
}
