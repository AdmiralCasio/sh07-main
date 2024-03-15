using UnityEngine;
using System.Collections.Generic;

namespace CENSIS.Utility
{ 
/**
* <summary>
*  Contains utility methods to determine a Location's visibility in viewport space.
* </summary>
**/
    public static class LocationVisibility
    {
        /**
        * <summary>
        *  Determines whether a Unity world space point <c>target</c> is within the view of the <c>camera</c>.
        * </summary>
        * <param name="target">the point to be checked</param>
        * <param name="camera">the camera which is being tested</param>
        * <returns>A boolean representing whether or not the point <c>target</c> is visible within the frame of <c>camera</c></returns>
        **/
        public static bool IsVisible(Vector3 target, Camera camera)
        {
            Vector3 screenPoint = camera.WorldToViewportPoint(target);
            return screenPoint.x is > 0 and < 1
                   && screenPoint.y is > 0 and < 1 
                   && screenPoint.z > 0;
        }

        public static IEnumerable<int> GetColour(Vector3 target, Camera camera)
        {
            Vector3 screenPoint = camera.WorldToViewportPoint(target);
            int[] array;
            if (screenPoint.z > 0)
            {
                 array = screenPoint.x switch
                {
                    >= 0 and < 1 => new[] { 0 },
                    < 0 and >= -1 => new[] { 7, 8 },
                    >= 1 and < 2 => new[] { 3, 4 },
                    < -1 => new[] { 5, 6 },
                    >= 2 => new[] { 1, 2 },
                    _ => new int[] { }
                };
            }
            else array = new int[] { }; 
            return array;

        }
    }

}
