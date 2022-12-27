package main

import (
	"fmt"
	"io/ioutil"
	"math"
	"regexp"
	"strconv"
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

type Coordinates struct {
	x, y int
}
type Sensor struct {
	coord             Coordinates
	manhattanDistance int
}

func buildSensors(lines []string) []Sensor {
	var result []Sensor
	for _, line := range lines {
		split := strings.Split(line, ":")
		r, _ := regexp.Compile("[-\\d]+")
		ss := r.FindAllString(split[0], 2)
		bs := r.FindAllString(split[1], 2)
		sensor := Coordinates{}
		sensor.x, _ = strconv.Atoi(ss[0])
		sensor.y, _ = strconv.Atoi(ss[1])
		beacon := Coordinates{}
		beacon.x, _ = strconv.Atoi(bs[0])
		beacon.y, _ = strconv.Atoi(bs[1])

		result = append(result, Sensor{coord: sensor, manhattanDistance: getManhattenDistance(sensor, beacon)})
	}
	return result
}
func getManhattenDistance(c1, c2 Coordinates) int {
	bsxDiff, bsyDiff := getAbs(c1.x-c2.x), getAbs(c1.y-c2.y)
	return bsxDiff + bsyDiff
}
func getAbs(inp int) int {
	return int(math.Abs(float64(inp)))
}
func isReachable(sensors []Sensor, c Coordinates) bool {
	for _, sensor := range sensors {
		manhattenDistance := getManhattenDistance(c, sensor.coord)
		if sensor.manhattanDistance >= manhattenDistance {
			return true
		}
	}
	return false
}
func main() {
	lines := readLinesAndTrim("input.txt")
	sensors := buildSensors(lines)

	max := 4000000
	for _, sensor := range sensors {
		distance := sensor.manhattanDistance + 1

		for i := 0; i < distance; i++ {
			targetRow := sensor.coord.y + i
			if targetRow < 0 {
				continue
			}
			if targetRow > max {
				break
			}
			colOffset := distance - getAbs(i)

			coordsToCheck := []Coordinates{}
			coordsToCheck = append(coordsToCheck, Coordinates{x: sensor.coord.x - colOffset, y: targetRow}) //left
			coordsToCheck = append(coordsToCheck, Coordinates{x: sensor.coord.x + colOffset, y: targetRow}) //right

			for _, coord := range coordsToCheck {
				if coord.x >= 0 && coord.x <= max {
					if !isReachable(sensors, coord) {
						fmt.Printf("%d", coord.x*4000000+coord.y)
						return
					}
				}
			}
		}
	}
}
