using System.Net.Security;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine;

namespace CENSIS.Runtime
{
    public static class Player
    {
        /// <returns>The current GPS location of the user's device</returns>
        public static Vector2 GetUserLocation()
        {
            return Vector2dToVector2(
                LocationProviderFactory
                    .Instance
                    .DefaultLocationProvider
                    .CurrentLocation
                    .LatitudeLongitude
            );
        }

        /// <returns>The current compass heading of the user's device</returns>
        public static float GetUserDirection()
        {
            return LocationProviderFactory
                .Instance
                .DefaultLocationProvider
                .CurrentLocation
                .UserHeading;
        }

        private static Vector2 Vector2dToVector2(Vector2d vector2D)
        {
            return new Vector2((float)vector2D.x, (float)vector2D.y);
        }
    }
}
