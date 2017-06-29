using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// format for the level to be loaded from the json 
[Serializable]
public struct LevelFormat
{
    public int[] leftGrid; // len 99, 9x11
    public int[] rightGrid; // len 99, 9x11
    public int time;    // time for game (in seconds)
}
