package main

import (
	"fmt"
	"os"
	"regexp"
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

type Valve struct {
	connections []string
	flow        int
}
type State struct {
	time                 int
	pressureReleased     int
	openValves           []string
	visitedValvesAtState []string //null when new valve opened
}

func getAllVales(lines []string) map[string]Valve {
	flowRegex, _ := regexp.Compile(`\d+`)
	valvesRegex, _ := regexp.Compile(`[A-Z]{2}`)
	valves := map[string]Valve{}

	for _, line := range lines {
		valve := Valve{connections: []string{}, flow: 0}
		valveStrings := valvesRegex.FindAllString(line, -1)
		flowString := flowRegex.FindAllString(line, -1)[0]
		valve.flow, _ = strconv.Atoi(flowString)
		valve.connections = append(valve.connections, valveStrings[1:]...)
		valves[valveStrings[0]] = valve
	}
	return valves
}
func cloneState(state State) State {
	newState := State{time: state.time, pressureReleased: state.pressureReleased, openValves: []string{}, visitedValvesAtState: []string{}}
	for _, valve := range state.openValves {
		newState.openValves = append(newState.openValves, valve)
	}
	for _, valve := range state.visitedValvesAtState {
		newState.visitedValvesAtState = append(newState.visitedValvesAtState, valve)
	}
	return newState
}

type Move struct {
	openValve bool
	position  string
}

func main() {
	lines := readLinesAndTrim("example.txt")
	valves := getAllVales(lines)
	maxTime := 30

	var move func(state State, moves []Move) State
	move = func(state State, moves []Move) State {
		if state.time >= maxTime {
			return state
		}

		positions := []string{}
		for _, move := range moves {
			positions = append(positions, move.position)
			if move.openValve {
				state.openValves = append(state.openValves, move.position)
				state.visitedValvesAtState = []string{}
			}
		}

		for _, openedValve := range state.openValves {
			state.pressureReleased += valves[openedValve].flow
		}
		state.time++

		positionsHash := strings.Join(positions, "")
		//has visited at current state
		for _, visitedHash := range state.visitedValvesAtState {
			if visitedHash == positionsHash {
				return state
			}
		}
		state.visitedValvesAtState = append(state.visitedValvesAtState, positionsHash)

		nextMoves := [][]Move{}

		for _, position := range positions {
			cMoves := []Move{}
			valve := valves[position]
			valveCanBeOpened := valve.flow > 0
			if valveCanBeOpened {
				for _, openedValve := range state.openValves {
					if openedValve == position {
						valveCanBeOpened = false
						break
					}
				}
			}

			if valveCanBeOpened {
				cMoves = append(cMoves, Move{openValve: true, position: position})
			}
			for _, connection := range valve.connections {
				cMoves = append(cMoves, Move{openValve: false, position: connection})
			}
			nextMoves = append(nextMoves, cMoves)
		}

		bestPath := State{pressureReleased: -1}
		newState := cloneState(state)
		for _, fMoves := range nextMoves[0] {
			for _, sMoves := range nextMoves[1] {
				cMoves := []Move{fMoves, sMoves}
				moveRes := move(newState, cMoves)
				if moveRes.pressureReleased > bestPath.pressureReleased {
					bestPath = moveRes
				}
			}
		}
		return bestPath
	}
	// startMoves := []Move{{openValve: false, position: "AA"}}
	startMoves := []Move{{openValve: false, position: "AA"}, {openValve: false, position: "AA"}}
	bestPath := move(State{time: 0, pressureReleased: 0, openValves: []string{}, visitedValvesAtState: []string{}}, startMoves)
	fmt.Println(bestPath.pressureReleased)
}
