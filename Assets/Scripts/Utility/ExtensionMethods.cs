using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{ 
	public static float Sqd(this float val)
    {
        return val = val * val;
    }

    public static Vector2 PointTo(this Vector2 val, Vector2 point)
    {
        val = point - val;

        return val.normalized;
    }

    public static Vector3 PointTo(this Vector3 val, Vector3 point)
    {
        val = point - val;

        return val.normalized;
    }

    public static Vector2 Direction(this Vector2 val, Vector2 point)
    {
        float x = val.x > point.x ? -1 : 1;
        float y = val.y > point.y ? -1 : 1;

        return new Vector2(x, y);
    }

    public static Vector3 Direction(this Vector3 val, Vector3 point)
    {
        float x = val.x > point.x ? -1 : 1;
        float y = val.y > point.y ? -1 : 1;
        float z = val.z > point.z ? -1 : 1;

        return new Vector3(x, y, z);
    }

    public static void NamedLog(this MonoBehaviour gO, string message)
    {
        Debug.Log(gO.name + ": " + message);
    }
}
