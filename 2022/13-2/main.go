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

func makeJsonAndParse(input string) []interface{} {
	var temp map[string][]interface{}
	wrappedInput := fmt.Sprintf("{\"temp\":[%s]}", input)
	json.Unmarshal([]byte(wrappedInput), &temp)
	return temp["temp"]
}
func getPackets(lines []string) []interface{} {
	var packets []interface{}

	for _, line := range lines {
		if line != "" {
			packets = append(packets, makeJsonAndParse(line))
		}
	}
	return packets
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
	dividerPackets := append(make([]interface{}, 0), makeJsonAndParse("[[2]]"), makeJsonAndParse("[[6]]"))

	lines := readLinesAndTrim("input.txt")
	packets := getPackets(lines)
	packets = append(packets, dividerPackets...)

	didSwap := true
	for didSwap == true {
		didSwap = false
		for i := 0; i < len(packets)-1; i++ {
			if compareSides(packets[i], packets[i+1]) == 1 {
				packets[i], packets[i+1] = packets[i+1], packets[i]
				didSwap = true
			}
		}
	}
	getDividerIndex := func(dividerPackage interface{}) int {
		for i, packet := range packets {
			if fmt.Sprintf("%v", packet) == fmt.Sprintf("%v", dividerPackage) {
				return i + 1
			}
		}
		return -1
	}
	fmt.Println(getDividerIndex(dividerPackets[0]) * getDividerIndex(dividerPackets[1]))
}
