using System;
using System.IO;
using CENSIS.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using Location = CENSIS.Locations.Location;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace CENSIS.Runtime
{
    public class GameScript : MonoBehaviour
    {
        #region SerializedFields

        [SerializeField]
        string filename;
        
        [SerializeField]
        TextMeshProUGUI solutionText;
        
        [SerializeField]
        TextMeshProUGUI clueText;
        
        [SerializeField] 
        MarkerHandler markerHandler;
        
        #endregion

        #region InfoOverlay

        public GameObject BuildingText;
        public TMP_Text title;
        public TMP_Text info;

        #endregion

        #region Vectors

        public Vector3 origin { get; private set; }
        Vector2 originPreConvert;
        Vector3 defaultInfoTitleScale;
        Vector3 defaultBuildingTextScale;

        #endregion

        #region Canvases

        [SerializeField]
        TMP_Text[] guideComponents;
        
        Canvas clueOverlay;
        Canvas locationFoundOverlay;
        Canvas InsideLocationOverlay;
        Canvas gameCompleteOverlay;
        Canvas nextButton;
        Canvas showClue;
        Canvas startUpOverlay;
        Canvas restartButton;
        Canvas confirmRestart;
        Canvas solutionOverlay;
        Canvas gameAid;
        Canvas locationUnavailableOverlay;

        #endregion
        
        private Camera cam;

        private void getOrigin()
        {
            originPreConvert = Player.GetUserLocation();
            origin =
                BoundaryBoxes.ConvertToUnityCartesian(originPreConvert) - cam.transform.position;
        }

        void Start()
        {
            cam = Camera.main;
            
            Permission.RequestUserPermission(Permission.FineLocation);
            Input.compass.enabled = true;

            InvokeRepeating(nameof(getOrigin), 2, 5);

            BuildingText.gameObject.SetActive(false);
            title.gameObject.SetActive(false);
            info.gameObject.SetActive(false);
            defaultInfoTitleScale = title.transform.localScale;
            defaultBuildingTextScale = BuildingText.transform.localScale;

            LocationHandler.locations = FileHandler.ReadFromJSON<Location>(filename);

            #region Instantiate Overlays

            clueOverlay = GameObject.Find("ClueOverlay").GetComponent<Canvas>();
            locationFoundOverlay = GameObject.Find("LocationFoundOverlay").GetComponent<Canvas>();
            InsideLocationOverlay = GameObject.Find("InsideLocationOverlay").GetComponent<Canvas>();
            gameCompleteOverlay = GameObject.Find("GameCompleteOverlay").GetComponent<Canvas>();
            nextButton = GameObject.Find("Next").GetComponent<Canvas>();
            showClue = GameObject.Find("ShowClue").GetComponent<Canvas>();
            gameAid = GameObject.Find("GameAidCanvas").GetComponent<Canvas>();
            startUpOverlay = GameObject.Find("StartOverlay").GetComponent<Canvas>();
            solutionOverlay = GameObject.Find("SolutionOverlay").GetComponent<Canvas>();
            locationUnavailableOverlay = GameObject
                .Find("LocationUnavailableOverlay")
                .GetComponent<Canvas>();

            restartButton = GameObject.Find("Restart").GetComponent<Canvas>();
            confirmRestart = GameObject.Find("RestartOverlay").GetComponent<Canvas>();
            // check if save file exists to check if user has opened the app before
            #endregion


            if (File.Exists(Path.Combine(Application.persistentDataPath, "PlayerData.dat")))
            {
                startUpOverlay.enabled = false;
            }


            #region Disable Overlays 

            confirmRestart.enabled = false;
            clueOverlay.enabled = false;
            nextButton.enabled = false;
            locationFoundOverlay.enabled = false;
            gameCompleteOverlay.enabled = false;
            gameAid.enabled = false;
            solutionOverlay.enabled = false;
            locationUnavailableOverlay.enabled = false;		
            restartButton.enabled = false;
            
            #endregion
        }

        public void LocationFound()
        {
            markerHandler.AddMarker(LocationHandler.GetCurrLocation());
            nextButton.enabled = true;
            showClue.enabled = false;
        }

        void Update()
        {
#if UNITY_EDITOR
#else
        if (!Player.CheckUserLocation() && startUpOverlay.enabled == false)
        {
            locationUnavailableOverlay.enabled = true;
        }
        else
        {
            locationUnavailableOverlay.enabled = false;
        }
#endif

            var location = Player.GetUserLocation();
            var curr = LocationHandler.GetCurrLocation();

            Vector3 normalisedCentre = BoundaryBoxes.ConvertToUnityCartesian(curr.centre, origin);
            Vector3 overlayLocation = normalisedCentre;
            
            float distanceFromOverlay = Vector3.Distance(cam.transform.position, overlayLocation);
            float scale = distanceFromOverlay / 5;


            if (
                LocationValidator.AtLocation(location, curr, origin)
                && !LocationValidator.LookingAtLocation(location, curr, origin, cam)
            )
            {
                HideLocationInformation();
                gameAid.enabled = true;
                int[] toDisplay = LocationVisibility.GetColour(BoundaryBoxes.ConvertToUnityCartesian(curr.centre,origin), cam);
                foreach (var comp in guideComponents)
                {
                    comp.enabled = false;
                }
                foreach (int o in toDisplay)
                {
                    guideComponents[o].enabled = true;
                }
                
                locationFoundOverlay.enabled = true;
                InsideLocationOverlay.enabled = false;
            }
            if (LocationValidator.LookingAtLocation(location, curr, origin, cam))
            {
                if (!BuildingText.gameObject.activeSelf)
                {
                    BuildingText.transform.LookAt(cam.transform.position);
                    BuildingText.transform.forward = -BuildingText.transform.forward;
                    ScaleText(new Vector3(scale,scale,1));
                }
                ShowLocationInformation(overlayLocation, curr);
                LocationFound();
                locationFoundOverlay.enabled = false;
                InsideLocationOverlay.enabled = false;
            }
            if (!LocationValidator.AtLocation(location, curr, origin))
            {
                HideLocationInformation();
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
                if (LocationValidator.LookingAtLocation(Player.GetUserLocation(), loc, origin, cam))
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
            int children = BuildingText.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                BuildingText.transform.GetChild(i).gameObject.SetActive(false);
            }
            info.enabled = false;
            title.enabled = false;
            gameAid.enabled = false;
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

            BuildingText.gameObject.SetActive(true);
            int children = BuildingText.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                BuildingText.transform.GetChild(i).gameObject.SetActive(true);
            }
            info.enabled = true;
            title.enabled = true;

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
                LocationHandler.NextLocation();
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

        
        public void ShowClue()
        {
            clueText.text = LocationHandler.GetCurrLocation().clue;
            clueOverlay.enabled = true;
        }

        public void CloseClue()
        {
            clueOverlay.enabled = false;
        }

        public void CloseStartUpPopUp()
        {
            startUpOverlay.enabled = false;
        }
        public void ShowSolution()
        {
            solutionOverlay.enabled = true;
            clueOverlay.enabled = false;
            solutionText.text = "You are looking for the " + 	LocationHandler.GetCurrLocation().name;
        }
        
        public void CloseSolution()
        {
            solutionOverlay.enabled = false;
        }

        public void ShowRestartConfirmation()
        {
            gameCompleteOverlay.enabled = false;
            confirmRestart.enabled = true;
        }

        public void CancelRestart()
        {
            confirmRestart.enabled = false;
        }

        public void ConfirmRestart()
        {
            LocationHandler.Restart();
            restartButton.enabled = false;
            confirmRestart.enabled = false;
            showClue.enabled = true;
            ShowClue();
        }

        void GameWon()
        {
            gameCompleteOverlay.enabled = true;
            showClue.enabled = false;
            nextButton.enabled = false;
            restartButton.enabled = true;
        }

        public void CloseGameCompleteOverlay()
        {
            gameCompleteOverlay.enabled = false;
        }
    }
}
