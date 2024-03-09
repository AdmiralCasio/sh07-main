using System;
using System.Collections;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.Android;

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

    public static bool CheckUserLocation()
    {
        var currLocation = LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation;
        Debug.Log("GPS: time diff:"+ (ConvertToUnixTimestamp(DateTime.Now)-currLocation.Timestamp));
        Debug.Log("GPS: curloctime:"+ currLocation.Timestamp + "time:" + ConvertToUnixTimestamp(DateTime.Now));
        if (
            (currLocation.IsLocationServiceEnabled
            || currLocation.IsLocationServiceInitializing)
            && ConvertToUnixTimestamp(DateTime.Now) - currLocation.Timestamp < 10
        )
            return true;
        else
            return false;
        
    }
    public static double ConvertToUnixTimestamp(DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return diff.TotalSeconds;
    }

    // public static bool CheckUserLocation()
    // {
    //     var currLocation = LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation;
    //     if (currLocation.IsLocationServiceInitializing)
    //     {
    //         Debug.Log("GPS: Location service is initialising");
    //     }
    //
    //     if (currLocation.IsLocationServiceEnabled)
    //     {
    //         Debug.Log("GPS: Location service is enabled");
    //     }
    //     else
    //     {
    //         Debug.Log("GPS:Location service not enabled");
    //     }
    //
    //     if (currLocation.IsLocationUpdated)
    //     {
    //         Debug.Log("GPS: Location service updated");
    //     }
    //     Debug.Log("GPS: time diff:"+ (ConvertToUnixTimestamp(DateTime.Now)-currLocation.Timestamp));
    //     Debug.Log("GPS: curloctime:"+ currLocation.Timestamp + "time:" + ConvertToUnixTimestamp(DateTime.Now));
    //     return true;
    // }

    
    public static float GetUserDirection()
    {
        return LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.UserHeading;
    }

    private static Vector2 Vector2dToVector2(Vector2d vector2D)
    {
        return new Vector2((float)vector2D.x, (float)vector2D.y);
    }
}
