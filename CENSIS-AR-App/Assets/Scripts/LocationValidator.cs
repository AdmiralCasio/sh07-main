using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationValidator
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool AtLocation(Vector2 position,  Location location)
    {
        return InBox(position, location.outer[0].points) && !InBox(position, location.inner[0].points);
    }

    public static bool LookingAtLocation(Vector2 position, Location location)
    {
        return AtLocation(position, location) && LocationVisibility.IsVisible(location.centre, Camera.main);
    }

    static bool InBox(Vector2 position, Vector2[] box)
    {
        return BoundaryBoxes.IsPointInPolygonGPS(position, box);
    }
}
