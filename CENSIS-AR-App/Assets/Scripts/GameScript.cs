using UnityEngine;
using UnityEngine.Android;
using TMPro;

public class GameScript : MonoBehaviour
{
    [SerializeField] 
    string filename;
    [SerializeField]
    TextMeshProUGUI clueText;

    Canvas clueOverlay;
    Canvas nextButton;
    Canvas showClue;
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Script Start");
        Permission.RequestUserPermission(Permission.FineLocation);
        Input.location.Start();
        Input.compass.enabled = true;

        // get locations from file
        LocationHandler.locations = FileHandler.ReadFromJSON<Location>(filename);


        clueOverlay = GameObject.Find("ClueOverlay").GetComponent<Canvas>();
        nextButton = GameObject.Find("Next").GetComponent<Canvas>();
        showClue = GameObject.Find("ShowClue").GetComponent<Canvas>();
        clueOverlay.enabled = false;
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

    void Update()
    {
        var location = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
        var curr = LocationHandler.GetCurrLocation();
        if (LocationValidator.AtLocation(location, curr) && !LocationValidator.LookingAtLocation(location, curr))
        {
            Debug.Log($"Game script: At {curr.name}");
        }

        if (LocationValidator.LookingAtLocation(location, curr))
        {
            Debug.Log($"Game script: Looking At {curr.name}");
            LocationFound();
        }

        if (!LocationValidator.AtLocation(location, curr))
        {
            Debug.Log($"Game Script: Not at {curr.name}");
        }
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
            nextButton.enabled = false;
            showClue.enabled = true;
        }
    }

    private void ShowClue()
    {
        // show clue;
        clueText.text = LocationHandler.GetCurrLocation().clue;
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
