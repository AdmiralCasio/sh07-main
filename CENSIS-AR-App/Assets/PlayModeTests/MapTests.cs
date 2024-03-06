using System.Collections;
using System.Security.Policy;
using Mapbox.Examples;
using Mapbox.Map;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.IO;

public class MapTests
{
    Transform editorLocation;


    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("AppScene", LoadSceneMode.Single);

        yield return null;
        var saveFilePath = Path.Combine(Application.persistentDataPath, "PlayerData.dat");
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }

        editorLocation = GameObject.Find("EditorLocationObject").transform;

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
        Button homeButton = GameObject
            .Find("NavbarAndTopBar/NavBar/HomeButton")
            .GetComponent<Button>();
        homeButton.onClick.Invoke();

        yield return null;

        var map = GameObject.Find("MapCamera");
        Assert.AreEqual(0.4f, map.GetComponent<Camera>().rect.height);
    }

    [UnityTest]
    public IEnumerator Start_HomeScreen_MapHeight1()
    {
        Button mapButton = GameObject
            .Find("NavbarAndTopBar/NavBar/MapButton")
            .GetComponent<Button>();
        mapButton.onClick.Invoke();

        yield return null;

        var map = GameObject.Find("MapCamera");
        Assert.AreEqual(1f, map.GetComponent<Camera>().rect.height);
    }

    [UnityTest]
    public IEnumerator ActivateCanvas_HomeButtonOnMap_MapHeight0Point4()
    {
        var map = GameObject.Find("MapCamera");

        Button mapButton = GameObject
            .Find("NavbarAndTopBar/NavBar/MapButton")
            .GetComponent<Button>();
        mapButton.onClick.Invoke();

        yield return null;

        Assert.AreEqual(1f, map.GetComponent<Camera>().rect.height);

        Button homeButton = GameObject
            .Find("NavbarAndTopBar/NavBar/HomeButton")
            .GetComponent<Button>();
        homeButton.onClick.Invoke();

        yield return null;

        Assert.AreEqual(0.4f, map.GetComponent<Camera>().rect.height);
    }

    [UnityTest]
    public IEnumerator ActivateCanvas_MapButtonOnHome_MapHeight1()
    {
        Button homeButton = GameObject
            .Find("NavbarAndTopBar/NavBar/HomeButton")
            .GetComponent<Button>();
        homeButton.onClick.Invoke();

        yield return null;

        var map = GameObject.Find("MapCamera");
        Assert.AreEqual(0.4f, map.GetComponent<Camera>().rect.height);

        Button mapButton = GameObject
            .Find("NavbarAndTopBar/NavBar/MapButton")
            .GetComponent<Button>();
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

    [UnityTest]
    public IEnumerator AddMarker_SirAlwynWilliams_InstantiatesPrefab()
    {
        AbstractMap map = GameObject.Find("Map").GetComponent<AbstractMap>();
        GameScript gs = GameObject.Find("GameScriptObject").GetComponent<GameScript>();
        #region
        editorLocation.SetPositionAndRotation(Conversions.GeoToWorldPosition(new Vector2d(55.873900, -4.292276), map.CenterMercator, map.WorldRelativeScale).ToVector3xz(), Quaternion.identity);
        TransformLocationProvider lp = (TransformLocationProvider)LocationProviderFactory.Instance.DefaultLocationProvider;
        lp.TargetTransform = editorLocation;
        lp.SendLocationEvent();
        #endregion

        GameObject o = new GameObject();
        Camera.main.transform.parent = o.transform;
        o.transform.position = BoundaryBoxes.ConvertToUnityCartesian(Player.GetUserLocation(), gs.origin);
        o.transform.LookAt(BoundaryBoxes.ConvertToUnityCartesian(LocationHandler.GetCurrLocation().centre));

        yield return null;

        GameObject marker = GameObject.Find("Sir Alwyn Williams").gameObject;
        Assert.IsNotNull(marker);
        Assert.IsTrue(marker.activeSelf);


    }

    [UnityTest]
    public IEnumerator AddMarker_SirAlwynWilliams_PrefabNameSirAlwynWilliams()
    {
        AbstractMap map = GameObject.Find("Map").GetComponent<AbstractMap>();
        GameScript gs = GameObject.Find("GameScriptObject").GetComponent<GameScript>();
        #region
        editorLocation.SetPositionAndRotation(Conversions.GeoToWorldPosition(new Vector2d(55.873900, -4.292276), map.CenterMercator, map.WorldRelativeScale).ToVector3xz(), Quaternion.identity);
        TransformLocationProvider lp = (TransformLocationProvider)LocationProviderFactory.Instance.DefaultLocationProvider;
        lp.TargetTransform = editorLocation;
        lp.SendLocationEvent();
        #endregion

        yield return null;

        GameObject o = new GameObject();
        Camera.main.transform.parent = o.transform;
        o.transform.position = BoundaryBoxes.ConvertToUnityCartesian(new Vector2((float)lp.CurrentLocation.LatitudeLongitude.x, (float)lp.CurrentLocation.LatitudeLongitude.y), gs.origin);
        o.transform.LookAt(BoundaryBoxes.ConvertToUnityCartesian(LocationHandler.GetCurrLocation().centre));

        yield return null;

        GameObject marker = GameObject.Find("Sir Alwyn Williams").gameObject;
        Assert.AreEqual(marker.name, LocationHandler.GetCurrLocation().name);

    }


    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return null;
    }
}
