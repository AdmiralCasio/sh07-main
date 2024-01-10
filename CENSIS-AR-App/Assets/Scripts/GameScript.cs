using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    [SerializeField] string filename;


    // Start is called before the first frame update
    void Start()
    {
        // get locations from file
        List<Location> locations = FileHandler.ReadFromJSON<Location>(filename);
        LocationHandler.locations = locations;
    }

    public void LocationFound()
    {
        // show info
        Debug.Log($"Location name: {LocationHandler.GetCurrLocation().name}, Building info: {LocationHandler.GetCurrLocation().information}");
        // show next button
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
