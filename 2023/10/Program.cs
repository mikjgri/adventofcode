var text = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\input.txt");
new Task1_loop(text).Solve();
new Task2_expansion(text).Solve();