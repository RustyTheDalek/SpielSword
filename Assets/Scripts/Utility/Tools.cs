using UnityEngine;

public static class Tools
{
    public static float BellCurve(int start, int finish)
    {
        float x = Mathf.InverseLerp(start, finish, Game.t);
        float y = -Mathf.Pow(x, 2) + (4 * x) + 1;
        return y;
    }
}
