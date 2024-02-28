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
    public IEnumerator MapActiveOnSceneLoad()
    {
        yield return null;
        Assert.IsTrue(GameObject.Find("Map").GetComponent<AbstractMap>().isActiveAndEnabled);
    }

    [UnityTest]
    public IEnumerator MapNotNullOnSceneLoad()
    {
        yield return null;
        Assert.IsNotNull(GameObject.Find("Map").GetComponent<AbstractMap>());
    }

    [UnityTest]
    public IEnumerator MapSmallOnHomeScreen()
    {
        Button homeButton = GameObject.Find("NavbarAndTopBar/NavBar/HomeButton").GetComponent<Button>();
        homeButton.onClick.Invoke();

        yield return null;

        var map = GameObject.Find("MapCamera");
        Assert.AreEqual(0.4f, map.GetComponent<Camera>().rect.height);
    }

    [UnityTest]
    public IEnumerator MapFullOnMapScreen()
    {
        Button mapButton = GameObject.Find("NavbarAndTopBar/NavBar/MapButton").GetComponent<Button>();
        mapButton.onClick.Invoke();

        yield return null;

        var map = GameObject.Find("MapCamera");
        Assert.AreEqual(1f, map.GetComponent<Camera>().rect.height);
    }

    [UnityTest]
    public IEnumerator MapSizeChangeMapToHome()
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
    public IEnumerator MapSizeChangeHomeToMap()
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

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return null;
    }

}
