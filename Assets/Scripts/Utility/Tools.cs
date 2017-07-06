using UnityEngine;
using System.Collections.Generic;

public static class Tools
{
    public static float BellCurve(int start, int finish)
    {
        float x = Mathf.InverseLerp(start, finish, Game.t);
        float y = -Mathf.Pow(x, 2) + (4 * x) + 1;
        return y;
    }


    public static bool WithinRange<T>(int index, List<T> list)
    {
        if (index < list.Count && index >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
