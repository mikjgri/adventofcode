using CommonLib;

var text = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\input.txt");
TaskRunner.RunWithStackSize(new Task1(text).Execute, 16);
TaskRunner.RunWithStackSize(new Task2(text).Execute, 8);