package main

import (
	"fmt"
	"io/ioutil"
	"regexp"
	"strconv"
	"strings"
)

func reverseArray(arr []string) []string {
	for i, j := 0, len(arr)-1; i < j; i, j = i+1, j-1 {
		arr[i], arr[j] = arr[j], arr[i]
	}
	return arr
}

type StackString struct {
	s []string
}

func (s *StackString) Push(value string) {
	s.s = append(s.s, value)
}

func (s *StackString) Pop() string {
	length := len(s.s)
	res := s.s[length-1]
	s.s = s.s[:length-1]
	return res
}
func (s *StackString) Top() string {
	length := len(s.s)
	res := s.s[length-1]
	return res
}
func (s *StackString) IsEmpty() bool {
	length := len(s.s)
	return length == 0
}

func main() {
	content, _ := ioutil.ReadFile("input.txt")
	lines := strings.Split(string(content), "\n")

	stacks := map[int]StackString{}

	stackLines := []string{}
	orderLines := []string{}
	for _, line := range lines {
		line = strings.Replace(line, "\r", "", -1)
		if strings.Contains(line, "[") {
			stackLines = append(stackLines, line)
		}
		if strings.Contains(line, "move") {
			orderLines = append(orderLines, line)
		}
	}
	for _, line := range reverseArray(stackLines) {
		i := 1
		for len(line) >= 3 {
			if _, ok := stacks[i]; !ok {
				stacks[i] = StackString{}
			}
			stack := stacks[i]

			sliceRange := 4
			if len(line) < 4 {
				sliceRange = 3
			}
			sLine := line[:sliceRange]
			line = line[sliceRange:]

			val := string(sLine[1])
			trimmed := strings.TrimSpace(val)
			if len(trimmed) > 0 {
				stack.Push(trimmed)
				stacks[i] = stack
			}
			i++
		}
	}
	for _, line := range orderLines {
		r, _ := regexp.Compile("[0-9]+")
		orders := r.FindAllString(line, -1)
		from, _ := strconv.Atoi(orders[1])
		to, _ := strconv.Atoi(orders[2])
		iterations, _ := strconv.Atoi(orders[0])
		for i := 0; i < iterations; i++ {
			sourceStack := stacks[from]
			crate := sourceStack.Pop()
			stacks[from] = sourceStack
			targetStack := stacks[to]
			targetStack.Push(crate)
			stacks[to] = targetStack
		}
	}
	for i := 1; i <= len(stacks); i++ {
		stack := stacks[i]
		if stack.IsEmpty() {
			fmt.Print("x")
		} else {
			fmt.Print(stack.Top())
		}
	}
}
