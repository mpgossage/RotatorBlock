using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class GameGridModel
{
    public const int WIDTH = 9, HEIGHT = 11;
    private int[,] grid;
    public int GetGrid(int x, int y)
    {
        return grid[x, y];
    }

    public void Init(int[] newGrid)
    {
        grid = new int[WIDTH, HEIGHT];
        for(int i=0;i<WIDTH*HEIGHT;i++)
        {
            int x = i % WIDTH;
            int y = i / WIDTH;
            grid[x, y] = newGrid[i];
        }
    }

}
