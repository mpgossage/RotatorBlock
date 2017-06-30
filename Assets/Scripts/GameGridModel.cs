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

    // returns if the grid set can rotate
    public bool CanRotate(int left, int top, int right, int bottom)
    {
        // range check
        if (left < 0 || left >= WIDTH || right < 0 || right >= WIDTH ||
            top < 0 || top >= HEIGHT || bottom < 0 || bottom >= HEIGHT)
            return false;
        // nothing to do
        if (left == right || top == bottom) return false;
        // TODO: solid check
        return true;
    }

    // rotates grid (anti)clockwise by one unit
    public bool Rotate(int left, int top, int right, int bottom, bool clockwise)
    {
        if (!CanRotate(left, top, right, bottom)) return false;
        // if incorrect order switch left/right top/bottom
        if (left > right)
        {
            int t = left;
            left = right;
            right = t;
        }
        if (top > bottom)
        {
            int t = top;
            top = bottom;
            bottom = t;
        }
        if (clockwise)
        {
            int topLeft = grid[left, top]; // for later:
            for (int j = top; j < bottom; j++) // shift LT->LB up
                grid[left, j] = grid[left, j + 1];
            for (int i = left; i < right; i++) // shift LB->RB left
                grid[i, bottom] = grid[i + 1, bottom];
            for (int j = bottom; j > top; j--) // shift RB->RT down
                grid[right, j] = grid[right, j - 1];
            for (int i = right; i > left; i--) // shift RT->LT right
                grid[i, top] = grid[i - 1, top];
            grid[left + 1, top] = topLeft; // replace overwritten square
        }
        else
        {
            int topLeft = grid[left, top]; // for later:
            for (int i = left; i < right; i++) // shift LT->RT left
                grid[i, top] = grid[i + 1, top];
            for (int j = top; j < bottom; j++) // shift RT->RB up
                grid[right, j] = grid[right, j + 1];
            for (int i = right; i > left; i--) // shift RB->LB right
                grid[i, bottom] = grid[i - 1, bottom];
            for (int j = bottom; j > top; j--) // shift LB->LT down
                grid[left, j] = grid[left, j - 1];
            grid[left, top + 1] = topLeft; // replace overwritten square
        }
        return true;
    }

}
