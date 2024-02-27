using Mapbox.Unity.Location;
using Mapbox.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class Player : MonoBehaviour
{
    /// <returns>The current GPS location of the user's device</returns>
    static public Vector2 GetUserLocation()
    {
        return Vector2dToVector2(LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.LatitudeLongitude);
    }

    /// <returns>The current compass heading of the user's device</returns>
    static public float GetUserDirection()
    {
        return LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.UserHeading;
    }

    static private Vector2 Vector2dToVector2(Vector2d vector2D)
    {
        return new Vector2((float)vector2D.x, (float)vector2D.y);
    }


}
