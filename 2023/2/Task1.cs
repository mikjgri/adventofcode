public class Task1{
    private string[] _input;
    public Task1(string[] input){
        _input = input;
    }
    public void Solve(){
        int result = 0;

        foreach (var line in _input){
            var isInt = (char item) => int.TryParse(item.ToString(), out var _);
            var firstN = line.First(isInt).ToString();
            var lastN = line.Last(isInt).ToString();
            result+= int.Parse(firstN+lastN);
        }
        Console.WriteLine(result);
    }
}