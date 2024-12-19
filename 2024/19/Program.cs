var text = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\input.txt");
new Task1(text).Execute();
new Task2_caching(text).Execute();
//new Task2_bruteforce(text).Execute();