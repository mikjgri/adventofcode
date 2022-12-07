// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var tree = new List<TreeNode>() {
    new TreeNode()
    {
        Name = "/",
        Size = 0,
        Type = NodeType.Directory,
        Children = new()
    }
};
var rootElement = tree[0];

var text = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\input.txt");

var currentElement = rootElement;
foreach (var line in text)
{
    if (line.Contains("$")) //command
    {
        if (line.Contains(" cd "))
        {
            var path = Regex.Match(line, "(?<=cd ).*").Value;
            if (path == "/")
            {
                currentElement = rootElement;
            }
            else if (path == "..")
            {
                currentElement = currentElement.Parent;
            }
            else
            {
                var existingChild = currentElement.Children.FirstOrDefault(item => item.Name == path);
                if (existingChild == null)
                {
                    currentElement.Children.Add(new TreeNode()
                    {
                        Name = path,
                        Size = 0,
                        Type = NodeType.Directory,
                        Parent = currentElement,
                        Children = new()
                    });
                }
                else
                {
                    currentElement = existingChild;
                }
            }
            continue;
        }
        if (line.Contains(" ls"))
        {
            //should I do something here?
            continue;
        }
    }
    if (line.StartsWith("dir "))
    {
        var dir = Regex.Match(line, "(?<=dir ).*").Value;
        if (!currentElement.Children.Any(item => item.Name == dir))
        {
            currentElement.Children.Add(new TreeNode()
            {
                Name = dir,
                Type = NodeType.Directory,
                Parent = currentElement,
                Size = 0,
                Children = new()
            });
        }
        continue;
    }
    //else this should be a file
    var split = line.Split(" ");
    var size = int.Parse(split[0]);
    var name = split[1];

    if (!currentElement.Children.Any(item => item.Name == name))
    {
        currentElement.Children.Add(new TreeNode()
        {
            Name = name,
            Size = size,
            Type = NodeType.File,
            Parent = currentElement
        });
    }
}
List<int> GetDirectorySizes(List<TreeNode> nodes)
{
    var directorySizes = new List<int>();
    foreach (var node in nodes)
    {
        if (node.Type == NodeType.Directory)
        {
            directorySizes.Add(node.Size);
            directorySizes.AddRange(GetDirectorySizes(node.Children));
        }
    }
    return directorySizes;
}
var spaceToDelete = 30000000 - (70000000 - rootElement.Size);

var directorySizes = GetDirectorySizes(tree).Order();
var dirToDelete = directorySizes.FirstOrDefault(item => item > spaceToDelete);

Console.WriteLine(dirToDelete);

class TreeNode
{
    public TreeNode Parent;
    private int _size;
    public int Size
    {
        get
        {
            if (Type == NodeType.Directory)
            {
                return Children.Sum(item => item.Size);
            }
            else
            {
                return _size;
            }
        }
        set
        {
            _size = value;
        }
    }
    public string Name;
    public NodeType Type;
    public List<TreeNode> Children;
}
enum NodeType
{
    File,
    Directory
}