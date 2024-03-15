using UnityEngine;

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
            return screenPoint.x > 0
                && screenPoint.x < 1
                && screenPoint.y > 0
                && screenPoint.y < 1
                && screenPoint.z > 0;
        }
    }
}
