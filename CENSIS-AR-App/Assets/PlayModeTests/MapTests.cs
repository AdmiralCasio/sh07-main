using System.Collections;
using Mapbox.Map;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
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
        Transform editorLocation;
        AbstractMap map;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            SceneManager.LoadScene("AppScene", LoadSceneMode.Single);
            yield return new EnterPlayModeOptions();
            editorLocation = GameObject.Find("EditorLocationObject").transform;
            map = GameObject.Find("Map").GetComponent<AbstractMap>();
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

        [UnityTest]
        public IEnumerator Update_LocationMove_PlayerIconMoveCorrectLocation()
        {
            yield return null;

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

            yield return null;

            var player = GameObject.Find("Player");
            var expectedLocation = map.GeoToWorldPosition(new Vector2d(55.87389437464513, -4.292198433520163));

            Assert.AreEqual(expectedLocation, player.transform.position);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return null;
        }
    }

}
