package main

import (
	"fmt"
	"io/ioutil"
	"math"
	"regexp"
	"strconv"
	"strings"
	"sync"
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
func main() {
	lines := readLinesAndTrim("input.txt")
	pairs := buildPairs(lines)
	pairsLength := len(pairs)

	// godRow := 10
	godRow := 2000000
	someMap := sync.Map{}
	var wg sync.WaitGroup
	wg.Add(pairsLength)
	for i := 0; i < pairsLength; i++ {
		go func(i int) {
			defer wg.Done()
			pair := pairs[i]
			if pair.beacon.y == godRow {
				someMap.Store(pair.beacon.x, "B")
			}

			bsxDiff, bsyDiff := getAbs(pair.beacon.x-pair.sensor.x), getAbs(pair.beacon.y-pair.sensor.y)
			max := bsxDiff + bsyDiff
			if pair.sensor.y+max < godRow || pair.sensor.y-max > godRow {
				fmt.Printf("Pair %d skipped...\n", i)
				return
			}
			godRowDiff := getAbs(pair.sensor.y - godRow)
			lenToCheck := max - godRowDiff
			for j := pair.sensor.x - lenToCheck; j <= pair.sensor.x+lenToCheck; j++ {
				val, _ := someMap.Load(j)
				if val != "B" {
					someMap.Store(j, "#")
				}
			}
			fmt.Printf("Done with pair: %d\n", i)
		}(i)
	}
	wg.Wait()

	beaconSafe := 0

	someMap.Range(func(k, v interface{}) bool {
		val := v.(string)
		if val == "#" {
			beaconSafe++
		}
		return true
	})
	fmt.Printf("Beacon safe: %d\n", beaconSafe)
}
