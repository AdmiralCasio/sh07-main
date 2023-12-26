using UnityEngine;
using System.Collections;
using UnityEngine.Android;

public class TestLocationService : MonoBehaviour
{

    private void Awake()
    {
        Permission.RequestUserPermission(Permission.FineLocation);
        Input.location.Start();
        Input.compass.enabled = true;

    }
    private void Update()
    {
        Start();
    }
    IEnumerator Start()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location not enabled on device or app does not have permission to access location");
            yield break;
        }

        // Starts the location service.

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        while (Input.location.status == LocationServiceStatus.Running)
        {   
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            Debug.Log("Location: " + Input.location.lastData.latitude + "," + Input.location.lastData.longitude + "," + Input.location.lastData.altitude + "  Accuracy: " + Input.location.lastData.horizontalAccuracy + "  Timestamp: " + Input.location.lastData.timestamp);
            Debug.Log("Facing: " + Input.compass.trueHeading);
            yield return new WaitForSeconds(1);
        }
    }
}