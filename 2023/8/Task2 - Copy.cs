//using System.Security;

//public class Task2
//{
//    private string[] _input;
//    public Task2(string[] input)
//    {
//        _input = input;
//    }
//    private Dictionary<string, (string left, string right)> GetMap()
//    {
//        var ret = new Dictionary<string, (string left, string right)>();
//        foreach (var line in _input[2..])
//        {
//            var split = line.Split("=");
//            var currentLocation = split[0].Trim();
//            var routes = split[1].Replace("(", "").Replace(")", "").Trim().Split(",");

//            ret.Add(currentLocation, (routes[0].Trim(), routes[1].Trim()));
//        }
//        return ret;
//    }
//    public void Solve()
//    {
//        var instructions = _input[0].Select(x => x.ToString()).ToList();
//        var map = GetMap();

//        var step = 0;
//        var locations = map.Select(x => x.Key).Where(item => item.EndsWith("A")).ToList();

//        var stepsInGoalDictionary = new Dictionary<int, List<int>>();

//        while (true)
//        {
//            var instruction = instructions[step % instructions.Count];

//            var newLocs = new List<string>();
//            step++;

//            foreach (var location in locations)
//            {
//                var locIndex = locations.IndexOf(location);
//                if (!stepsInGoalDictionary.ContainsKey(locIndex))
//                {
//                    stepsInGoalDictionary[locIndex] = [];
//                }

//                var newLoc = instruction == "L" ? map[location].left : map[location].right;
//                if (newLoc.EndsWith("Z"))
//                {
//                    stepsInGoalDictionary[locIndex].Add(step);
//                }
//                newLocs.Add(newLoc);
//            }
//            locations = newLocs;
            
//            bool existsInAll(int happyStep)
//            {
//                foreach (var otherLocation in stepsInGoalDictionary.Where(item => item.Key != stepsInGoalDictionary.First().Key))
//                {
//                    //var existsInThing = false;
//                    if (otherLocation.Value.Any(otherHappy => otherHappy == happyStep))
//                    {
//                        continue;
//                    }
//                    //foreach (var otherHappy in otherLocation.Value)
//                    //{
//                    //    //if (otherHappy == happyStep) existsInThing = true;
//                    //    if (otherHappy == happyStep) continue;
//                    //}
//                    return false;
//                    //if (!existsInThing)
//                    //{
//                    //    return false;
//                    //}
//                }
//                return true;
//            }

//            var firstHappy = stepsInGoalDictionary[0].FirstOrDefault(existsInAll);
//            if (firstHappy != default)
//            {
//                Console.WriteLine(firstHappy);
//                break;
//            }

//        }

//        //while (!locations.All(x => x.EndsWith("Z")))
//        //{
//        //    var instructionIndex = step % instructions.Count;
//        //    var instruction = instructions[instructionIndex];

//        //    var newLocs = new List<string>();

//        //    foreach (var location in locations)
//        //    {
//        //        newLocs.Add(instruction == "L" ? map[location].left : map[location].right);
//        //    }
//        //    locations = newLocs;
//        //    step++;
//        //}
//        //Console.WriteLine(step);
//    }
//}