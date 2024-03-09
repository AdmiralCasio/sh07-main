using Mapbox.Unity.Location;
using UnityEngine;

namespace CENSIS.Runtime
{
    public class MapManager : MonoBehaviour
    {
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
}
