package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

func readLinesAndTrim(file string) []string {
	content, _ := os.ReadFile(file)
	rawLines := strings.Split(string(content), "\n")
	lines := []string{}
	for _, line := range rawLines {
		lines = append(lines, strings.TrimSpace(strings.Replace(line, "\r", "", -1)))
	}
	return lines
}

type Rock struct {
	polygons [][]bool
}
type Coord struct {
	x, y int
}
type State struct {
	height        int
	rockIteration int
}

func getRocks(lines []string) []Rock {
	rocks := []Rock{}
	rock := Rock{polygons: [][]bool{}}
	for _, line := range lines {
		if line == "" {
			rocks = append(rocks, rock)
			rock = Rock{polygons: [][]bool{}}
		} else {
			polygon := []bool{}
			for _, char := range line {
				if char == '#' {
					polygon = append(polygon, true)
				} else {
					polygon = append(polygon, false)
				}
			}
			rock.polygons = append(rock.polygons, polygon)
		}
	}
	return rocks
}
func getJetPatterns(line string) []int {
	patterns := []int{}
	for _, char := range line {
		var pattern int
		if string(char) == "<" {
			pattern = -1
		} else {
			pattern = 1
		}
		patterns = append(patterns, pattern)
	}
	return patterns
}
func findHighestY(cave [][]bool) int {
	for i := len(cave) - 1; i > 0; i-- {
		for j := 0; j < len(cave[i]); j++ {
			if cave[i][j] {
				return i
			}
		}
	}
	return 0
}
func getRockSpawn(rock Rock, cave [][]bool) (int, int) {
	highestY := findHighestY(cave)
	rockHeight := len(rock.polygons)
	y, x := highestY+rockHeight+3, 2
	return x, y
}
func canMoveDown(rockCoordinates []Coord, cave [][]bool) bool {
	for i := 0; i < len(rockCoordinates); i++ {
		if cave[rockCoordinates[i].y-1][rockCoordinates[i].x] {
			return false
		}
	}
	return true
}
func canBePushed(rockCoordinates []Coord, cave [][]bool, direction int) bool {
	for i := 0; i < len(rockCoordinates); i++ {
		//hits wall
		if rockCoordinates[i].x+direction < 0 || rockCoordinates[i].x+direction >= len(cave[0]) {
			return false
		}
		//hits rock
		if cave[rockCoordinates[i].y][rockCoordinates[i].x+direction] {
			return false
		}
	}
	return true
}
func getRockIndex(iteration int, rocks []Rock) int {
	return iteration % len(rocks)
}
func getNextRock(iteration int, rocks []Rock) Rock {
	return rocks[getRockIndex(iteration, rocks)]
}
func getJetPatternIndex(iteration int, jetPatterns []int) int {
	return iteration % len(jetPatterns)
}
func getNextJetPattern(iteration int, jetPatterns []int) int {
	return jetPatterns[getJetPatternIndex(iteration, jetPatterns)]
}
func getStateHash(cave [][]bool, jetPatternIndex, rockIndex int) string {
	maxY := findHighestY(cave)
	topNodes := []int{}
	for x := 0; x < len(cave[0]); x++ {
		for y := len(cave) - 1; y >= 0; y-- {
			if cave[y][x] {
				topNodes = append(topNodes, y-maxY)
				break
			}
		}
	}
	hash := fmt.Sprintf("%v", topNodes)
	hash += strconv.Itoa(jetPatternIndex)
	hash += "--"
	hash += strconv.Itoa(rockIndex)
	return hash
}
func main() {
	lines := readLinesAndTrim("input.txt")
	rockLines := readLinesAndTrim("rocks.txt")
	rocks := getRocks(rockLines)
	jetPatterns := getJetPatterns(lines[0])

	cave := [][]bool{}
	floor := []bool{}
	for i := 0; i < 7; i++ {
		floor = append(floor, true)
	}
	cave = append(cave, floor)

	stateHashList := map[string]State{}
	jetPatternIterations := 0
	rockIteration := 0

	fireInTheHole := func(cave [][]bool) [][]bool {
		rock := getNextRock(rockIteration, rocks)
		spawnX, spawnY := getRockSpawn(rock, cave)
		//place rock
		rockCoordinates := []Coord{}
		for i := 0; i < len(rock.polygons); i++ {
			for j := 0; j < len(rock.polygons[i]); j++ {
				if rock.polygons[i][j] {
					rockCoordinates = append(rockCoordinates, Coord{x: spawnX + j, y: spawnY - i})
				}
			}
		}

		//extend cave
		missingRows := spawnY - len(cave) + 1
		for i := 0; i < missingRows; i++ {
			cave = append(cave, []bool{})
			for j := 0; j < len(cave[0]); j++ {
				cave[len(cave)-1] = append(cave[len(cave)-1], false)
			}
		}
		rockIsMoving := true
		for rockIsMoving {
			rockIsMoving = false
			jetPattern := getNextJetPattern(jetPatternIterations, jetPatterns)
			if canBePushed(rockCoordinates, cave, jetPattern) {
				for i := 0; i < len(rockCoordinates); i++ {
					rockCoordinates[i].x += jetPattern
				}
			}
			jetPatternIterations++
			if canMoveDown(rockCoordinates, cave) {
				rockIsMoving = true
				for i := 0; i < len(rockCoordinates); i++ {
					rockCoordinates[i].y--
				}
			}
		}
		for i := 0; i < len(rockCoordinates); i++ {
			cave[rockCoordinates[i].y][rockCoordinates[i].x] = true
		}
		return cave
	}

	currentState := State{height: 0, rockIteration: 0}
	targetIterations := 1000000000000
	duplicateStateFound := false
	for !duplicateStateFound {
		cave = fireInTheHole(cave)
		highestY := findHighestY(cave)
		stateHash := getStateHash(cave, getJetPatternIndex(jetPatternIterations, jetPatterns), getRockIndex(rockIteration, rocks))
		if prevState, ok := stateHashList[stateHash]; ok {
			currentState.height = prevState.height
			currentState.rockIteration = prevState.rockIteration

			diffHeight := highestY - prevState.height
			diffIterations := rockIteration - prevState.rockIteration
			cyclesToAdd := (targetIterations - currentState.rockIteration) / diffIterations

			currentState.height += cyclesToAdd * diffHeight
			currentState.rockIteration += cyclesToAdd * diffIterations

			duplicateStateFound = true
		} else {
			stateHashList[stateHash] = State{height: highestY, rockIteration: rockIteration}
		}
		rockIteration++
	}
	rockIteration = currentState.rockIteration
	for rockIteration < targetIterations {
		oldHeight := findHighestY(cave)
		cave = fireInTheHole(cave)
		newHeight := findHighestY(cave)
		currentState.height += newHeight - oldHeight
		rockIteration++
	}
	fmt.Println(currentState.height)
}
