using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    [SerializeField] string filename;
    Canvas clueOverlay;
    Canvas nextButton;
    Canvas showClue;



    // Start is called before the first frame update
    void Start()
    {
        // get locations from file
        List<Location> locations = FileHandler.ReadFromJSON<Location>(filename);
        LocationHandler.locations = locations;
        clueOverlay = GameObject.Find("ClueOverlay").GetComponent<Canvas>();
        nextButton = GameObject.Find("Next").GetComponent<Canvas>();
        showClue = GameObject.Find("ShowClue").GetComponent<Canvas>();
        clueOverlay.enabled = false;
        nextButton.enabled = false;
        Debug.Log($"clueOverlay.enabled {clueOverlay.enabled}");
        Debug.Log($"nextButton.enabled {nextButton.enabled}");

    }

    public void LocationFound()
    {
        // show info
        Debug.Log($"Location name: {LocationHandler.GetCurrLocation().name}, Building info: {LocationHandler.GetCurrLocation().information}");
        // show next button
        nextButton.enabled = true;
        showClue.enabled = false;


    }

    public void Next()
    {
        if (LocationHandler.IsFinalLocation())  {
            GameWon();
        }
        else
        {
            // switch to next location
            LocationHandler.NextLocation();
            // show clue
            ShowClue();
            Debug.Log($"Locations clue: {LocationHandler.GetCurrLocation().clue}");
            nextButton.enabled = false;
            showClue.enabled = true;
        }
    }

    public void ShowClue()
    {
        // show clue;
        clueOverlay.enabled = true;
    }

    public void CloseClue()
    {
        // close clue
        clueOverlay.enabled = false;
        Debug.Log($"clueOverlay.enabled {clueOverlay.enabled}");
    }

    void GameWon()
    {
        // display congradulations
        Debug.Log("Game finished, well done");
    }

}
