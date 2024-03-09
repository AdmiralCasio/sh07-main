using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationVisibility

{
    public static bool IsVisible(Vector3 target, Camera camera)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(target);
        return screenPoint.x is > 0 and < 1 && screenPoint.y is > 0 and < 1 && screenPoint.z > 0;
    }

    public static int[] GetColour(Vector3 target, Camera camera)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(target);
        int [] array = screenPoint.x switch
        {
            > 0 and < 1 => new[] { 0 },
            < 0 and > -1 => new[] { 7, 8 },
            > 1 and < 2 => new [] { 3, 4 }, 
            <-2 => new [] { 5, 6 },
            _ => new [] { 1, 2 }
        };
        return array;
    }
}
