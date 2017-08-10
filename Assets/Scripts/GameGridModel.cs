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
        UnityEngine.Debug.Assert(left <= right && top <= bottom);
        // range check
        if (left < 0 || left >= WIDTH || right < 0 || right >= WIDTH ||
            top < 0 || top >= HEIGHT || bottom < 0 || bottom >= HEIGHT)
            return false;
        // nothing to do
        if (left == right || top == bottom) return false;
        // solid check:
        for (int i = left; i <= right; i++)
        {
            if (IsSolid(grid[i, top]) || IsSolid(grid[i, bottom])) return false;
        }
        for (int j = top; j <= bottom; j++)
        {
            if (IsSolid(grid[left, j]) || IsSolid(grid[right, j])) return false;
        }

        return true;
    }

    // rotates grid (anti)clockwise by one unit
    public void Rotate(int left, int top, int right, int bottom, bool clockwise)
    {
        UnityEngine.Debug.Assert(left <= right && top <= bottom);
        if (!CanRotate(left, top, right, bottom)) return;
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
    }

    public bool IsClear(int val)
    {
        return val == 0;
    }
    public bool CanFall(int val)
    {
        return val >= 2;
    }
    public bool IsSolid(int val)
    {
        return val == 1;
    }

    // checks for falling blocks and returns them all
    public bool CheckForFalling(List<KeyValuePair<int,int>> pairs)
    {
        pairs.Clear();
        // working left=>right & bottom=> top
        for(int i=0;i<WIDTH;i++)
        {
            bool falling = false;
            for(int j=HEIGHT-2; j>=0;j--) // looking at j,j+1
            {
                // if it can fall & its on nothing or on a falling block
                falling = (CanFall(grid[i, j]) &&
                    (falling || IsClear(grid[i, j + 1])));
                if (falling)
                {
                    pairs.Add(new KeyValuePair<int, int>(i, j));
                }
            }
        }
        return pairs.Count > 0;
    }

    // moves a cell down by 1
    public void MoveDown(int x,int y)
    {
        grid[x, y + 1] = grid[x, y];
        grid[x, y] = 0;
    }

}
