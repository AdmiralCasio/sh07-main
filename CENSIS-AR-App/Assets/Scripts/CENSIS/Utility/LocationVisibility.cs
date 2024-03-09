using UnityEngine;

namespace CENSIS.Utility
{
    public static class LocationVisibility
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
}
