using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationHandler : MonoBehaviour
{
    static int locationIndex;
    public static List<Location> locations { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        // gets the index of the current building or 0 
        int locationIndex = PlayerPrefs.GetInt("CurrentBuilding", 0);
    }
    public static bool NextLocation()
    {
        locationIndex += 1;
        PlayerPrefs.SetInt("Current location", locationIndex);
        PlayerPrefs.Save();
        return true;
    }

    public static bool IsFinalLocation()
    { return locationIndex == locations.Count-1; }

    public static Location GetCurrLocation()
    {
        return locations[locationIndex];
    }
}
