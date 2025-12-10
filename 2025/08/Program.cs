var text = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\input.txt");
new Task1(text).Execute();
new Task1And2(text).Execute();