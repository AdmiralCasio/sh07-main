using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using Unity.Plastic.Newtonsoft.Json;

public class GameScript : MonoBehaviour
{
    [SerializeField] string filename;
    Component text;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Script Start");
        Permission.RequestUserPermission(Permission.FineLocation);
        Input.location.Start();
        Input.compass.enabled = true;

        // get locations from file
        List<Location> locations = FileHandler.ReadFromJSON<Location>(filename);
        LocationHandler.locations = locations;
        Debug.Log("Game script: Locations: " + locations[0].clue);
        Debug.Log("Game script: Location count: " + locations.Count);
        string data = "";
        foreach (Location location in locations)
        {
            Debug.Log("game script: Location: " +  location.name);
            data += JsonConvert.SerializeObject(location, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });            
            Debug.Log("GAMESCRIPT DATA: " + data);

        }
        using (StreamWriter sw = new StreamWriter("C:\\Users\\ahmed\\sh07-main\\CENSIS-AR-App\\Assets\\Resources\\Test.json"))
        {
            sw.Write(data);
        }

        var go = new GameObject();
        go.transform.parent = Camera.main.transform;
        text = go.AddComponent<Text>();
    }

    public void LocationFound()
    {
        // show info
        Debug.Log($"Location name: {LocationHandler.GetCurrLocation().name}, Building info: {LocationHandler.GetCurrLocation().information}");
        // show next button
    }

    void Update()
    {
        // 2024/01/16 18:42:40.574 23681 23707 Error Unity ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.

        var location = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
        var curr = LocationHandler.GetCurrLocation();
        var textComp = text.GetComponent<Text>();
        if (LocationValidator.AtLocation(location, curr) && !LocationValidator.LookingAtLocation(location, curr))
        {
            Debug.Log($"Game script: At {curr.name}");
            textComp.enabled = true;
            textComp.text = curr.name;
        }

        if (LocationValidator.LookingAtLocation(location, curr))
        {
            Debug.Log($"Game script: Looking At {curr.name}");
            textComp.enabled = true;
            textComp.text = curr.information;
        }

        if (!LocationValidator.AtLocation(location, curr))
        {
            Debug.Log($"Game Script: Not at {curr.name}");
            textComp.enabled = false;
        }
    }

    private void Next()
    {
        // switch to next location
        if (LocationHandler.UpdateLocation())  {
            // show clue
            ShowClue();
            Debug.Log($"Locations clue: {LocationHandler.GetCurrLocation().clue}");
        }
        else
        {
            GameWon();
        }
    }

    private void ShowClue()
    {
        // show clue;
    }

    void GameWon()
    {
        // display congradulations
        Debug.Log("Game finished, well done");
    }

}
