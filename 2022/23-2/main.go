package main

import (
	"fmt"
	"io/ioutil"
	"math"
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

type Elf struct {
	id                   int
	x, y                 int
	proposedX, proposedY *int
}
type Instruction struct {
	EmptyPositions []string
	ProposeMove    string
}

func getElves(lines []string) *[]Elf {
	var result []Elf
	id := 0
	for y, line := range lines {
		for x, row := range line {
			if string(row) == "#" {
				result = append(result, Elf{id: id, x: x, y: y})
				id++
			}
		}
	}
	return &result
}
func getMinMax(elves []Elf) (int, int, int, int) {
	minX, maxX, minY, maxY := 0, 0, 0, 0
	for _, elf := range elves {
		if elf.x < minX {
			minX = elf.x
		}
		if elf.x > maxX {
			maxX = elf.x
		}
		if elf.y < minY {
			minY = elf.y
		}
		if elf.y > maxY {
			maxY = elf.y
		}
	}
	return minX, maxX, minY, maxY
}
func getInstructions() []Instruction {
	return []Instruction{
		{EmptyPositions: []string{"N", "NE", "NW"}, ProposeMove: "N"},
		{EmptyPositions: []string{"S", "SE", "SW"}, ProposeMove: "S"},
		{EmptyPositions: []string{"W", "NW", "SW"}, ProposeMove: "W"},
		{EmptyPositions: []string{"E", "NE", "SE"}, ProposeMove: "E"},
	}
}
func getRotatedInstructions(instructions []Instruction) []Instruction {
	rotatedInstructions := instructions[1:]
	rotatedInstructions = append(rotatedInstructions, instructions[0])
	return rotatedInstructions
}
func getRelativeCoordinates(elf *Elf, direction string) (*int, *int) {
	var newX, newY *int
	newX, newY = new(int), new(int)
	*newX, *newY = elf.x, elf.y
	switch direction {
	case "N":
		*newY = elf.y - 1
	case "S":
		*newY = elf.y + 1
	case "W":
		*newX = elf.x - 1
	case "E":
		*newX = elf.x + 1
	case "NE":
		*newX = elf.x + 1
		*newY = elf.y - 1
	case "NW":
		*newX = elf.x - 1
		*newY = elf.y - 1
	case "SE":
		*newX = elf.x + 1
		*newY = elf.y + 1
	case "SW":
		*newX = elf.x - 1
		*newY = elf.y + 1
	}
	return newX, newY
}
func hasAdjecentElves(elf *Elf, elves []Elf) bool {
	for _, elf2 := range elves {
		if elf2.id == elf.id {
			continue
		}
		xDiff, yDiff := elf2.x-elf.x, elf2.y-elf.y
		if math.Abs(float64(xDiff)) <= 1 && math.Abs(float64(yDiff)) <= 1 {
			return true
		}
	}
	return false
}

func main() {
	lines := readLinesAndTrim("input.txt")
	elves := *getElves(lines)
	instructions := getInstructions()

	elvesMoved := true
	round := 0
	for elvesMoved {
		fmt.Printf("Round %d\n", round)
		round++
		//propose move
		for elfIndex := range elves {
			elf := &elves[elfIndex]
			if !hasAdjecentElves(elf, elves) {
				continue
			}
			for _, instruction := range instructions {
				empty := true
				for _, positionToCheck := range instruction.EmptyPositions {
					x, y := getRelativeCoordinates(elf, positionToCheck)
					for _, elf2 := range elves {
						if elf2.x == *x && elf2.y == *y {
							empty = false
							break
						}
					}
				}
				if empty {
					elf.proposedX, elf.proposedY = getRelativeCoordinates(elf, instruction.ProposeMove)
					break
				}
			}
		}
		//remove invalid moves
		for elfIndex := range elves {
			elf := &elves[elfIndex]
			if elf.proposedX == nil && elf.proposedY == nil {
				continue
			}
			foundDuplicate := false
			for elfIndex := range elves {
				elf2 := &elves[elfIndex]
				if elf == elf2 { //skip self
					continue
				}
				if elf2.proposedX == nil && elf2.proposedY == nil {
					continue
				}
				if *elf.proposedX == *elf2.proposedX && *elf.proposedY == *elf2.proposedY {
					foundDuplicate = true
					elf2.proposedX = nil
					elf2.proposedY = nil
				}
			}
			if foundDuplicate {
				elf.proposedX = nil
				elf.proposedY = nil
			}
		}

		//check if elves moved
		elvesMoved = false
		for _, elf := range elves {
			if elf.proposedX != nil && elf.proposedY != nil {
				elvesMoved = true
				break
			}
		}

		//move
		for elfIndex := range elves {
			elf := &elves[elfIndex]
			if elf.proposedX != nil && elf.proposedY != nil {
				elf.x = *elf.proposedX
				elf.y = *elf.proposedY
				elf.proposedX = nil
				elf.proposedY = nil
			}
		}
		instructions = getRotatedInstructions(instructions)
	}
	fmt.Printf("Elves didnt move at round: %d", round)
}
