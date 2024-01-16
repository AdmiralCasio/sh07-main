using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    [SerializeField] string filename;
    Component text;

    // Start is called before the first frame update
    void Start()
    {
        // get locations from file
        List<Location> locations = FileHandler.ReadFromJSON<Location>(filename);
        LocationHandler.locations = locations;

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
        var textComp = text.GetComponent<Text>();
        if (LocationValidator.AtLocation(location, curr) && !LocationValidator.LookingAtLocation(location, curr))
        {
            textComp.enabled = true;
            textComp.text = curr.name;
        }

        if (LocationValidator.LookingAtLocation(location, curr))
        {
            textComp.enabled = true;
            textComp.text = curr.information;
        }

        if (!LocationValidator.AtLocation(location, curr))
        {
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
