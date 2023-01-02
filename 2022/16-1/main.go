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

func main() {
	lines := readLinesAndTrim("input.txt")
	valves := getAllVales(lines)
	maxTime := 30

	var move func(state State, position string, openValve bool) State
	move = func(state State, position string, openValve bool) State {
		if state.time >= maxTime {
			return state
		}

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
		if valveCanBeOpened && openValve {
			state.openValves = append(state.openValves, position)
			state.visitedValvesAtState = []string{}
		}

		//has visited at current state
		for _, visitedValve := range state.visitedValvesAtState {
			if visitedValve == position {
				return state
			}
		}
		state.visitedValvesAtState = append(state.visitedValvesAtState, position)

		for _, openedValve := range state.openValves {
			state.pressureReleased += valves[openedValve].flow
		}
		state.time++

		paths := []State{}
		if valveCanBeOpened {
			newState := cloneState(state)
			paths = append(paths, move(newState, position, true))
		}
		for _, connection := range valve.connections {
			newState := cloneState(state)
			paths = append(paths, move(newState, connection, false))
		}
		if len(paths) == 0 { //no more paths
			return state
		}

		bestPath := State{pressureReleased: -1}
		for _, path := range paths {
			if path.pressureReleased > bestPath.pressureReleased {
				bestPath = path
			}
		}
		return bestPath
	}
	bestPath := move(State{time: 0, pressureReleased: 0, openValves: []string{}, visitedValvesAtState: []string{}}, "AA", false)
	fmt.Println(bestPath.pressureReleased)
}
