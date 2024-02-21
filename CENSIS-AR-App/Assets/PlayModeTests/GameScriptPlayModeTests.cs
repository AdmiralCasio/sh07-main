using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class GameScriptPlayModeTests
{
    Transform editorLocation;
    GameObject[] rootGameObjects;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Assets/Scenes/AppScene.unity");

        yield return null;

        rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        editorLocation = rootGameObjects[rootGameObjects.Length - 1].transform;
        string saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.dat");
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        yield return null;

    }

    public Canvas GetCanvas(string canvas)
    {
        return GameObject.Find(canvas).GetComponent<Canvas>();
    }

    [UnityTest]
    public IEnumerator TestLocationFound()
    {
        GameObject gameScriptObject = GameObject.Find("GameScriptObject");
        GameScript gameScript = gameScriptObject.GetComponent<GameScript>();
        gameScript.LocationFound();

        Assert.IsTrue(GameObject.Find("Next").GetComponent<Canvas>().enabled);
        Assert.IsFalse(GameObject.Find("ShowClue").GetComponent<Canvas>().enabled);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestShowClue()
    {
        Button showClueButton = GameObject.Find("ShowClueButton").GetComponent<Button>();
        showClueButton.onClick.Invoke();

        Assert.IsTrue(GetCanvas("ClueOverlay").enabled);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestCloseClue()
    {
        GetCanvas("ClueOverlay").enabled = true;
        Button closeClueButton = GameObject.Find("CloseButton").GetComponent<Button>();
        closeClueButton.onClick.Invoke();

        Assert.IsFalse(GetCanvas("ClueOverlay").enabled);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestNextNotFinalLocation()
    {
        Button nextButton = GameObject.Find("NextButton").GetComponent<Button>();
        nextButton.onClick.Invoke();

        Assert.IsTrue(GetCanvas("ClueOverlay").enabled);
        Assert.IsFalse(GetCanvas("Next").enabled);
        Assert.IsTrue(GetCanvas("ShowClue").enabled);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestNextIsFinalLocation()
    {
        // initialise empty list
        LocationHandler.locations = new List<Location>();
        LocationHandler.locations.Add(new Location("testLocation", "clue text", "information", new float[] { 55.87394f, -4.29181f }, new float[][][]{ new float[][]{
        new float[] {55.6829478f,-4.5160826f },
        new float[] {55.6830784f, -4.5155368f},
        new float[] {55.6827737f, -4.5153076f },
        new float[] {55.6826432f, -4.5158534f },
        new float[] {55.6829478f,-4.5160826f }, }
        }, new float[][][]{ new float [][]{
        new float[] {55.6827422f, -4.5150968f },
        new float[] {55.6831358f, -4.5154564f },
        new float[] {55.6829316f, -4.5165759f },
        new float[] {55.6824903f, -4.5160380f },
        new float[] {55.6827422f, -4.5150968f } } }
        ));
        Button nextButton = GameObject.Find("NextButton").GetComponent<Button>();
        nextButton.onClick.Invoke();
        // test overlay 

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestCorrectOverlayOnStart()
    {
        Assert.IsTrue(LocationHandler.locations.Count > 0);
        Assert.IsFalse(GetCanvas("ClueOverlay").enabled);
        Assert.IsFalse(GetCanvas("Next").enabled);
        Assert.IsTrue(GetCanvas("ShowClue").enabled);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Update_NotAtPreviousLocation_NoTextDisplayed()
    {

        var editorLocationProvider = (EditorLocationProvider)LocationProviderFactory.Instance.EditorLocationProvider;
        var map = LocationProviderFactory.Instance.mapManager;
        editorLocation.SetPositionAndRotation(VectorExtensions.ToVector3xz(Conversions.GeoToWorldPosition(new Vector2d(55.87383, -4.29214), map.CenterMercator, map.WorldRelativeScale)), Quaternion.identity);
        editorLocationProvider.SendLocationEvent();

        yield return null;

        editorLocation.SetPositionAndRotation(VectorExtensions.ToVector3xz(Conversions.GeoToWorldPosition(new Vector2d(55.87412, -4.29383), map.CenterMercator, map.WorldRelativeScale)), Quaternion.identity);
        editorLocationProvider.SendLocationEvent();

        yield return null;

        Assert.IsNull(GameObject.Find("BuildingText"));

    }

    [UnityTest]
    public IEnumerator Update_AtPreviousLocation_TextDisplayed()
    {
        Debug.ClearDeveloperConsole();

        yield return null;

        var transformLocationProvider = (TransformLocationProvider)LocationProviderFactory.Instance.EditorLocationProvider;
        var map = LocationProviderFactory.Instance.mapManager;
        editorLocation.SetPositionAndRotation(VectorExtensions.ToVector3xz(Conversions.GeoToWorldPosition(new Vector2d(55.87388545492344, -4.292255711315057), map.CenterMercator, map.WorldRelativeScale)), Quaternion.identity);
        Debug.Log(LocationValidator.AtLocation(LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.LatitudeLongitude.ToVector3xz(), LocationHandler.GetCurrLocation()));
        transformLocationProvider.TargetTransform = editorLocation;
        transformLocationProvider.SendLocationEvent();
        Debug.Log(transformLocationProvider.CurrentLocation.LatitudeLongitude);
        Debug.Log(LocationValidator.AtLocation(LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.LatitudeLongitude.ToVector3xz(), LocationHandler.GetCurrLocation()));

        yield return null;
        
        Debug.Log(LocationHandler.GetCurrLocation().name);


        editorLocation.SetPositionAndRotation(VectorExtensions.ToVector3xz(Conversions.GeoToWorldPosition(new Vector2d(55.87299574483832, -4.295382266171097), map.CenterMercator, map.WorldRelativeScale)), Quaternion.identity);
        Debug.Log(editorLocation.position);
        Debug.Log(Conversions.GeoToWorldPosition(transformLocationProvider.CurrentLocation.LatitudeLongitude, map.CenterMercator, map.WorldRelativeScale).ToVector3xz() + editorLocation.position);
        Debug.Log(LocationHandler.GetCurrLocation().name);
        transformLocationProvider.TargetTransform = editorLocation;
        transformLocationProvider.SendLocationEvent();
        Debug.Log(LocationProviderFactory.Instance.EditorLocationProvider.CurrentLocation.LatitudeLongitude);
        Debug.Log(LocationValidator.AtLocation(LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.LatitudeLongitude.ToVector3xz(), LocationHandler.GetCurrLocation()));

        yield return null;

        editorLocation.SetPositionAndRotation(VectorExtensions.ToVector3xz(Conversions.GeoToWorldPosition(new Vector2d(55.87388545492344, -4.292255711315057), map.CenterMercator, map.WorldRelativeScale)), Quaternion.identity);
        transformLocationProvider.TargetTransform = editorLocation;
        transformLocationProvider.SendLocationEvent();
        Debug.Log(LocationValidator.AtLocation(LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.LatitudeLongitude.ToVector3xz(), LocationHandler.GetCurrLocation()));
        Debug.Log(transformLocationProvider.CurrentLocation.LatitudeLongitude);

        yield return null;

        Assert.IsNotNull(GameObject.Find("BuildingText"));
        Assert.IsTrue(GameObject.Find("BuildingText").activeSelf);
    }

    [UnityTest]
    public IEnumerator Update_AtFutureLocation_NoTextDisplayed()
    {
        throw new System.NotImplementedException();
    }

}
