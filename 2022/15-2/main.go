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
type SensorBeaconPair struct {
	sensor Coordinates
	beacon Coordinates
}

func buildPairs(lines []string) []SensorBeaconPair {
	var result []SensorBeaconPair
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

		result = append(result, SensorBeaconPair{sensor: sensor, beacon: beacon})
	}
	return result
}
func getAbs(inp int) int {
	return int(math.Abs(float64(inp)))
}
func build2dArray(maxAxis int) [][]bool {
	result := [][]bool{}
	for i := 0; i <= maxAxis; i++ {
		result = append(result, []bool{})
		for j := 0; j <= maxAxis; j++ {
			result[i] = append(result[i], false)
		}
	}
	return result
}
func main() {
	lines := readLinesAndTrim("input.txt")
	// lines := readLinesAndTrim("example.txt")
	pairs := buildPairs(lines)

	maxAxis := 4000000
	// maxAxis := 20
	arr := build2dArray(maxAxis)

	for i, pair := range pairs {
		bsxDiff, bsyDiff := getAbs(pair.beacon.x-pair.sensor.x), getAbs(pair.beacon.y-pair.sensor.y)
		max := bsxDiff + bsyDiff
		if pair.sensor.y-max > maxAxis || pair.sensor.x-max > maxAxis {
			fmt.Printf("Pair %d skipped...\n", i)
			return
		}
		maxY := pair.sensor.y + max
		if maxY > maxAxis {
			maxY = maxAxis
		}
		startY := pair.sensor.y - max
		if startY < 0 {
			startY = 0
		}

		for y := startY; y <= maxY; y++ {
			xLen := max - getAbs(y-pair.sensor.y)
			startX := pair.sensor.x - xLen
			if startX < 0 {
				startX = 0
			}
			maxX := pair.sensor.x + xLen
			if maxX > maxAxis {
				maxX = maxAxis
			}
			for x := startX; x <= maxX; x++ {
				arr[y][x] = true
			}
		}
		fmt.Printf("Done with pair: %d\n", i)
	}
	for i := range arr {
		for j := range arr[i] {
			if !arr[i][j] {
				fmt.Printf("x=%d,y=%d\n", j, i)
				val := j*4000000 + i
				fmt.Println(val)
				return
			}
		}
	}
}
