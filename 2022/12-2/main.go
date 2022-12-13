package main

import (
	"fmt"
	"io/ioutil"
	"strings"
)

type Coordinates struct {
	x int
	y int
}
type MapInfo struct {
	Map               [][]string
	End               Coordinates
	StartingPositions []Coordinates
}

func buildMap(lines []string) *MapInfo {
	mapInfo := new(MapInfo)
	mapInfo.Map = [][]string{}
	for y, line := range lines {
		mapInfo.Map = append(mapInfo.Map, []string{})
		line = strings.TrimSpace(strings.Replace(line, "\r", "", -1))
		split := strings.Split(line, "")
		for x, letter := range split {
			if letter == "S" || letter == "a" {
				letter = "a"
				mapInfo.StartingPositions = append(mapInfo.StartingPositions, Coordinates{x: x, y: y})
			} else if letter == "E" {
				mapInfo.End = Coordinates{x: x, y: y}
				letter = "z"
			}
			mapInfo.Map[y] = append(mapInfo.Map[y], letter)
		}
	}
	return mapInfo
}

func getLetterPosition(letter string) int {
	r := []rune(letter)[0]
	return int(r - 'A' + 1)
}
func main() {
	content, _ := ioutil.ReadFile("input.txt")
	lines := strings.Split(string(content), "\n")

	mapInfo := buildMap(lines)

	isValidMove := func(current Coordinates, next Coordinates) bool {
		if next.x < 0 || next.y < 0 || next.x > len(mapInfo.Map[0])-1 || next.y > len(mapInfo.Map)-1 {
			return false
		}
		currentLetter, nextLetter := mapInfo.Map[current.y][current.x], mapInfo.Map[next.y][next.x]

		ci, ni := getLetterPosition(currentLetter), getLetterPosition(nextLetter)
		if ci > ni { //currentlyHigherUp
			return true
		}
		return (ni - ci) <= 1
	}

	shortestPath := 999
	for _, start := range mapInfo.StartingPositions {
		solved := false
		currentSet := []Coordinates{start}
		visited := []Coordinates{}
		moves := 0
		for solved == false && len(currentSet) > 0 {
			nextSet := []Coordinates{}
			for _, set := range currentSet {
				hasVisited := func() bool {
					for _, visit := range visited {
						if visit.x == set.x && visit.y == set.y {
							return true
						}
					}
					return false
				}

				if hasVisited() {
					continue
				}
				visited = append(visited, set)

				if mapInfo.End.x == set.x && mapInfo.End.y == set.y {
					solved = true
					continue
				}

				rightC := Coordinates{x: set.x + 1, y: set.y}
				if isValidMove(set, rightC) {
					nextSet = append(nextSet, rightC)
				}
				leftC := Coordinates{x: set.x - 1, y: set.y}
				if isValidMove(set, leftC) {
					nextSet = append(nextSet, leftC)
				}
				upC := Coordinates{x: set.x, y: set.y + 1}
				if isValidMove(set, upC) {
					nextSet = append(nextSet, upC)
				}
				downC := Coordinates{x: set.x, y: set.y - 1}
				if isValidMove(set, downC) {
					nextSet = append(nextSet, downC)
				}
			}
			if !solved {
				moves++
			}
			currentSet = nextSet
		}
		if solved && moves < shortestPath {
			shortestPath = moves
		}
	}

	fmt.Println(shortestPath)
}
