package main

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
)

func main() {
	content, _ := ioutil.ReadFile("input.txt")
	var lines = strings.Split(string(content), "\n")

	for i := 99999378149762; i > 9999999999999; i-- {
		inpIndex := 0
		inpList := strings.Split(fmt.Sprint(i), "")
		if strings.Count(fmt.Sprint(i), "0") > 0 {
			continue
		}
		invalidAlu := false
		variables := map[string]int{}
		for _, line := range lines {
			line = strings.Replace(line, "\r", "", -1)
			split := strings.Split(line, " ")
			instr := split[0]
			if instr == "inp" {
				variables[split[1]], _ = strconv.Atoi(inpList[inpIndex])
				inpIndex++
				continue
			}

			c1, c2 := split[1], split[2]
			v1 := variables[c1]
			var v2 int
			if pv, err := strconv.Atoi(c2); err == nil {
				v2 = pv
			} else {
				v2 = variables[c2]
			}
			switch instr {
			case "add":
				variables[c1] = v1 + v2
			case "mul":
				variables[c1] = v1 * v2
			case "div":
				if v2 == 0 {
					invalidAlu = true
					break
				}
				variables[c1] = v1 / v2
			case "mod":
				if v1 < 0 || v2 <= 0 {
					invalidAlu = true
					break
				}
				variables[c1] = v1 % v2
			case "eql":
				if v1 == v2 {
					variables[c1] = 1
				} else {
					variables[c1] = 0
				}
			}
		}
		fmt.Print(i)
		if invalidAlu {
			fmt.Print(" - invalid")
		} else if variables["z"] != 0 {
			fmt.Printf(" - %d", variables["z"])
		} else {
			fmt.Print(" - success!")
			break
		}
		fmt.Println()
	}
}
