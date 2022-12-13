package main

import (
	"fmt"
	"io/ioutil"
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

type Pair struct {
	left  []string
	right []string
}

func buildPairs(lines []string) []Pair {
	pairs := []Pair{}

	getList := func(line string) []string {
		substr := line[1 : len(line)-1]
		return strings.Split(substr, ",")
	}

	for i, line := range lines {
		fmt.Println(line)
		if (i+1)%3 == 0 {
			pairs = append(pairs, Pair{
				left:  getList(lines[i-2]),
				right: getList(lines[i-1]),
			})
		}
	}
	return pairs
}

func isInt(inp string) bool {
	return !isList(inp)
}
func isList(inp string) bool {
	return strings.Contains(inp, "[")
}
func getInt(inp string) int {
	val, _ := strconv.Atoi(inp)
	return val
}
func getIntList(inp string) []int {
	split := strings.Split(inp, ",")
	vals := []int{}
	for _, val := range split {
		vals = append(vals, getInt(val))
	}
	return vals
}
func determineSmallest(left string, right string) int {

}
func main() {
	lines := readLinesAndTrim("example.txt")
	pairs := buildPairs(lines)

	for _, pair := range pairs {
		maxLen := len(pair.left)
		if len(pair.right) > maxLen {
			maxLen = len(pair.right)
		}
		for i := 0; i < maxLen; i++ {
			fv, sv := pair.left[i], pair.right[i]
		}
	}

	fmt.Println(pairs)
}
