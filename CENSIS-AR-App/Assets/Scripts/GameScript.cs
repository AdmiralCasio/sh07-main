using System;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using System.Resources;
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using UnityEngine;
using UnityEngine.Android;
using TMPro;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEditor;
using UnityEngine.XR.ARFoundation;
using Component = UnityEngine.Component;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class GameScript : MonoBehaviour
{
    [SerializeField]
    string filename;
    
    [SerializeField]
    TextMeshProUGUI clueText;
    Component text;
    public GameObject BuildingText;
    public TMP_Text title;
    public TMP_Text info;
    
    Vector3 origin;
    Vector2 originPreConvert;

    [SerializeField]
    GameObject[] debugText;

    Canvas clueOverlay;
    Canvas locationFoundOverlay;
    Canvas gameCompleteOverlay;
    Canvas nextButton;
    Canvas showClue;
    private Camera cam;
    
    //string coordinates;
    
    void getOrigin()
    {
        originPreConvert = Player.GetUserLocation();
        origin = BoundaryBoxes.ConvertToUnityCartesian(originPreConvert) - cam.transform.position;
        Debug.Log("ORIGIN : " + BoundaryBoxes.ConvertToCartesian(Player.GetUserLocation()));
        Debug.Log("ORIGIN : " + Vector2dToVector2(LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.LatitudeLongitude));
    }
     
    // Start is called before the first frame update
    void Start()
    {    
        cam = Camera.main;

        Debug.Log("Game Script Start");

        // Get user permissions and start location tracking
        Permission.RequestUserPermission(Permission.FineLocation);
        Input.compass.enabled = true;

        // define origin point
        // InvokeRepeating("getOrigin", 2,5);
        Invoke("getOrigin",1);
        // Define text mesh pro components
        
        BuildingText.gameObject.SetActive(false);
        title.gameObject.SetActive(false);  
        info.gameObject.SetActive(false);
        
        // BuildingText.gameObject.SetActive(true);
        // title.gameObject.SetActive(true);
        // info.gameObject.SetActive(true);

        // get locations from file
        LocationHandler.locations = FileHandler.ReadFromJSON<Location>(filename);

        clueOverlay = GameObject.Find("ClueOverlay").GetComponent<Canvas>();
        locationFoundOverlay = GameObject.Find("LocationFoundOverlay").GetComponent<Canvas>();
        gameCompleteOverlay = GameObject.Find("GameCompleteOverlay").GetComponent<Canvas>();
        nextButton = GameObject.Find("Next").GetComponent<Canvas>();
        showClue = GameObject.Find("ShowClue").GetComponent<Canvas>();
        clueOverlay.enabled = false;
        nextButton.enabled = false;
        locationFoundOverlay.enabled = false;
        gameCompleteOverlay.enabled = false;
        Debug.Log($"clueOverlay.enabled {clueOverlay.enabled}");
        Debug.Log($"nextButton.enabled {nextButton.enabled}");

        // Vector2[] outerconvert = BoundaryBoxes.ConvertToCartesian(LocationHandler.locations[2].outer[0].points);
        // foreach (Vector2 coord in outerconvert)
        // {
        //     Debug.Log(coord.ToString("F8"));
        //     coordinates += (coord.ToString("F8")) + "\n";
        // }
        // Debug.Log("CENTRE : " + BoundaryBoxes.ConvertToCartesian(LocationHandler.locations[2].centre));
        // Debug.Log("CENTRE (user): " + BoundaryBoxes.ConvertToCartesian(new Vector2(55.872146f, -4.289161f)));
        //
        // string path = "C:/Users/ahmed/sh07-main/CENSIS-AR-App/Assets/Resources/test2.txt";
        // using (StreamWriter writer = File.CreateText(path))
        // {
        //     writer.Write(coordinates);
        // }

    }

    public void LocationFound()
    {
        // show info
        Debug.Log($"Location name: {LocationHandler.GetCurrLocation().name}, Building info: {LocationHandler.GetCurrLocation().information}");
        // show next button
        nextButton.enabled = true;
        showClue.enabled = false;
    }

    private Vector2 Vector2dToVector2(Vector2d vector2D)
    {
        return new Vector2((float) vector2D.x, (float) vector2D.y);
    }

    // private Vector2 stringToVector2(string vectorString)
    // {
    //     string[] temp = vectorString.Split(",");
    //     Debug.Log("ORIGIN: " + temp[0]);
    //     float x = float.Parse(temp[0]);
    //     float y = float.Parse(temp[1]);
    //     Debug.Log("(ORIGIN) x:" + x);
    //     return new Vector2(x, y);
    // }

    void Update()
    {
        // define user, current building, and overlay locations
        var location = Player.GetUserLocation();  
        var curr = LocationHandler.GetCurrLocation();
        
        // calculate where the overlay should appear
        Vector3 normalisedCentre = BoundaryBoxes.ConvertToUnityCartesian(curr.centre, origin);
        Vector3 overlayLocation = normalisedCentre;

        String strOriginPreConvert = originPreConvert.ToString("N8");
        String strOriginConverted = origin.ToString("N8");
        
        debugText[2].GetComponent<TMP_Text>().text = "dist from location:" +
                                                     Math.Abs(cam.transform.position.x - overlayLocation.x) + " " + 
                                                     Math.Abs(cam.transform.position.y - overlayLocation.y) + " " +
                                                     Math.Abs(cam.transform.position.z - overlayLocation.z) + " " ;
        debugText[3].GetComponent<TMP_Text>().text = "overlay is at : "+ BoundaryBoxes.ConvertToUnityCartesian(curr.centre) + " | Normalised : " + overlayLocation;
        debugText[4].GetComponent<TMP_Text>().text = "Location accuracy : "+ LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.Accuracy;
        debugText[5].GetComponent<TMP_Text>().text = "origin is : " + strOriginPreConvert + "  |  Converted to : " + strOriginConverted; 
        
        // check if user is within location but not looking at the right direction
        if (LocationValidator.AtLocation(location, curr,origin) && !LocationValidator.LookingAtLocation(location, curr, origin))
        {
            // toggle game object states
            BuildingText.gameObject.SetActive(false);
            info.gameObject.SetActive(false);
            title.gameObject.SetActive(false);

            // set text items to correct values
            title.text = curr.name;
            info.text = curr.information;

            // on screen debug
            debugText[0].GetComponent<TMP_Text>().text = "At Location: true";
            debugText[1].GetComponent<TMP_Text>().text = "Looking at Location : false";
            Debug.Log($"Game script: At {curr.name}");
            locationFoundOverlay.enabled = true;

        }
        // check if user is both in and looking at location
        if (LocationValidator.LookingAtLocation(location, curr, origin))
        {
            // move overlay to be in front of camera
            if (Math.Abs(overlayLocation.y - cam.transform.position.y) >= 10 || overlayLocation.y - cam.transform.position.y < 0)
            {
                overlayLocation = new Vector3(overlayLocation.x, cam.transform.position.y, overlayLocation.z);
            }
            BuildingText.transform.position = overlayLocation;

            // toggle game object states
            BuildingText.gameObject.SetActive(true);
            info.gameObject.SetActive(true);
            title.gameObject.SetActive(true);
            
            // set text items to correct values
            title.text = curr.name;
            info.text = curr.information;

            // on screen debug
            debugText[0].GetComponent<TMP_Text>().text = "At Location: true";
            debugText[1].GetComponent<TMP_Text>().text = "Looking at Location : true";
            // Location Found
            Debug.Log($"Game script: Looking At {curr.name}");
            LocationFound();
            locationFoundOverlay.enabled = false;
        }
        // check if user is not in the location
        if (!LocationValidator.AtLocation(location, curr, origin))
        {
            // toggle game object states
            BuildingText.gameObject.SetActive(false);
            info.gameObject.SetActive(false);
            title.gameObject.SetActive(false);

            // on screen debug
            debugText[0].GetComponent<TMP_Text>().text = "At Location: false";
            debugText[1].GetComponent<TMP_Text>().text = "Looking at Location : false";

            Debug.Log($"Game Script: Not at {curr.name}");
            locationFoundOverlay.enabled = false;
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
        // display congratulations
        Debug.Log("Game finished, well done");
        gameCompleteOverlay.enabled = true;
    }

}
