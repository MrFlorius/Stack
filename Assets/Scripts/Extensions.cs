using System;
using UnityEngine;

public static class Extensions
{
    public static Bounds GetXZ(this Bounds aBounds)
    {
        var ext = aBounds.extents;
        ext.y = float.PositiveInfinity;
        aBounds.extents = ext;
        return aBounds;
    }

    public static Vector3 SnapToGrid(this Vector3 aVector3, float grid)
    {
        var v = aVector3;
        for (int i = 0; i < 3; i++)
            if(Math.Abs(grid) > float.Epsilon)
                v[i] = Mathf.Round(v[i] / grid) * grid;
        
        return v;
    }

    public static Vector3 SnapToGrid(this Vector3 aVector3, Vector3 grid)
    {
        var v = aVector3;
        for (int i = 0; i < 3; i++)
            if(Math.Abs(grid[i]) > float.Epsilon)
                v[i] = Mathf.Round(v[i] / grid[i]) * grid[i];
        
        return v;
    }

    public static bool All(this Vector3 aVector3)
    {
        for(int i = 0; i < 3; i++)
            if (Mathf.Abs((aVector3)[i]) < float.Epsilon)
                return false;

        return true;
    }
}