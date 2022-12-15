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

func main() {
	lines := readLinesAndTrim("input.txt")
	pairs := buildPairs(lines)

	// godRow := 10
	godRow := 2000000
	solutionRow := map[int]string{}

	minX, maxX := math.MaxInt, math.MinInt

	for i, pair := range pairs {
		fmt.Printf("Pair %d", i)
		fmt.Println()
		if pair.beacon.y == godRow {
			solutionRow[pair.beacon.x] = "B"
		}

		bsxDiff, bsyDiff := pair.beacon.x-pair.sensor.x, pair.beacon.y-pair.sensor.y
		if bsxDiff < 0 {
			bsxDiff *= -1
		}
		if bsyDiff < 0 {
			bsyDiff *= -1
		}
		max := bsxDiff + bsyDiff
		if pair.sensor.y+max < godRow || pair.sensor.y-max > godRow {
			continue
		}
		//dbg
		totalInPair := max * max
		fmt.Println("Total in pair:", totalInPair)
		doneInPair := 0

		for x := pair.sensor.x - max; x <= pair.sensor.x+max; x++ {
			if x < minX {
				minX = x
			}
			if x > maxX {
				maxX = x
			}
			for y := pair.sensor.y - max; y <= pair.sensor.y+max; y++ {
				doneInPair++
				if y != godRow {
					continue
				}
				xDiff, yDiff := x-pair.sensor.x, y-pair.sensor.y
				if xDiff < 0 {
					xDiff *= -1
				}

				if yDiff < 0 {
					yDiff *= -1
				}
				if xDiff+yDiff <= max {
					if solutionRow[x] != "B" {
						solutionRow[x] = "#"
					}
				}
			}
		}
	}
	beaconSafe := 0
	for x := minX; x <= maxX; x++ {
		if solutionRow[x] == "#" {
			beaconSafe++
		}
		// fmt.Print(solutionRow[x])
	}
	fmt.Printf("Beacon safe: %d\n", beaconSafe)
}
