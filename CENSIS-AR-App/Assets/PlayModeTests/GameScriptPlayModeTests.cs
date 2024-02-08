using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class GameScriptPlayModeTests
{
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Assets/Scenes/AppScene.unity");
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

    //[UnityTest]
    //public IEnumerator TestNextIsFinalLocation()
    //{
    //    // initialise empty list
    //    LocationHandler.locations = new List<Location>();
    //    Button nextButton = GameObject.Find("NextButton").GetComponent<Button>();
    //    nextButton.onClick.Invoke();
    //    // test overlay 
    //    yield return null;
    //}

    [UnityTest]
    public IEnumerator TestCorrectOverlayOnStart()
    {
        Assert.IsTrue(LocationHandler.locations.Count > 0);
        Assert.IsFalse(GetCanvas("ClueOverlay").enabled);
        Assert.IsFalse(GetCanvas("Next").enabled);
        Assert.IsTrue(GetCanvas("ShowClue").enabled);

        yield return null;
    }


}
