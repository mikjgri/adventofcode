package main

import (
	"fmt"
	"io/ioutil"
	"math"
	"strconv"
	"strings"
)

type Coordinates struct {
	x int
	y int
}

func containsCoordinates(list []Coordinates, coords Coordinates) bool {
	for _, item := range list {
		if item.x == coords.x && item.y == coords.y {
			return true
		}
	}
	return false
}
func main() {
	content, _ := ioutil.ReadFile("input.txt")
	lines := strings.Split(string(content), "\n")

	knots := []Coordinates{}
	for i := 0; i < 10; i++ {
		knots = append(knots, Coordinates{x: 1, y: 1})
	}
	tailVisitedCoordinates := []Coordinates{{x: knots[0].x, y: knots[0].y}}

	for _, line := range lines {
		line = strings.TrimSpace(strings.Replace(line, "\r", "", -1))
		split := strings.Split(line, " ")
		direction := split[0]
		moveCount, _ := strconv.Atoi(split[1])
		for i := 0; i < moveCount; i++ {
			switch direction {
			case "R":
				knots[0].x++
			case "L":
				knots[0].x--
			case "U":
				knots[0].y++
			case "D":
				knots[0].y--
			}
			for i, knot := range knots {
				if i == 0 {
					continue
				}
				previousKnot := knots[i-1]

				xDiff := previousKnot.x - knot.x
				yDiff := previousKnot.y - knot.y
				if math.Abs(float64(xDiff)) <= 1 && math.Abs(float64(yDiff)) <= 1 {
					continue //tail close enough
				}

				if previousKnot.x != knot.x && previousKnot.y != knot.y {
					if xDiff > 0 {
						knot.x++
					} else {
						knot.x--
					}
					if yDiff > 0 {
						knot.y++
					} else {
						knot.y--
					}
				} else if previousKnot.y == knot.y {
					if xDiff > 1 {
						knot.x++
					} else {
						knot.x--
					}
				} else if previousKnot.x == knot.x {
					if yDiff > 1 {
						knot.y++
					} else {
						knot.y--
					}
				}
				knots[i] = knot
			}
			tail := knots[len(knots)-1]
			if !containsCoordinates(tailVisitedCoordinates, tail) {
				tailVisitedCoordinates = append(tailVisitedCoordinates, Coordinates{
					x: tail.x,
					y: tail.y,
				})
			}
		}
	}

	totalVisited := len(tailVisitedCoordinates)

	fmt.Println(totalVisited)

	//visualization for fun
	xMax, xMin, yMax, yMin := 0, 0, 0, 0
	for _, coord := range tailVisitedCoordinates {
		if coord.x > xMax {
			xMax = coord.x
		}
		if coord.x < xMin {
			xMin = coord.x
		}
		if coord.y > yMax {
			yMax = coord.y
		}
		if coord.y < yMin {
			yMin = coord.y
		}
	}
	for y := yMax; y >= yMin; y-- {
		for x := xMin; x <= xMax; x++ {
			if containsCoordinates(tailVisitedCoordinates, Coordinates{x: x, y: y}) {
				if tailVisitedCoordinates[0].x == x && tailVisitedCoordinates[0].y == y {
					fmt.Print("S")
				} else {
					fmt.Print("#")
				}
			} else {
				fmt.Print(".")
			}
		}
		fmt.Println()
	}
}
