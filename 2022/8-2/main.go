package main

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
)

func main() {
	content, _ := ioutil.ReadFile("input.txt")
	lines := strings.Split(string(content), "\n")

	grid := []([]int){}

	for _, line := range lines {
		line = strings.TrimSpace(strings.Replace(line, "\r", "", -1))
		rowSplit := strings.Split(line, "")

		rowArray := []int{}
		for _, row := range rowSplit {
			r, _ := strconv.Atoi(row)
			rowArray = append(rowArray, r)
		}
		grid = append(grid, rowArray)
	}

	columnCount := len(grid)
	rowCount := len(grid[0])

	maxScore := 0

	for ic, column := range grid {
		for ir, tree := range column {
			topScore := 0
			for i := ic - 1; i >= 0; i-- {
				topScore++
				if grid[i][ir] >= tree {
					break
				}
			}
			botScore := 0
			for i := ic + 1; i < columnCount; i++ {
				botScore++
				if grid[i][ir] >= tree {
					break
				}
			}

			leftScore := 0
			for i := ir - 1; i >= 0; i-- {
				leftScore++
				if grid[ic][i] >= tree {
					break
				}
			}
			rightScore := 0
			for i := ir + 1; i < rowCount; i++ {
				rightScore++
				if grid[ic][i] >= tree {
					break
				}
			}
			treeScore := topScore * botScore * leftScore * rightScore
			if treeScore > maxScore {
				maxScore = treeScore
			}
		}
	}

	fmt.Print(maxScore)
}
