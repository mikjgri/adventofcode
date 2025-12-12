using CommonLib;
using CommonLib.Solvers;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

public class Task1(string[] input) : BaseTask()
{
    protected override object Solve()
    {
        var shapesInput = input.TakeWhile(x => x.Length <= 3).ToArray();
        var regionInput = input[shapesInput.Length..];

        var shapes = ParseShapes(shapesInput);
        var regions = ParseRegions(regionInput);
        Console.WriteLine($"Regions: {regions.Count}");

        //filterRegions
        regions = regions.Where(r => r.ShapesQuantity.Sum(sq => sq.quantity * shapes[sq.index].Size) <= r.SizeX * r.SizeY).ToList();
        Console.WriteLine($"Optimized away impossible regions. Regions left: {regions.Count}"); //wtf. this works as the task 1 answer?

        var sum = 0;
        foreach (var region in regions)
        {
            Console.WriteLine($"Processing... {regions.IndexOf(region)}/{regions.Count}");
            var regionGrid = GridTools.InitializeGridArray(region.SizeX, region.SizeY, false);
            var firstShape = GetFirstShapeAndNewQuantity(region.ShapesQuantity);
            var r = CanFitShape(region, firstShape.shape, [], firstShape.shapesQuantity);
            Console.WriteLine(r);
            if (r) sum++;
        }

        bool CanFitShape(Region region, Shape shape, List<(int x, int y)> filledRegion, (int index, int quantity)[] shapesQuantity)
        {
            for (var x = 0; x < region.SizeX; x++)
            {
                for (var y = 0; y < region.SizeY; y++)
                {
                    foreach (var variant in shape.Variants)
                    {
                        (int x, int y)[] positionedVariant = variant.Select(p => (p.x + x, p.y + y)).ToArray();
                        if (positionedVariant.Any(pv => pv.x > region.SizeX - 1 || pv.y > region.SizeY - 1))
                        {
                            continue;
                        }
                        if (positionedVariant.Any(pv => filledRegion.Contains(pv)))
                        {
                            continue;
                        }
                        var newRegion = filledRegion.Union(positionedVariant).ToList();
                        if (shapesQuantity.All(sq => sq.quantity == 0)) return true;
                        var nextShape = GetFirstShapeAndNewQuantity(shapesQuantity);
                        if (CanFitShape(region, nextShape.shape, newRegion, nextShape.shapesQuantity)) return true;
                    }
                }
            }
            return false;
        }

        (Shape shape, (int index, int quantity)[] shapesQuantity) GetFirstShapeAndNewQuantity((int index, int quantity)[] shapesQuantity)
        {
            var sI = shapesQuantity.FirstOrDefault(s => s.quantity > 0);
            var newSq = shapesQuantity.ToArray();
            newSq[newSq.IndexOf(sI)].quantity--;
            return (shapes[sI.index], newSq);
        }

        return sum;
    }
    static List<Shape> ParseShapes(string[] shapesInput)
    {
        List<Shape> ret = [];
        for (var i = 0; i < shapesInput.Length; i += 5)
        {
            var shape = shapesInput[(i + 1)..(i + 4)];
            (int x, int y)[] coordinates = [.. Enumerable.Range(0, shape.Length).SelectMany(y => Enumerable.Range(0, shape[0].Length).Select(x => (x, y)).Where(c => shape[c.y][c.x] == '#'))];
            var variants = new List<(int x, int y)[]>
            {
                coordinates,
                MirrorHorizontally(coordinates)
            };
            var prevOrientation = coordinates;
            for (var r = 0; r < 3; r++)
            {
                var rotated = Rotate90Right(prevOrientation);
                variants.Add(rotated);
                variants.Add(MirrorHorizontally(rotated));
                prevOrientation = rotated;
            }

            ret.Add(new Shape(variants, coordinates.Length));
        }
        (int x, int y)[] Rotate90Right((int x, int y)[] coordinates)
        {
            return [.. coordinates.Select(c => (2 - c.y, (c.x)))];
        }
        (int x, int y)[] MirrorHorizontally((int x, int y)[] coordinates)
        {
            return [.. coordinates.Select(c => (c.x, 2 - c.y))];
        }
        return ret;
    }
    static List<Region> ParseRegions(string[] regionInput)
    {
        var ret = regionInput.Select(line =>
        {
            var s1 = line.Split(": ");
            var s2 = s1[0].Split("x");

            var shapeQuantities = s1[1].Split(" ")
                .Select((v, i) => (index: i, quantity: int.Parse(v)))
                .Where(t => t.quantity > 0)
                .ToArray();

            return new Region(int.Parse(s2[0]), int.Parse(s2[1]), shapeQuantities);
        });
        return [.. ret];
    }
    record Shape(List<(int x, int y)[]> Variants, int Size);
    record Region(int SizeX, int SizeY, (int index, int quantity)[] ShapesQuantity);
}