using System;

public static class Lerp
{
    public static int Stat(int Min, int Max, int Level)
    {
        float x = 0;
        float perc = (float)Level / Globals.MaxLevel;
        if (Level == 1) { perc = 0; }
        x = ((Max - Min) * perc) + Min;
        return (int)x;
    }
}
