public class Task2{
    private string[] numbers = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
    private string[] _input;
    public Task2(string[] input){
        _input = input;
    }
    public void Solve(){
        int result = 0;

        int? getNumber(int cI, string line){
            var current = line[cI].ToString();
            var rest = line[(cI+1)..];
            if (int.TryParse(current.ToString(), out var intNumber)){
                return intNumber;
            }
            var remaining = current+rest;
            var strNumber = numbers.FirstOrDefault(remaining.StartsWith);
            if (!string.IsNullOrEmpty(strNumber)){
                return numbers.ToList().IndexOf(strNumber)+1;
            }
            return null;
        }

        foreach (var line in _input){
            var first = 0;
            for (var i = 0; i < line.Length; i++){
                var happy = getNumber(i, line);
                if (happy.HasValue) {
                    first = happy.Value; break;
                }
            }
            var last = 0;
            for (var i = line.Length-1; i >= 0 ; i--){
                var happy = getNumber(i, line);
                if (happy.HasValue) {
                    last = happy.Value; break;
                }
            }
            result+= int.Parse(first.ToString()+last.ToString());
        }
        Console.WriteLine(result);
    }
}