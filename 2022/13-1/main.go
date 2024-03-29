package main

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"reflect"
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
	left  []interface{}
	right []interface{}
}

func makeJsonAndParse(input string) []interface{} {
	var temp map[string][]interface{}
	wrappedInput := fmt.Sprintf("{\"temp\":[%s]}", input)
	json.Unmarshal([]byte(wrappedInput), &temp)
	return temp["temp"]
}
func buildPairs(lines []string) []Pair {
	pairs := []Pair{}
	for i := range lines {
		if (i+1)%3 == 0 {
			pairs = append(pairs, Pair{
				left:  makeJsonAndParse(lines[i-2]),
				right: makeJsonAndParse(lines[i-1]),
			})
		}
	}
	return pairs
}
func isNumber(input interface{}) bool {
	return reflect.TypeOf(input).String() == "float64"
}
func isList(input interface{}) bool {
	return reflect.TypeOf(input).String() == "[]interface {}"
}
func compareSides(left interface{}, right interface{}) int {
	if isNumber(left) && isNumber(right) {
		ln, rn := left.(float64), right.(float64)
		if ln < rn {
			return -1
		}
		if ln == rn {
			return 0
		}
		return 1
	}
	if isList(left) && isList(right) {
		ll, rl := left.([]interface{}), right.([]interface{})
		i := 0
		for ; i < len(ll) && i < len(rl); i++ {
			compResult := compareSides(ll[i], rl[i])
			if compResult != 0 {
				return compResult
			}
		}
		if len(ll) == i && len(rl) == i {
			return 0
		}
		if len(ll) == i && len(rl) > i {
			return -1
		}
		if len(ll) > i && len(rl) == i {
			return 1
		}
		return 0
	}
	if isList(left) {
		return compareSides(left, []interface{}{right})
	}
	return compareSides([]interface{}{left}, right)
}

func main() {
	lines := readLinesAndTrim("input.txt")
	pairs := buildPairs(lines)

	indicesInOrderSum := 0
	for pairIndex, pair := range pairs {
		if compareSides(pair.left, pair.right) == -1 {
			indicesInOrderSum += pairIndex + 1
		}
	}

	fmt.Println(indicesInOrderSum)
}
