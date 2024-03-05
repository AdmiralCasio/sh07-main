using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationVisibility
{
    public static bool IsVisible(Vector3 target, Camera camera)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(target);
        return screenPoint.x > 0
            && screenPoint.x < 1
            && screenPoint.y > 0
            && screenPoint.y < 1
            && screenPoint.z > 0;
    }
}
