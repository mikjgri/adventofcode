public class Task2(string[] input)
{
    public void Solve(){

        var left = input.ToList().Select(s => int.Parse(s.Split(" ").First().Trim())).Order();
        var right = input.ToList().Select(s => int.Parse(s.Split(" ").Last().Trim())).Order().ToList();
        
        var result = left.Sum(l => l * right.Count(r => r == l));
        Console.WriteLine(result);
    }
}