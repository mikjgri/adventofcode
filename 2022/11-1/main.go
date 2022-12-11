package main

import (
	"fmt"
	"io/ioutil"
	"sort"
	"strconv"
	"strings"
)

func remove[T any](slice []T, s int) []T {
	return append(slice[:s], slice[s+1:]...)
}

type Monkey struct {
	Items           []int
	OperationType   string
	Operation       string
	TestDivisibleBy int
	TrueTestMonkey  int
	FalseTestMonkey int
}

func getMonkeys(lines []string) []Monkey {
	monkeys := []Monkey{}
	var crtMonkey *Monkey
	for _, line := range lines {
		line = strings.TrimSpace(strings.Replace(line, "\r", "", -1))
		if strings.HasPrefix(line, "Monkey") {
			monkeys = append(monkeys, Monkey{
				Items: []int{},
			})
			crtMonkey = &monkeys[len(monkeys)-1]
		} else if strings.HasPrefix(line, "Starting items") {
			split := strings.Split(line, " ")
			for _, item := range split[2:] {
				item = strings.Replace(item, ",", "", -1)
				itemInt, _ := strconv.Atoi(item)
				crtMonkey.Items = append(crtMonkey.Items, itemInt)
			}
		} else if strings.HasPrefix(line, "Operation") {
			split := strings.Split(line, " ")
			crtMonkey.OperationType = split[4]
			crtMonkey.Operation = split[5]
		} else if strings.HasPrefix(line, "Test") {
			split := strings.Split(line, " ")
			crtMonkey.TestDivisibleBy, _ = strconv.Atoi(split[3])
		} else if strings.HasPrefix(line, "If true") {
			split := strings.Split(line, " ")
			crtMonkey.TrueTestMonkey, _ = strconv.Atoi(split[5])
		} else if strings.HasPrefix(line, "If false") {
			split := strings.Split(line, " ")
			crtMonkey.FalseTestMonkey, _ = strconv.Atoi(split[5])
		}
	}
	return monkeys
}
func main() {
	content, _ := ioutil.ReadFile("input.txt")
	lines := strings.Split(string(content), "\n")

	monkeys := getMonkeys(lines)
	itemsInspected := []int{}
	for range monkeys {
		itemsInspected = append(itemsInspected, 0)
	}
	for i := 0; i < 20; i++ {
		for monkeyIndex := range monkeys {
			monkey := &monkeys[monkeyIndex]
			for _, item := range monkey.Items {
				itemsInspected[monkeyIndex]++
				operationVal := item //old
				if val, err := strconv.Atoi(monkey.Operation); err == nil {
					operationVal = val
				}
				if monkey.OperationType == "*" {
					item = item * operationVal
				} else { // +
					item = item + operationVal
				}
				item = item / 3 //rounds
				if item%monkey.TestDivisibleBy == 0 {
					monkeys[monkey.TrueTestMonkey].Items = append(monkeys[monkey.TrueTestMonkey].Items, item)
				} else {
					monkeys[monkey.FalseTestMonkey].Items = append(monkeys[monkey.FalseTestMonkey].Items, item)
				}
			}
			monkey.Items = []int{}
		}
	}
	sort.Sort(sort.Reverse(sort.IntSlice(itemsInspected)))
	fmt.Println(itemsInspected[0] * itemsInspected[1])
}
