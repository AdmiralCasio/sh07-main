using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using CENSIS.Runtime;
using Location = CENSIS.Locations.Location;
using CENSIS.Utility;

namespace PlayModeTests
{
    public class GameScriptPlayModeTests
    {
        Transform editorLocation;
        AbstractMap map;
    
        [UnitySetUp]
        public IEnumerator Setup()
        {
            string saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.dat");
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }
    
            SceneManager.LoadScene("Assets/Scenes/AppScene.unity");
    
            yield return null;
    
            editorLocation = GameObject.Find("EditorLocationObject").GetComponent<Transform>();
            map = GameObject.Find("Map").GetComponent<AbstractMap>();
    
            #region
            editorLocation.SetPositionAndRotation(
                Conversions
                    .GeoToWorldPosition(
                        new Vector2d(55.8732190726261, -4.29288021410597),
                        map.CenterMercator,
                        map.WorldRelativeScale
                    )
                    .ToVector3xz(),
                Quaternion.identity
            );
            var lp = (TransformLocationProvider)
                LocationProviderFactory.Instance.DefaultLocationProvider;
            lp.TargetTransform = editorLocation;
            lp.SendLocationEvent();
            #endregion
    
            yield return null;
        }
    
        public Canvas GetCanvas(string canvas)
        {
            return GameObject.Find(canvas).GetComponent<Canvas>();
        }
    
        [UnityTest]
        public IEnumerator LocationFound_WhenLocationFound_NextButtonAndClueShown()
        {
            GameObject gameScriptObject = GameObject.Find("GameScriptObject");
            GameScript gameScript = gameScriptObject.GetComponent<GameScript>();
            gameScript.LocationFound();
    
            Assert.IsTrue(GameObject.Find("Next").GetComponent<Canvas>().enabled);
            Assert.IsFalse(GameObject.Find("ShowClue").GetComponent<Canvas>().enabled);
            yield return null;
        }
    
        [UnityTest]
        public IEnumerator ShowClue_WhenShowClueClicked_ClueOverlayShown()
        {
            Button showClueButton = GameObject.Find("ShowClueButton").GetComponent<Button>();
            showClueButton.onClick.Invoke();
    
            Assert.IsTrue(GetCanvas("ClueOverlay").enabled);
    
            yield return null;
        }
    
        [UnityTest]
        public IEnumerator CloseClue_WhenCloseClueClicked_ClueOverlayHidden()
        {
            GetCanvas("ClueOverlay").enabled = true;
            Button closeClueButton = GameObject.Find("CloseButton").GetComponent<Button>();
            closeClueButton.onClick.Invoke();
    
            Assert.IsFalse(GetCanvas("ClueOverlay").enabled);
    
            yield return null;
        }
    
        [UnityTest]
        public IEnumerator Next_NotFinalLocation_ClueOverlayAndShowClueShownNextButtonHidden()
        {
            Button nextButton = GameObject.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.Invoke();
    
            Assert.IsTrue(GetCanvas("ClueOverlay").enabled);
            Assert.IsFalse(GetCanvas("Next").enabled);
            Assert.IsTrue(GetCanvas("ShowClue").enabled);
    
            yield return null;
        }
    
        [UnityTest]
        public IEnumerator Next_IsFinalLocation_GameCompleteOverlayShown()
        {
            // initialise empty list
            LocationHandler.locations = new List<Location>();
            LocationHandler.locations.Add(
                new Location(
                    "testLocation",
                    "clue text",
                    "information",
                    new float[] { 55.87394f, -4.29181f },
                    new float[][][]
                    {
                        new float[][]
                        {
                            new float[] { 55.6829478f, -4.5160826f },
                            new float[] { 55.6830784f, -4.5155368f },
                            new float[] { 55.6827737f, -4.5153076f },
                            new float[] { 55.6826432f, -4.5158534f },
                            new float[] { 55.6829478f, -4.5160826f },
                        }
                    },
                    new float[][][]
                    {
                        new float[][]
                        {
                            new float[] { 55.6827422f, -4.5150968f },
                            new float[] { 55.6831358f, -4.5154564f },
                            new float[] { 55.6829316f, -4.5165759f },
                            new float[] { 55.6824903f, -4.5160380f },
                            new float[] { 55.6827422f, -4.5150968f }
                        }
                    }
                )
            );
            Button nextButton = GameObject.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.Invoke();
            // test overlay
            Assert.IsTrue(GetCanvas("GameCompleteOverlay").enabled);
    
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShowSolution_StuckButtonClicked_ShowSolution()
        {
            Canvas solutionOverlay = GetCanvas("SolutionOverlay");
            Assert.False(solutionOverlay.enabled);
            Button stuckButton = GameObject.Find("StuckButton").GetComponent<Button>();
            stuckButton.onClick.Invoke();
            Assert.True(solutionOverlay.enabled);
            
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator CloseSolution_CloseButtonClick_CloseSolution()
        {
            Canvas solutionOverlay = GetCanvas("SolutionOverlay");
            solutionOverlay.enabled = true;
            Button closeButton = GameObject.Find("SolutionCloseButton").GetComponent<Button>();
            closeButton.onClick.Invoke();
            Assert.False(solutionOverlay.enabled);
            
            yield return null;
        }
    
        [UnityTest]
        public IEnumerator Start_FirstStart_LocationsPopulatedShowClueButtonAndStartOverlay()
        {
            string saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.dat");
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }
    
            yield return null;
    
            Assert.IsTrue(LocationHandler.locations.Count > 0);
            Assert.IsFalse(GetCanvas("ClueOverlay").enabled);
            Assert.IsFalse(GetCanvas("Next").enabled);
            Assert.IsTrue(GetCanvas("ShowClue").enabled);
            Assert.IsTrue(GetCanvas("StartOverlay").enabled);
    
            yield return null;
        }
    
        [UnityTest]
        public IEnumerator Start_NotFirstStart_LocationsPopulatedShowClueButtonNoStartOverlay()
        {
            
            Button nextButton = GameObject.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.Invoke();
    
            SceneManager.LoadScene("Assets/Scenes/AppScene.unity");
            yield return null;
    
            Assert.IsTrue(LocationHandler.locations.Count > 0);
            Assert.IsFalse(GetCanvas("ClueOverlay").enabled);
            Assert.IsFalse(GetCanvas("Next").enabled);
            Assert.IsTrue(GetCanvas("ShowClue").enabled);
            Assert.IsFalse(GetCanvas("StartOverlay").enabled);
    
            yield return null;
        }
    
        [UnityTest]
        public IEnumerator Update_NotAtPreviousLocation_NoTextDisplayed()
        {
            GameScript gameScript = GameObject.Find("GameScriptObject").GetComponent<GameScript>();
    
            gameScript.LocationFound();
            gameScript.Next();
    
            #region
            editorLocation.SetPositionAndRotation(
                Conversions
                    .GeoToWorldPosition(new Vector2d(5, 43), map.CenterMercator, map.WorldRelativeScale)
                    .ToVector3xz(),
                Quaternion.identity
            );
            var lp = (TransformLocationProvider)
                LocationProviderFactory.Instance.DefaultLocationProvider;
            lp.TargetTransform = editorLocation;
            lp.SendLocationEvent();
            #endregion
    
            var location = LocationHandler.locations.Find(location =>
                location.name == "Sir Alwyn Williams"
            );
    
            yield return null;
    
            Assert.IsFalse(gameScript.BuildingText.activeSelf);
            Assert.IsFalse(gameScript.info.enabled);
            Assert.IsFalse(gameScript.title.enabled);
        }
    
        [UnityTest]
        public IEnumerator Update_AtPreviousLocation_TextDisplayed()
        {
            GameScript gameScript = GameObject.Find("GameScriptObject").GetComponent<GameScript>();
    
            gameScript.LocationFound();
            gameScript.Next();
    
            #region
            editorLocation.SetPositionAndRotation(
                Conversions
                    .GeoToWorldPosition(
                        new Vector2d(55.87389437464513, -4.292198433520163),
                        map.CenterMercator,
                        map.WorldRelativeScale
                    )
                    .ToVector3xz(),
                Quaternion.identity
            );
            var lp = (TransformLocationProvider)
                LocationProviderFactory.Instance.DefaultLocationProvider;
            lp.TargetTransform = editorLocation;
            lp.SendLocationEvent();
            #endregion
    
            var location = LocationHandler.locations.Find(location =>
                location.name == "Sir Alwyn Williams"
            );
    
            GameObject n = new GameObject();
            Camera.main.transform.parent = n.transform;
            n.transform.position = BoundaryBoxes.ConvertToUnityCartesian(
                Player.GetUserLocation(),
                gameScript.origin
            );
            n.transform.LookAt(BoundaryBoxes.ConvertToUnityCartesian(location.centre));
    
            yield return null;
    
            Assert.IsTrue(gameScript.BuildingText.activeSelf);
            Assert.IsTrue(gameScript.info.enabled);
            Assert.IsTrue(gameScript.title.enabled);
        }
    
        [UnityTest]
        public IEnumerator Update_AtFutureLocation_NoTextDisplayed()
        {
            GameScript gameScript = GameObject.Find("GameScriptObject").GetComponent<GameScript>();
    
            #region
            editorLocation.SetPositionAndRotation(
                Conversions
                    .GeoToWorldPosition(
                        new Vector2d(55.8718268818084, -4.2951840883561845),
                        map.CenterMercator,
                        map.WorldRelativeScale
                    )
                    .ToVector3xz(),
                Quaternion.identity
            );
            var lp = (TransformLocationProvider)
                LocationProviderFactory.Instance.DefaultLocationProvider;
            lp.TargetTransform = editorLocation;
            lp.SendLocationEvent();
            #endregion
    
            var location = LocationHandler.locations.Find(location =>
                location.name == "Advanced Research Centre"
            );
    
            GameObject n = new GameObject();
            Camera.main.transform.parent = n.transform;
            n.transform.position = BoundaryBoxes.ConvertToUnityCartesian(
                Player.GetUserLocation(),
                gameScript.origin
            );
            n.transform.LookAt(BoundaryBoxes.ConvertToUnityCartesian(location.centre));
            Camera.main.transform.position = BoundaryBoxes.ConvertToUnityCartesian(
                Player.GetUserLocation(),
                gameScript.origin
            );
            Camera.main.transform.LookAt(BoundaryBoxes.ConvertToUnityCartesian(location.centre));
    
            yield return null;
    
            Assert.IsFalse(gameScript.BuildingText.activeSelf);
            Assert.IsFalse(gameScript.info.enabled);
            Assert.IsFalse(gameScript.title.enabled);
        }
    
        [UnityTest]
        public IEnumerator Update_WasAtPreviousLocation_NoTextDisplayed()
        {
            GameScript gameScript = GameObject.Find("GameScriptObject").GetComponent<GameScript>();
    
            gameScript.LocationFound();
            gameScript.Next();
    
            #region
            editorLocation.SetPositionAndRotation(
                Conversions
                    .GeoToWorldPosition(
                        new Vector2d(55.87389437464513, -4.292198433520163),
                        map.CenterMercator,
                        map.WorldRelativeScale
                    )
                    .ToVector3xz(),
                Quaternion.identity
            );
            var lp = (TransformLocationProvider)
                LocationProviderFactory.Instance.DefaultLocationProvider;
            lp.TargetTransform = editorLocation;
            lp.SendLocationEvent();
            #endregion
    
            var location = LocationHandler.locations.Find(location =>
                location.name == "Sir Alwyn Williams"
            );
    
            GameObject n = new GameObject();
            Camera.main.transform.parent = n.transform;
            n.transform.position = BoundaryBoxes.ConvertToUnityCartesian(
                Player.GetUserLocation(),
                gameScript.origin
            );
            n.transform.LookAt(BoundaryBoxes.ConvertToUnityCartesian(location.centre));
    
            yield return null;
    
            #region
            editorLocation.SetPositionAndRotation(
                Conversions
                    .GeoToWorldPosition(
                        new Vector2d(55.893793849785176, -4.324952777065971),
                        map.CenterMercator,
                        map.WorldRelativeScale
                    )
                    .ToVector3xz(),
                Quaternion.identity
            );
            lp = (TransformLocationProvider)LocationProviderFactory.Instance.DefaultLocationProvider;
            lp.TargetTransform = editorLocation;
            lp.SendLocationEvent();
    
            yield return null;
            #endregion
    
            Assert.IsFalse(gameScript.BuildingText.activeSelf);
            Assert.IsFalse(gameScript.info.enabled);
            Assert.IsFalse(gameScript.title.enabled);
        }
    
        [UnityTest]
        public IEnumerator ShowInsidePopup_TooCloseToBuilding_PopupVisible()
        {
            #region
            editorLocation.SetPositionAndRotation(
                Conversions
                    .GeoToWorldPosition(
                        new Vector2d(LocationHandler.GetCurrLocation().centre.x,LocationHandler.GetCurrLocation().centre.y),
                        map.CenterMercator,
                        map.WorldRelativeScale
                    )
                    .ToVector3xz(),
                Quaternion.identity
            );
            var lp = (TransformLocationProvider)LocationProviderFactory.Instance.DefaultLocationProvider;
            lp.TargetTransform = editorLocation;
            lp.SendLocationEvent();
            yield return null;
    
            #endregion
    
            Assert.IsNotNull(GameObject.Find("InsideLocationOverlay"));
            Assert.IsTrue(GameObject.Find("InsideLocationOverlay").GetComponent<Canvas>().enabled);
            yield return null;
        }
        [UnityTest]
        public IEnumerator ShowInsidePopup_NotTooCloseToBuilding_PopupNotVisible()
        {
            #region
            editorLocation.SetPositionAndRotation(
                Conversions
                    .GeoToWorldPosition(
                        new Vector2d(LocationHandler.GetCurrLocation().outer[0].points[0].x,LocationHandler.GetCurrLocation().outer[0].points[0].y),
                        map.CenterMercator,
                        map.WorldRelativeScale
                    )
                    .ToVector3xz(),
                Quaternion.identity
            );
            var lp = (TransformLocationProvider)LocationProviderFactory.Instance.DefaultLocationProvider;
            lp.TargetTransform = editorLocation;
            lp.SendLocationEvent();
            yield return null;
    
            #endregion
    
            Assert.IsNotNull(GameObject.Find("InsideLocationOverlay"));
            Assert.IsFalse(GameObject.Find("InsideLocationOverlay").GetComponent<Canvas>().enabled);
            yield return null;
        }
    
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            string saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.dat");
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }
            yield return null;
        }
    }

}
