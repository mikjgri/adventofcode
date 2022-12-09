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

	tail, head := Coordinates{
		x: 1,
		y: 1,
	}, Coordinates{
		x: 1,
		y: 1,
	}

	tailVisitedCoordinates := []Coordinates{tail}

	for _, line := range lines {
		line = strings.TrimSpace(strings.Replace(line, "\r", "", -1))
		split := strings.Split(line, " ")
		direction := split[0]
		moveCount, _ := strconv.Atoi(split[1])
		for i := 0; i < moveCount; i++ {
			switch direction {
			case "R":
				head.x++
			case "L":
				head.x--
			case "U":
				head.y++
			case "D":
				head.y--
			}
			xDiff := head.x - tail.x
			yDiff := head.y - tail.y
			if math.Abs(float64(xDiff)) <= 1 && math.Abs(float64(yDiff)) <= 1 {
				continue //tail close enough
			}

			if head.x != tail.x && head.y != tail.y {
				if xDiff > 0 {
					tail.x++
				} else {
					tail.x--
				}
				if yDiff > 0 {
					tail.y++
				} else {
					tail.y--
				}
			} else if head.y == tail.y {
				if xDiff > 1 {
					tail.x++
				} else {
					tail.x--
				}
			} else if head.x == tail.x {
				if yDiff > 1 {
					tail.y++
				} else {
					tail.y--
				}
			}
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
}
