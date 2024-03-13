using System.Collections;
using Mapbox.Unity.Map;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace PlayModeTests
{
    public class MapTests
    {
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            SceneManager.LoadScene("AppScene", LoadSceneMode.Single);
            yield return new EnterPlayModeOptions();
        }
    
        [UnityTest]
        public IEnumerator Start_OnSceneLoad_MapActive()
        {
            yield return null;
            Assert.IsTrue(GameObject.Find("Map").GetComponent<AbstractMap>().isActiveAndEnabled);
        }
    
        [UnityTest]
        public IEnumerator Start_OnSceneLoad_MapNotNull()
        {
            yield return null;
            Assert.IsNotNull(GameObject.Find("Map").GetComponent<AbstractMap>());
        }
    
        [UnityTest]
        public IEnumerator ActivateCanvas_OnHomeScreen_MapSmall()
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
        public IEnumerator ActivateCanvas_OnMapScreen_MapFull()
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
        public IEnumerator ActivateCanvas_MapPageToHomePage_MapSizeChange()
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
        public IEnumerator ActivateCanvas_HomePageToMapPage_MapSizeChange()
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
    
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return null;
        }
    }

}
