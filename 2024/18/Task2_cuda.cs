using CommonLib;
using ILGPU;
using ILGPU.IR;
using ILGPU.IR.Values;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;
using ILGPU.Runtime.OpenCL;
using Spectre.Console;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Task2_cuda(string[] input) : BaseTask()
{
    private int byteDropIndex = 1024;
    private int max = 69;
    protected override void Solve()
    {
        Position[] bytePositions = input.Select(line => { var s = line.Split(","); return new Position(int.Parse(s[0]), int.Parse(s[1])); }).ToArray();
        using Context context = Context.CreateDefault();

        using var accelerator = context.GetCudaDevice(0).CreateAccelerator(context);

        var kernel = accelerator.LoadAutoGroupedStreamKernel<
            Index1D, ArrayView<int>>(KernelCode);

        using var bytePositionBuffer = accelerator.Allocate1D<Position>(bytePositions.Length);
        bytePositionBuffer.CopyFromCPU(bytePositions);
        using var outputBuffer = accelerator.Allocate1D<int>(bytePositions.Length);

        kernel((int)outputBuffer.Length, outputBuffer.View);
        //var outputData = outputBuffer.GetAsArray1D().ToList();

    }

    static void KernelCode(
        Index1D index,
        ArrayView<int> outputView
        )
    {
        var max = 69;
        var grid = new string[max, max];

        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < max; j++)
            {
                grid[i, j] = ".";
            }
        }

        var directions = new (int xOff, int yOff)[]
        {
            (0, 1), (1, 0), (0, -1), (-1, 0)
        };
        //var grid2 = Enumerable.Range(0, max).Select(c => Enumerable.Range(0, max).Select(r => ".").ToArray()).ToArray();

        outputView[index] = getTimes10(0);
        int getTimes10(int p1)
        {
            if (p1 == 10)
            {
                return 5;
            }
            else
            {
                return getTimes10(p1 + 1);
            }
        }
        //var max = 69;
        //var grid = GridTools.InitializeGrid(max + 1, max + 1, '.');
        //for (var i = 0; i < index; i++)
        //{
        //    var bp = bytePositions[i];
        //    grid[bp.Y][bp.X] = '#';
        //}
        //var directions = GridTools.GetSquare4DirectionOffsets();
        //var log = new Dictionary<string, int>();
        //var possible = RunForrestRun((0, 0), 0);
        //outputView[index] = possible;
        //bool RunForrestRun((int x, int y) position, int steps)
        //{
        //    var key = $"{position}";
        //    if (!GridTools.IsInGrid(position, grid) || grid[position.y][position.x] == '#' || (log.ContainsKey(key) && log[key] <= steps))
        //    {
        //        return false;
        //    }
        //    log[key] = steps;
        //    if (position.x == max && position.y == max)
        //    {
        //        return true;
        //    }
        //    return directions.Any(dir => RunForrestRun((position.x + dir.xOff, position.y + dir.yOff), steps + 1));
        //}
    }
    struct Position
    {
        public int X;
        public int Y;

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
