using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.XR.ARFoundation;

public class GameScript : MonoBehaviour
{
    [SerializeField]
    string filename;
    [SerializeField]
    TextMeshProUGUI clueText;
    Component text;
    //Transform buildingTransform;
    public GameObject BuildingText;
    [SerializeField] public GameObject[] textItems;
    TMP_Text title;
    TMP_Text info;

    [SerializeField]
    GameObject[] debugText;
    

    ARCameraManager arCameraManager;

    Canvas clueOverlay;
    Canvas nextButton;
    Canvas showClue;


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
        textItems[0].SetActive(false); textItems[1].SetActive(false);

        arCameraManager = FindAnyObjectByType<ARCameraManager>();

        // get locations from file
        List<Location> locations = FileHandler.ReadFromJSON<Location>(filename);
        LocationHandler.locations = locations;


        // string data = "";
        // foreach (Location location in locations)
        // {
        //     data += JsonConvert.SerializeObject(location, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        // }
        // using (StreamWriter sw = new StreamWriter("C:\\Users\\ahmed\\sh07-main\\CENSIS-AR-App\\Assets\\Resources\\Test.json"))
        // {
        //     sw.Write(data);
        // }

        var go = new GameObject();
        go.transform.parent = Camera.main.transform;
        text = go.AddComponent<Text>();

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

    void Update()
    {
        var location = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
        Camera.main.transform.position = BoundaryBoxes.ConvertToUnityCartesian(location);
        var curr = LocationHandler.GetCurrLocation();

        Vector3 overlayLocation = new Vector3(BoundaryBoxes.ConvertToUnityCartesian(curr.centre).x,
                                                       Camera.main.transform.position.y,
                                                       BoundaryBoxes.ConvertToUnityCartesian(curr.centre).z);

        BuildingText.transform.position = overlayLocation;
        
        if (LocationValidator.AtLocation(location, curr) && !LocationValidator.LookingAtLocation(location, curr))
        {
            Debug.Log($"Game script: At {curr.name}");

            // enable text items
            title.enabled = true; info.enabled = true;
            textItems[0].SetActive(true); textItems[1].SetActive(true);

            // set text items to correct values
            title.text = curr.name;
            info.text = curr.information;

            // on screen debug
            debugText[0].GetComponent<TMP_Text>().text = "At Location: true";
            debugText[1].GetComponent<TMP_Text>().text = "Looking at Location : false";
            debugText[2].GetComponent<TMP_Text>().text = "Overlay: " + info.enabled;
        }

        if (LocationValidator.LookingAtLocation(location, curr))
        {
            Debug.Log($"Game script: Looking At {curr.name}");

            // enable text items
            title.enabled = true; info.enabled = true;
            textItems[0].SetActive(true); textItems[1].SetActive(true);

            // set text items to correct values
            title.text = curr.name;
            info.text = curr.information;

            // on screen debug 
            debugText[0].GetComponent<TMP_Text>().text = "At Location: true";
            debugText[1].GetComponent<TMP_Text>().text = "Looking at Location : true";
            debugText[2].GetComponent<TMP_Text>().text = "Overlay: " + info.enabled;

            //LocationFound();
        }

        if (!LocationValidator.AtLocation(location, curr))
        {
            Debug.Log($"Game Script: Not at {curr.name}");

            // disable text items
            title.enabled = false; info.enabled = false;
            textItems[0].SetActive(false); textItems[1].SetActive(false);

            // on screen debug
            debugText[0].GetComponent<TMP_Text>().text = "At Location: false";
            debugText[1].GetComponent<TMP_Text>().text = "Looking at Location : false";
            debugText[2].GetComponent<TMP_Text>().text = "Overlay: " + info.enabled;
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
            Debug.Log($"Locations clue: {LocationHandler.GetCurrLocation().clue}");
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
