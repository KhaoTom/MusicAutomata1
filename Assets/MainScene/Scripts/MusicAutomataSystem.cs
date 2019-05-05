﻿
public class MusicAutomataSystem
{
    readonly int max;
    readonly int w;
    readonly int h;
    private int[,] cells;

    private int generationCount = 0;

    public int GenerationCount { get => generationCount; }

    public MusicAutomataSystem(int width, int height, int maxValue)
    {
        max = maxValue;
        h = height;
        w = width;
        cells = new int[w, h];
    }

    public int IncrementCell(int x, int y)
    {
        if (cells[x, y] < max) cells[x, y] += 1;
        return cells[x, y];
    }

    public void AdvanceGeneration()
    {
        ++generationCount;
        int[,] nextCells = new int[w, h];
        for (int x = 0; x < w; x++)
        {
            // get left and right neighbor coords, wrapping around at edges
            int lx = x == 0 ? w - 1 : x - 1;
            int rx = x == w - 1 ? 0 : x + 1;

            for (int y = 0; y < h; y++)
            {
                // get bottom and top neighbor coords, wrapping around at edges
                int by = y == 0 ? h - 1 : y - 1;
                int ty = y == h - 1 ? 0 : y + 1;

                // count neighbors that have current cell value + 1
                int dv = cells[x, y] + 1;
                int count = 0;
                {
                    if (cells[lx, ty] == dv) ++count;
                    if (cells[x, ty] == dv) ++count;
                    if (cells[rx, ty] == dv) ++count;
                    if (cells[lx, y] == dv) ++count;
                    if (cells[rx, y] == dv) ++count;
                    if (cells[lx, by] == dv) ++count;
                    if (cells[x, by] == dv) ++count;
                    if (cells[rx, by] == dv) ++count;
                }

                // set next cell value
                // cell with two or three neighbours stays at current value
                // cell with four neighbours increments by 1, clamped at max
                // cell with five neighbours increments by 2, clamped at max
                // otherwise the cell decrements, clamped at 0
                int cv = cells[x, y];
                if (count == 2 || count == 3)
                {
                    nextCells[x, y] = cv;
                }
                else if (count == 4)
                {
                    if (cv < max) nextCells[x, y] = cv + 1;
                }
                else if (count == 5)
                {
                    if (cv < max) nextCells[x, y] = cv + 1;
                    if (cv < max) nextCells[x, y] = cv + 1;
                }
                else
                {
                    if (cv > 0) nextCells[x, y] = cv - 1;
                }

            }
        }

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                cells[x, y] = nextCells[x, y];
            }
        }
    }

    public delegate void ForEachCellDelegate(int x, int y, int value);

    public void ForEachCell(ForEachCellDelegate callback)
    {
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                callback(x, y, cells[x,y]);
            }
        }
    }
}
