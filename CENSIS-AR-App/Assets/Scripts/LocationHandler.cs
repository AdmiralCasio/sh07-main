using System.Collections.Generic;
using UnityEngine;

public class LocationHandler : MonoBehaviour
{
    static int locationIndex;
    public static int LocationIndex
    {
        get { return locationIndex; }
        set { locationIndex = value; }
    }
    public static List<Location> locations { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        // gets the index of the current building or 0 
        int locationIndex = PlayerPrefs.GetInt("CurrentLocation", 0);
    }
    public static void NextLocation()
    {
        locationIndex += 1;
        PlayerPrefs.SetInt("CurrentLocation", locationIndex);
        PlayerPrefs.Save(); 
    }

    public static bool IsFinalLocation()
    { return locationIndex == locations.Count-1; }

    public static Location GetCurrLocation()
    {
        return locations[locationIndex];
    }
}
