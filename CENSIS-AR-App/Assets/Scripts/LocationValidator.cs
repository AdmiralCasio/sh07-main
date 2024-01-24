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
        bool inInner = false;
        bool inOuter = false;

        foreach (var box in location.outer) {
            int boxNumber = 0;
            if (InBox(position, location.outer[boxNumber].points)){
                inOuter = true;
            }
        }

        foreach (var box in location.inner)
        {
            int boxNumber = 0;
            if (InBox(position, location.inner[boxNumber].points)){
                inInner = true;
            }
        }

        return inOuter && !inInner;
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
