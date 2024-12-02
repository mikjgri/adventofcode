public class Task1(string[] input)
{
    public void Solve(){

        var left = input.ToList().Select(s => int.Parse(s.Split(" ").First().Trim())).Order();
        var right = input.ToList().Select(s => int.Parse(s.Split(" ").Last().Trim())).Order().ToList();
        
        var result = left.Select((item, index) => (item, index)).Sum(x => Math.Abs(x.item- right[x.index]));
        Console.WriteLine(result);
    }
}