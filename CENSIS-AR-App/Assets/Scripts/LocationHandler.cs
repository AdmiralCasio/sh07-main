using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;
using System.IO;


[Serializable]
public class PlayerData
{
    public int locationIndex;
}

public class LocationHandler : MonoBehaviour
{
    private static PlayerData playerData;
    private static BinaryFormatter binaryFormatter;
    private static string saveFilePath;
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
        //int locationIndex = PlayerPrefs.GetInt("CurrentLocation", 0);
        playerData = new PlayerData();
        binaryFormatter = new BinaryFormatter();
        saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.dat");

        if (File.Exists(saveFilePath))
        {
            FileStream file = File.Open(saveFilePath, FileMode.Open);
            playerData = (PlayerData)binaryFormatter.Deserialize(file);
            file.Close();
            locationIndex = playerData.locationIndex;
            Debug.Log("Load complete, current location index " + playerData.locationIndex);
        }
        else
        {

            locationIndex = 0;
            playerData.locationIndex = locationIndex;
            Debug.Log("No save files to load");

        }
    }
    public static void NextLocation()
    {
        locationIndex += 1;
        //PlayerPrefs.SetInt("CurrentLocation", locationIndex);
        //PlayerPrefs.Save(); 
        playerData.locationIndex = locationIndex;
        FileStream file = File.Create(saveFilePath);
        binaryFormatter.Serialize(file, playerData);
        file.Close();
    }

    public static bool IsFinalLocation()
    { return locationIndex == locations.Count-1; }

    public static Location GetCurrLocation()
    {
        return locations[locationIndex];
    }
}
