package main

import (
	"fmt"
	"os"
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
func visualize(cave [][]bool, rockCoordinates []Coord) {
	return
	for i := len(cave) - 1; i >= 0; i-- {
		if i == 0 {
			fmt.Print("+")
		} else {
			fmt.Print("|")
		}
		for j := 0; j < len(cave[i]); j++ {
			if cave[i][j] {
				if i == 0 {
					fmt.Print("-")
				} else {
					fmt.Print("#")
				}
			} else {
				hasRock := false
				for k := 0; k < len(rockCoordinates); k++ {
					if rockCoordinates[k].x == j && rockCoordinates[k].y == i {
						hasRock = true
					}
				}
				if hasRock {
					fmt.Print("@")
				} else {
					fmt.Print(".")
				}
			}
		}
		if i == 0 {
			fmt.Print("+")
		} else {
			fmt.Print("|")
		}
		fmt.Println()
	}
	fmt.Println()
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
func getNextRock(iteration int, rocks []Rock) Rock {
	return rocks[iteration%len(rocks)]
}
func getNextJetPattern(iteration int, jetPatterns []int) int {
	return jetPatterns[iteration%len(jetPatterns)]
}

func main() {
	lines := readLinesAndTrim("example.txt")
	rockLines := readLinesAndTrim("rocks.txt")
	rocks := getRocks(rockLines)
	jetPatterns := getJetPatterns(lines[0])

	cave := [][]bool{}
	floor := []bool{}
	for i := 0; i < 7; i++ {
		floor = append(floor, true)
	}
	cave = append(cave, floor)

	jetPatternIterations := 0
	for rocksFallen := 0; rocksFallen < 1000000000000; rocksFallen++ {
		if rocksFallen%10000000 == 0 {
			percentDone := float64(rocksFallen) / 1000000000000 * 100
			fmt.Printf("%.3f%% done\n", percentDone)
		}
		rock := getNextRock(rocksFallen, rocks)
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
		visualize(cave, rockCoordinates)
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
			visualize(cave, rockCoordinates)
		}
		for i := 0; i < len(rockCoordinates); i++ {
			cave[rockCoordinates[i].y][rockCoordinates[i].x] = true
		}
		visualize(cave, []Coord{})
	}
	fmt.Println(findHighestY(cave))
}
