using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    [SerializeField] string filename;
    Canvas clue = GameObject.Find("ClueOverlay").GetComponent<Canvas>();
    Button nextButton = GameObject.Find("NextButton").GetComponent<Button>();
    Button showClue = GameObject.Find("ShowClue").GetComponent<Button>();



    // Start is called before the first frame update
    void Start()
    {
        // get locations from file
        List<Location> locations = FileHandler.ReadFromJSON<Location>(filename);
        LocationHandler.locations = locations;
        clue.enabled = false;
        nextButton.enabled = false;
        
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
        // switch to next location
        if (LocationHandler.UpdateLocation())  {
            // show clue
            ShowClue();
            Debug.Log($"Locations clue: {LocationHandler.GetCurrLocation().clue}");
            nextButton.enabled = false;
            showClue.enabled = true;
        }
        else
        {
            GameWon();
        }
    }

    public void ShowClue()
    {
        // show clue;
        clue.enabled = true;
    }

    public void CloseClue()
    {
        // close clue
        clue.enabled = false;
    }

    void GameWon()
    {
        // display congradulations
        Debug.Log("Game finished, well done");
    }

}
