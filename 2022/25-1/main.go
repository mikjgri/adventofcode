package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

func reverse(s string) string {
	rns := []rune(s)
	for i, j := 0, len(rns)-1; i < j; i, j = i+1, j-1 {
		rns[i], rns[j] = rns[j], rns[i]
	}
	return string(rns)
}
func readLinesAndTrim(file string) []string {
	content, _ := os.ReadFile(file)
	rawLines := strings.Split(string(content), "\n")
	lines := []string{}
	for _, line := range rawLines {
		lines = append(lines, strings.TrimSpace(strings.Replace(line, "\r", "", -1)))
	}
	return lines
}
func toDecimal(snafu string) int {
	decimal := 0
	snafu = reverse(snafu)
	factor := 1
	for _, char := range snafu {
		val := string(char)
		if val == "-" {
			val = "-1"
		} else if val == "=" {
			val = "-2"
		}
		decVal, _ := strconv.Atoi(val)
		decimal += decVal * factor
		factor *= 5
	}
	return decimal
}
func toSnafu(decimal int) string {
	if decimal == 0 {
		return ""
	}
	return toSnafu((decimal+2)/5) + []string{"0", "1", "2", "=", "-"}[decimal%5]
}

func main() {
	lines := readLinesAndTrim("input.txt")
	sumDecimal := 0
	for _, line := range lines {
		sumDecimal += toDecimal(line)
	}
	fmt.Println(toSnafu(sumDecimal))
}
