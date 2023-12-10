public class Task1(string[] input)
{
    public void Solve(){
        int result = 0;

        foreach (var line in input){
            var isInt = (char item) => int.TryParse(item.ToString(), out var _);
            var firstN = line.First(isInt).ToString();
            var lastN = line.Last(isInt).ToString();
            result+= int.Parse(firstN+lastN);
        }
        Console.WriteLine(result);
    }
}