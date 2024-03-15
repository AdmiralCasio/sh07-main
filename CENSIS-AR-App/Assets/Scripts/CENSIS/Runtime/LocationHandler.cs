using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CENSIS.Locations;
using UnityEngine;

namespace CENSIS.Runtime
{
    [Serializable]
    public class PlayerData
    {
        public int locationIndex { get; set; }
    }

    public class LocationHandler : MonoBehaviour
    {
        private static PlayerData playerData;
        private static BinaryFormatter binaryFormatter;
        private static string saveFilePath;

        public static int LocationIndex { get; set; }
        public static List<Location> locations { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            // gets the index of the current building or 0
            playerData = new PlayerData();
            binaryFormatter = new BinaryFormatter();
            saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.dat");

            if (File.Exists(saveFilePath))
            {
                FileStream file = File.Open(saveFilePath, FileMode.Open);
                playerData = (PlayerData)binaryFormatter.Deserialize(file);
                file.Close();
                LocationIndex = playerData.locationIndex;
                Debug.Log(
                    "FileLoad: Load complete, current location index " + playerData.locationIndex
                );
            }
            else
            {
                LocationIndex = 0;
                playerData.locationIndex = LocationIndex;
                Debug.Log("FileLoad: No save files to load");
            }
        }

        public static void NextLocation()
        {
            LocationIndex += 1;
            playerData.locationIndex = LocationIndex;
            FileStream file = File.Create(saveFilePath);
            binaryFormatter.Serialize(file, playerData);
            file.Close();
        }

        public static void Restart()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "PlayerData.dat");
            // check if file exists
            if (!File.Exists(filePath))
            {
                Debug.Log("no file exists");
            }
            else
            {
                Debug.Log("file exists, deleting...");

                File.Delete(filePath);

                RefreshEditorProjectWindow();
            }
            LocationIndex = 0;
            playerData.locationIndex = LocationIndex;
        }

        public static bool IsFinalLocation()
        {
            return LocationIndex == locations.Count - 1;
        }

        public static Location GetCurrLocation()
        {
            return locations[LocationIndex];
        }
        
        static void RefreshEditorProjectWindow()
        {
        #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
        #endif
        }
    }
}
