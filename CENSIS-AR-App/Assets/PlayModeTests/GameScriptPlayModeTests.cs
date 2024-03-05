using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class GameScriptPlayModeTests
{
    string saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.dat");

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Assets/Scenes/AppScene.unity");
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
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
    public IEnumerator Start_FirstStart_LocationsPopulatedShowClueButtonAndStartOverlay()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
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
        //LocationHandler.locations = new List<Location>();
        //LocationHandler.locations.Add(new Location("testLocation", "clue text", "information", new float[] { 55.87394f, -4.29181f }, new float[][][]{ new float[][]{
        //new float[] {55.6829478f,-4.5160826f },
        //new float[] {55.6830784f, -4.5155368f},
        //new float[] {55.6827737f, -4.5153076f },
        //new float[] {55.6826432f, -4.5158534f },
        //new float[] {55.6829478f,-4.5160826f }, }
        //}, new float[][][]{ new float [][]{
        //new float[] {55.6827422f, -4.5150968f },
        //new float[] {55.6831358f, -4.5154564f },
        //new float[] {55.6829316f, -4.5165759f },
        //new float[] {55.6824903f, -4.5160380f },
        //new float[] {55.6827422f, -4.5150968f } } }
        //));
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
}
