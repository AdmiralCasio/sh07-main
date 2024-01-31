using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using TMPro;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;

public class GameScript : MonoBehaviour
{
    [SerializeField] string filename;
    Component text;
    public GameObject BuildingText;
    [SerializeField] public GameObject[] textItems;
    public TMP_Text title;
    public TMP_Text info;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Script Start");

        // Get user permissions and start location tracking
        Permission.RequestUserPermission(Permission.FineLocation);
        Input.location.Start();
        Input.compass.enabled = true;

        // Define text mesh pro components
        title = textItems[0].GetComponent<TMP_Text>();
        info = textItems[1].GetComponent<TMP_Text>();
        title.enabled = false; info.enabled = false;

        // get locations from file
        List<Location> locations = FileHandler.ReadFromJSON<Location>(filename);
        LocationHandler.locations = locations;


        string data = "";
        foreach (Location location in locations)
        {
            data += JsonConvert.SerializeObject(location, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
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
        var location = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
        var curr = LocationHandler.GetCurrLocation();
        if (LocationValidator.AtLocation(location, curr) && !LocationValidator.LookingAtLocation(location, curr))
        {
            Debug.Log($"Game script: At {curr.name}");
          
            info.text = curr.information;
        }

        if (LocationValidator.LookingAtLocation(location, curr))
        {
            Debug.Log($"Game script: Looking At {curr.name}");
            title.enabled = true; info.enabled = true;
            title.text = curr.name;
            info.text = curr.information;

        }

        if (!LocationValidator.AtLocation(location, curr))
        {
            Debug.Log($"Game Script: Not at {curr.name}");
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
