using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using UnityEngine;

namespace CENSIS.Runtime
{
    public class MapManager : MonoBehaviour
    {

        bool _tracking = false;
        Camera _mapCamera;
        AbstractMap _map;

        public void ReCentre()
        {

            var currentLocation = LocationProviderFactory
                    .Instance
                    .DefaultLocationProvider
                    .CurrentLocation;

            LocationProviderFactory.Instance.mapManager.UpdateMap(
                                    currentLocation.LatitudeLongitude
                                );
            _tracking = true;
        }
        
        void Update()
        {
            var viewportPoint = _mapCamera.ScreenToViewportPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0) && (viewportPoint.x is < 1 and > 0 && viewportPoint.y is < 1 and > 0))
            {
                _tracking = false;
            }
            
            if (_tracking)
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

        private void Start()
        {
            _mapCamera = GameObject.Find("MapCamera").GetComponent<Camera>();
            _map = LocationProviderFactory.Instance.mapManager;
        }
    }
}
