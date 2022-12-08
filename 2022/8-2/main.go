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

	forest := []([]int){}

	for _, line := range lines {
		line = strings.TrimSpace(strings.Replace(line, "\r", "", -1))
		rowSplit := strings.Split(line, "")

		rows := []int{}
		for _, row := range rowSplit {
			r, _ := strconv.Atoi(row)
			rows = append(rows, r)
		}
		forest = append(forest, rows)
	}

	columnCount := len(forest)
	rowCount := len(forest[0])

	maxScore := 0

	for ic, column := range forest {
		for ir, tree := range column {
			topScore := 0
			for i := ic - 1; i >= 0; i-- {
				topScore++
				if forest[i][ir] >= tree {
					break
				}
			}
			botScore := 0
			for i := ic + 1; i < columnCount; i++ {
				botScore++
				if forest[i][ir] >= tree {
					break
				}
			}

			leftScore := 0
			for i := ir - 1; i >= 0; i-- {
				leftScore++
				if forest[ic][i] >= tree {
					break
				}
			}
			rightScore := 0
			for i := ir + 1; i < rowCount; i++ {
				rightScore++
				if forest[ic][i] >= tree {
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
