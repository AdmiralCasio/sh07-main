using UnityEngine;
using CENSIS.Locations;

namespace CENSIS.Utility
{
    public class LocationValidator
    {
        // Start is called before the first frame update
        void Start() { }
    
        // Update is called once per frame
        void Update() { }
    
        public static bool AtLocation(Vector2 position, Location location, Vector3 origin)
        {
            bool inInner = false;
            bool inOuter = false;
    
            foreach (var box in location.outer)
            {
                int boxNumber = 0;
                if (InBox(position, location.outer[boxNumber].points,origin))
                {
                    inOuter = true;
                }
            }
    
            foreach (var box in location.inner)
            {
                int boxNumber = 0;
                if (InBox(position, location.inner[boxNumber].points,origin))
                {
                    inInner = true;
                }
            }
    
            return inOuter && !inInner;
        }
    
        public static bool LookingAtLocation(Vector2 position, Location location, Vector3 origin)
        {
            return AtLocation(position, location,origin)
                && LocationVisibility.IsVisible(
                    BoundaryBoxes.ConvertToUnityCartesian(location.centre, origin),
                    Camera.main
                );
        }
    
        public static bool InBox(Vector2 position, Vector2[] box, Vector3 origin)
        {
            return BoundaryBoxes.IsPointInPolygonGPS(position, box,origin);
        }
    }

}
