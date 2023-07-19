using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static Vector3 toUnityCoord(Vector3 vec)
    {
        return new Vector3(-vec.x, vec.y, vec.z);
    }

    public static Quaternion toUnityAngle(Vector3 vec)
    {
        Quaternion angle = Quaternion.Euler(vec * Mathf.Rad2Deg);
        return new Quaternion(
             angle.x,
            -angle.y,
            -angle.z,
             angle.w
        );
    }
}
