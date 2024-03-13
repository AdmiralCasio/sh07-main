using System;
using System.IO;
using CENSIS.Utility;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using Component = UnityEngine.Component;
using Location = CENSIS.Locations.Location;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace CENSIS.Runtime
{
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
        public Vector3 origin { get; private set; }
        Vector2 originPreConvert;
        Vector3 defaultInfoTitleScale;
        Vector3 defaultBuildingTextScale;


        [SerializeField]
        GameObject[] debugText;

        Canvas clueOverlay;
        Canvas locationFoundOverlay;
        Canvas InsideLocationOverlay;
        Canvas gameCompleteOverlay;
        Canvas nextButton;
        Canvas showClue;
        Canvas startUpOverlay;

        private Camera cam;

        private void getOrigin()
        {
            originPreConvert = Player.GetUserLocation();
            origin =
                BoundaryBoxes.ConvertToUnityCartesian(originPreConvert) - cam.transform.position;
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
            InvokeRepeating("getOrigin", 2, 5);

            // Define text mesh pro components
            BuildingText.gameObject.SetActive(false);
            title.gameObject.SetActive(false);
            info.gameObject.SetActive(false);
            defaultInfoTitleScale = title.transform.localScale;
            defaultBuildingTextScale = BuildingText.transform.localScale;

            // get locations from file
            LocationHandler.locations = FileHandler.ReadFromJSON<Location>(filename);

            clueOverlay = GameObject.Find("ClueOverlay").GetComponent<Canvas>();
            locationFoundOverlay = GameObject.Find("LocationFoundOverlay").GetComponent<Canvas>();
            InsideLocationOverlay = GameObject.Find("InsideLocationOverlay").GetComponent<Canvas>();
            gameCompleteOverlay = GameObject.Find("GameCompleteOverlay").GetComponent<Canvas>();
            nextButton = GameObject.Find("Next").GetComponent<Canvas>();
            showClue = GameObject.Find("ShowClue").GetComponent<Canvas>();
            startUpOverlay = GameObject.Find("StartOverlay").GetComponent<Canvas>();
            // check if save file exists to check if user has opened the app before
            if (File.Exists(Path.Combine(Application.persistentDataPath, "PlayerData.dat")))
            {
                startUpOverlay.enabled = false;
            }
            clueOverlay.enabled = false;
            nextButton.enabled = false;
            locationFoundOverlay.enabled = false;
            gameCompleteOverlay.enabled = false;
            Debug.Log($"clueOverlay.enabled {clueOverlay.enabled}");
            Debug.Log($"nextButton.enabled {nextButton.enabled}");
        }

        public void LocationFound()
        {
            // show info
            Debug.Log(
                $"Location name: {LocationHandler.GetCurrLocation().name}, Building info: {LocationHandler.GetCurrLocation().information}"
            );
            // show next button
            nextButton.enabled = true;
            showClue.enabled = false;
        }

        private Vector2 Vector2dToVector2(Vector2d vector2D)
        {
            return new Vector2((float)vector2D.x, (float)vector2D.y);
        }

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
            float distanceFromOverlay = Vector3.Distance(cam.transform.position, overlayLocation);
            float scale = distanceFromOverlay / 5;
            
            debugText[2].GetComponent<TMP_Text>().text =
                "dist from location:"
                + Math.Abs(cam.transform.position.x - overlayLocation.x)
                + " "
                + Math.Abs(cam.transform.position.y - overlayLocation.y)
                + " "
                + Math.Abs(cam.transform.position.z - overlayLocation.z)
                + " ";
            debugText[3].GetComponent<TMP_Text>().text =
                "overlay is at : "
                + BoundaryBoxes.ConvertToUnityCartesian(curr.centre)
                + " | Normalised : "
                + overlayLocation;
            debugText[4].GetComponent<TMP_Text>().text =
                "Location accuracy : "
                + LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.Accuracy;
            debugText[5].GetComponent<TMP_Text>().text =
                "origin is : " + strOriginPreConvert + "  |  Converted to : " + strOriginConverted;

            // check if user is within location but not looking at the right direction
            if (
                LocationValidator.AtLocation(location, curr, origin)
                && !LocationValidator.LookingAtLocation(location, curr, origin)
            )
            {
                HideLocationInformation();

                // on screen debug
                debugText[0].GetComponent<TMP_Text>().text = "At Location: true";
                debugText[1].GetComponent<TMP_Text>().text = "Looking at Location : false";
                Debug.Log($"Game script: At {curr.name}");
                locationFoundOverlay.enabled = true;
                InsideLocationOverlay.enabled = false;
            }
            // check if user is both in and looking at location
            if (LocationValidator.LookingAtLocation(location, curr, origin))
            {
                if (!BuildingText.gameObject.activeSelf)
                {
                    title.transform.LookAt(BoundaryBoxes.ConvertToUnityCartesian(location,origin));
                    info.transform.LookAt(BoundaryBoxes.ConvertToUnityCartesian(location,origin));

                    title.transform.forward = -title.transform.forward;
                    info.transform.forward = -info.transform.forward;
                    ScaleText(new Vector3(scale,scale,1));
                }
                ShowLocationInformation(overlayLocation, curr);

                // on screen debug
                debugText[0].GetComponent<TMP_Text>().text = "At Location: true";
                debugText[1].GetComponent<TMP_Text>().text = "Looking at Location : true";
                // Location Found
                Debug.Log($"Game script: Looking At {curr.name}");
                LocationFound();
                locationFoundOverlay.enabled = false;
                InsideLocationOverlay.enabled = false;
            }
            // check if user is not in the location
            if (!LocationValidator.AtLocation(location, curr, origin))
            {
                // toggle game object states
                HideLocationInformation();

                // on screen debug
                debugText[0].GetComponent<TMP_Text>().text = "At Location: false";
                debugText[1].GetComponent<TMP_Text>().text = "Looking at Location : false";
                Debug.Log($"Game Script: Not at {curr.name}");
                locationFoundOverlay.enabled = false;
                InsideLocationOverlay.enabled = false;
            }

            foreach (var inner in curr.inner)
            {
                if (LocationValidator.InBox(location, inner.points, origin))
                {
                    InsideLocationOverlay.enabled = true;
                }
            }

            int locationCheckIndex = 0;
            foreach (Location loc in LocationHandler.locations)
            {
                if (locationCheckIndex == LocationHandler.LocationIndex)
                    break;
                if (LocationValidator.LookingAtLocation(Player.GetUserLocation(), loc, origin))
                {
                    var displayLocation = BoundaryBoxes.ConvertToUnityCartesian(loc.centre, origin);
                    ShowLocationInformation(displayLocation, loc);
                    break;
                }
                locationCheckIndex++;
            }
        }

        private void HideLocationInformation()
        {
            BuildingText.gameObject.SetActive(false);
            info.enabled = false;
            title.enabled = false;
            title.transform.localScale = defaultInfoTitleScale;
            info.transform.localScale = defaultInfoTitleScale;
            BuildingText.transform.localScale = defaultBuildingTextScale;
        }

        private void ShowLocationInformation(Vector3 overlayLocation, Location loc)
        {
            if (
                Math.Abs(overlayLocation.y - cam.transform.position.y) >= 10
                || overlayLocation.y - cam.transform.position.y < 0
            )
            {
                overlayLocation = new Vector3(
                    overlayLocation.x,
                    cam.transform.position.y,
                    overlayLocation.z
                );
            }
            
            BuildingText.transform.position = overlayLocation;

            // toggle game object states
            BuildingText.gameObject.SetActive(true);
            info.enabled = true;
            title.enabled = true;

            // set text items to correct values
            title.text = loc.name;
            info.text = loc.information;
        }

        public void Next()
        {
            if (LocationHandler.IsFinalLocation())
            {
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
        private void ScaleText(Vector3 scale)
        {
            BuildingText.transform.localScale.Scale(scale);
            info.gameObject.transform.localScale.Scale(scale);
            title.gameObject.transform.localScale.Scale(scale);
        }

        private void ShowClue()
        {
            clueText.text = LocationHandler.GetCurrLocation().clue;
            clueOverlay.enabled = true;
        }

        public void CloseClue()
        {
            clueOverlay.enabled = false;
            Debug.Log($"clueOverlay.enabled {clueOverlay.enabled}");
        }

        public void CloseStartUpPopUp()
        {
            startUpOverlay.enabled = false;
        }

        void GameWon()
        {
            // display congratulations
            Debug.Log("Game finished, well done");
            gameCompleteOverlay.enabled = true;
        }
    }
}
