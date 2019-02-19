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

    public static Vector3 PointTo(this Transform val, Transform point)
    {
        Vector3 dir = point.position - val.position;

        return dir.normalized;
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

    public static Vector2 XY(this Vector3 val)
    {
        return new Vector2(val.x, val.y);   
    }

    public static void NamedLog(this MonoBehaviour gO, string message)
    {
        Debug.Log(gO.name + ": " + message);
    }

    public static bool WithinRange<T>(this List<T> list, float index)
    {
        return list.WithinRange((int)index);
    }

    public static bool WithinRange<T>(this List<T> list, int index)
    {
        if (index < list.Count && index >= 0)
        {
            return true;
        }
        else
        {
            //Debug.Log(index + " Is not within Lists max of : " + list.Count);
            return false;
        }
    }

    public static Vector2 ToVector2(this Direction dir)
    {
        if(dir == global::Direction.Left)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.right;
        }
    }

    public static Color SetAlpha(this Color col, float alpha)
    {
        return new Color(col.r, col.g, col.b, alpha);
    }

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}
