var text = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\input.txt");
new Task1(text).Execute();
new Task2_Overengineered(text).Execute();
new Task2(text).Execute();