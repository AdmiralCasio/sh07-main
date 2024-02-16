using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class Player : MonoBehaviour
{ 
    private void Awake()
    {
        Permission.RequestUserPermission(Permission.FineLocation);
        Input.location.Start();
        Input.compass.enabled = true;

        VerifyLocation();
    }

    IEnumerator VerifyLocation()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location not enabled on decies or app doesn  not have permission to access location");
            yield break;
        }
    }

    static public Vector2 getUserLocation()
    {
        return new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
    }
    static public float getUserDirection()
    {
        return Input.compass.trueHeading;
    }

}
