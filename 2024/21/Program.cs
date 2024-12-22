using CommonLib;

var text = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\input.txt");
new Task1(text).Execute();
TaskRunner.RunWithStackSize(new Task2(text).Execute, 16);
//TaskRunner.RunWithStackSize(new Task2_2(text).Execute, 16);
//TaskRunner.RunWithStackSize(new Task2(text).Execute, 8);