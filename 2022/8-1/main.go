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

	visibleTrees := 0
	for ic, column := range forest {
		for ir, tree := range column {
			inColumnBorder := ic == 0 || ic == columnCount-1
			inRowBorder := ir == 0 || ir == rowCount-1

			if inColumnBorder || inRowBorder {
				visibleTrees++
				fmt.Printf("tree %d at %d,%d is border visible", tree, ir, ic)
				fmt.Println()
				continue
			}

			if !inColumnBorder {
				topVisible := true
				for i := 0; i < ic; i++ {
					if forest[i][ir] >= tree {
						topVisible = false
						break
					}
				}
				botVisible := true
				for i := columnCount - 1; i > ic; i-- {
					if forest[i][ir] >= tree {
						botVisible = false
						break
					}
				}
				if topVisible || botVisible {
					visibleTrees++
					fmt.Printf("tree %d at %d,%d visible", tree, ir, ic)
					fmt.Println()
					continue
				}
			}
			if !inRowBorder {
				leftVisible := true
				for i := 0; i < ir; i++ {
					if forest[ic][i] >= tree {
						leftVisible = false
						break
					}
				}
				rightVisible := true
				for i := rowCount - 1; i > ir; i-- {
					if forest[ic][i] >= tree {
						rightVisible = false
						break
					}
				}
				if leftVisible || rightVisible {
					visibleTrees++
					fmt.Printf("tree %d at %d,%d visible", tree, ir, ic)
					fmt.Println()
					continue
				}
			}
		}
	}

	fmt.Print(visibleTrees)
}
