using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationVisibility

{
    public static bool IsVisible(Vector3 target, Camera camera)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(target);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1 && screenPoint.z > 0;
    }

    public static int[] GetColour(Vector3 target, Camera camera)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(target);
        int[] array; // Declare array here for scope visibility
        if (screenPoint.x > 0 && screenPoint.x < 1)
        {
            array = new int[] { 0 }; // Corrected array initialization
        }
        else if (screenPoint.x < 0 && screenPoint.x > -1)
        {
            array = new int[] {7, 8}; // Amber left arrows
        }
        else if (screenPoint.x > 1 && screenPoint.x < 2)
        {
            array = new int[] {3, 4}; // Amber right arrows
        }
        else if (screenPoint.x < -2)
        {
            array = new int[] {5, 6}; // Red left arrows
        }
        else
        {
            array = new int[] {1, 2}; // Red right arrows
        }
        return array;
    }
}
