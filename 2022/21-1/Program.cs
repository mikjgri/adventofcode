using System.Text.RegularExpressions;

var text = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\input.txt");

var tasks = new Dictionary<string, Func<long>>();
long buildTask(string id1, string id2, string operand)
{
    var v1 = tasks[id1]();
    var v2 = tasks[id2]();
    switch (operand)
    {
        case "+":
            return v1 + v2;
        case "-":
            return v1 - v2;
        case "*":
            return v1 * v2;
        case "/":
            return v1 / v2;
    }
    throw new NotImplementedException();
}

foreach (var line in text)
{
    var s1 = line.Split(":");
    var left = s1[0];
    var right = s1[1].Trim();

    var val = Regex.Match(right, "\\d+");
    if (val.Success)
    {
        tasks.Add(left, () => int.Parse(val.Value));
        continue;
    }

    var operand = Regex.Match(right, "[+*/-]");
    var s2 = right.Split(" ");
    tasks.Add(left, () => buildTask(s2[0], s2[2], operand.Value));
}
var result = tasks["root"]();
Console.WriteLine(result);
Console.ReadLine();