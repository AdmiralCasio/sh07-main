using System.Collections;
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using CENSIS.Locations;
using Mapbox.Utils;
using UnityEngine;

namespace CENSIS.Runtime
{
    public class MarkerHandler : MonoBehaviour
    {
        [SerializeField]
        AbstractMap _map;

        [SerializeField]
        float _spawnScale = 100f;

        [SerializeField]
        GameObject _markerPrefab;

        List<GameObject> _spawnedObjects { get; set; }
        List<Vector2d> vectorLocations;
        
        IEnumerator getPrevLocations()
        {
            yield return new WaitForEndOfFrame();
            Debug.Log("[MARKER] location index : " + LocationHandler.LocationIndex);
            for (int i=0; i < LocationHandler.LocationIndex; i++)
            {
                print("[MARKER] adding location ... :");
                AddMarker(LocationHandler.locations[i]);
                print("[MARKER] added location :" + LocationHandler.locations[i]);
            }
        }

        void Start()
        {
            _spawnedObjects = new List<GameObject>();
            vectorLocations = new List<Vector2d>();
            StartCoroutine(getPrevLocations());
        }

        public void AddMarker(Location location)
        {
            if (_spawnedObjects.Find(obj => obj.name == location.name) == null)
            {
                Vector2d convertedLocation = new Vector2d(location.centre.x, location.centre.y);
                vectorLocations.Add(convertedLocation);
                var instance = Instantiate(_markerPrefab);
                instance.transform.localPosition = _map.GeoToWorldPosition(convertedLocation, true);
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                instance.name = location.name;
                instance.GetComponent<LabelTextSetter>().Set(new Dictionary<string, object> { { "name", location.name } });
                _spawnedObjects.Add(instance);
            }
        }

        void Update()
        {
            int count = _spawnedObjects.Count;
            for (int i = 0; i < count; i++)
            {
                var spawnedObject = _spawnedObjects[i];
                var location = vectorLocations[i];
                spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
                spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            }
        }
    }
}
