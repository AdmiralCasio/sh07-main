using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Location;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        var currentLocation = LocationProviderFactory
            .Instance
            .DefaultLocationProvider
            .CurrentLocation;

        if (currentLocation.IsLocationUpdated)
        {
            LocationProviderFactory.Instance.mapManager.UpdateMap(
                currentLocation.LatitudeLongitude
            );
        }
    }
}
