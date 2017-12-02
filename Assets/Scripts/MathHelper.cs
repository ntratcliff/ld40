using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static float Interpolate(float from, float to, float t)
    {
        return from + (to - from) * t;
    }

    public static Vector3 Interpolate(Vector3 from, Vector3 to, float t)
    {
        return from + (to - from) * t;
    }
}