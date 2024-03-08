using System.Collections;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.Android;

namespace CENSIS.Runtime
{
    public class Player : MonoBehaviour
    {
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

        public static float GetUserDirection()
        {
            return LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.UserHeading;
        }

        private static Vector2 Vector2dToVector2(Vector2d vector2D)
        {
            return new Vector2((float)vector2D.x, (float)vector2D.y);
        }
    }
}

