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

/**
 * <summary>
 *  Keeps track of and provides methods to manipulate locations.
 * </summary> 
**/
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

    void Start()
    {
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

    /// <summary>
    ///     Advances the current location to the next in the list.
    /// </summary>
    public static void NextLocation()
    {
        locationIndex += 1;
        playerData.locationIndex = locationIndex;
        FileStream file = File.Create(saveFilePath);
        binaryFormatter.Serialize(file, playerData);
        file.Close();
    }

    /// <summary>
    ///  Has the final location in the list been reached.
    /// </summary>
    /// <returns>A boolean representing whether the final location in the list has been reached.</returns>
    public static bool IsFinalLocation()
    { return locationIndex == locations.Count-1; }

    /// <summary>
    /// Gets the current location in the list of locations.
    /// </summary>
    /// <returns>The current location in the scavenger hunt.</returns>
    public static Location GetCurrLocation()
    {
        return locations[locationIndex];
    }
}
