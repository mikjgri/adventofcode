package main

import (
	"fmt"
	"io/ioutil"
	"strings"
)

func containsDuplicates(input string) bool {
	for _, char := range input {
		if strings.Count(input, string(char)) > 1 {
			return true
		}
	}
	return false
}

func main() {
	content, _ := ioutil.ReadFile("input.txt")

	for i, _ := range content {
		if i < 4 {
			continue
		}
		mark := string(content[i-4 : i])
		if !containsDuplicates(mark) {
			fmt.Println(i)
			break
		}
	}
}
