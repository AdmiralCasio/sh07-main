using System.Collections;
using System.Security.Policy;
using Mapbox.Examples;
using Mapbox.Map;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Moq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class MapTests
{
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("AppScene", LoadSceneMode.Single);
        yield return new EnterPlayModeOptions();
    }

    [UnityTest]
    public IEnumerator SceneLoad_GameStartup_MapActiveAndEnabled()
    {
        yield return null;
        Assert.IsTrue(GameObject.Find("Map").GetComponent<AbstractMap>().isActiveAndEnabled);
    }

    [UnityTest]
    public IEnumerator SceneLoad_GameStartup_MapNotNull()
    {
        yield return null;
        Assert.IsNotNull(GameObject.Find("Map").GetComponent<AbstractMap>());
    }

    [UnityTest]
    public IEnumerator ActivateCanvas_MapScreen_MapHeight0Point4()
    {
        Button homeButton = GameObject.Find("NavbarAndTopBar/NavBar/HomeButton").GetComponent<Button>();
        homeButton.onClick.Invoke();

        yield return null;

        var map = GameObject.Find("MapCamera");
        Assert.AreEqual(0.4f, map.GetComponent<Camera>().rect.height);
    }

    [UnityTest]
    public IEnumerator Start_HomeScreen_MapHeight1()
    {
        Button mapButton = GameObject.Find("NavbarAndTopBar/NavBar/MapButton").GetComponent<Button>();
        mapButton.onClick.Invoke();

        yield return null;

        var map = GameObject.Find("MapCamera");
        Assert.AreEqual(1f, map.GetComponent<Camera>().rect.height);
    }

    [UnityTest]
    public IEnumerator ActivateCanvas_HomeButtonOnMap_MapHeight0Point4()
    {
        var map = GameObject.Find("MapCamera");

        Button mapButton = GameObject.Find("NavbarAndTopBar/NavBar/MapButton").GetComponent<Button>();
        mapButton.onClick.Invoke();

        yield return null;

        Assert.AreEqual(1f, map.GetComponent<Camera>().rect.height);

        Button homeButton = GameObject.Find("NavbarAndTopBar/NavBar/HomeButton").GetComponent<Button>();
        homeButton.onClick.Invoke();

        yield return null;

        Assert.AreEqual(0.4f, map.GetComponent<Camera>().rect.height);
    }

    [UnityTest]
    public IEnumerator ActivateCanvas_MapButtonOnHome_MapHeight1()
    {
        Button homeButton = GameObject.Find("NavbarAndTopBar/NavBar/HomeButton").GetComponent<Button>();
        homeButton.onClick.Invoke();

        yield return null;

        var map = GameObject.Find("MapCamera");
        Assert.AreEqual(0.4f, map.GetComponent<Camera>().rect.height);

        Button mapButton = GameObject.Find("NavbarAndTopBar/NavBar/MapButton").GetComponent<Button>();
        mapButton.onClick.Invoke();

        yield return null;

        Assert.AreEqual(1f, map.GetComponent<Camera>().rect.height);
    }

    [UnityTest]
    public IEnumerator AddMarker_AbitraryLocation_InstantiatesPrefab()
    {
        MarkerHandler markerHandler = GameObject.Find("Map").GetComponent<MarkerHandler>();
        Location loc = new Location("TestLoc", "Clue", "Info", new float[] { 5, 4 }, new float[][][] { }, new float[][][] { });
        markerHandler.AddMarker(loc);

        yield return null;

        var result = GameObject.Find("TestLoc").gameObject;
        Assert.IsNotNull(result);
        Assert.IsTrue(result.activeSelf);
    }

    [UnityTest]
    public IEnumerator AddMarker_LocationTestLoc_PrefabNameTestLoc()
    {
        MarkerHandler markerHandler = GameObject.Find("Map").GetComponent<MarkerHandler>();
        Location loc = new Location("TestLoc", "Clue", "Info", new float[] { 5, 4 }, new float[][][] { }, new float[][][] { });
        markerHandler.AddMarker(loc);

        yield return null;

        var result = GameObject.Find("TestLoc").gameObject;
        Assert.AreEqual("TestLoc", result.name);
    }



    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return null;
    }

}
