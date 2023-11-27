using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Tests
{
    public class NavBarSuit : InputTestFixture
    {
        Mouse mouse;
        public override void Setup()
        {
            base.Setup();
            EditorSceneManager.OpenScene("Assets/ExampleAssets/Scenes/SampleScene.unity");
            mouse = InputSystem.AddDevice<Mouse>();
        }

        public void ClickUI(GameObject uiElement)
        {
            Camera camera = GameObject.Find("XR Origin/Camera Offset/Main Camera").GetComponent<Camera>();
            Vector3 screenPos = camera.WorldToScreenPoint(uiElement.transform.position);
            Set(mouse.position, screenPos);
            Click(mouse.leftButton);
        }

        // Home Page button tests
        [UnityTest]
        public IEnumerator TestCameraButtonOnHomePage()
        {
            GameObject cameraButton = GameObject.Find("HomeCanvas/NavBar/CameraButton");
            ClickUI(cameraButton);

            Canvas cameraCanvas = GameObject.Find("CameraCanvas").GetComponent<Canvas>();
            Assert.IsTrue(cameraCanvas.enabled);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestMapButtonOnHomePage()
        {
            GameObject mapButton = GameObject.Find("HomeCanvas/NavBar/MapButton");
            ClickUI(mapButton);

            Canvas mapCanvas = GameObject.Find("MapCanvas").GetComponent<Canvas>();
            Assert.IsTrue(mapCanvas.enabled);

            yield return null;
        }


        [UnityTest]
        public IEnumerator TestInfoButtonOnHomePage()
        {
            GameObject infoButton = GameObject.Find("HomeCanvas/NavBar/InfoButton");
            ClickUI(infoButton);

            Canvas infoCanvas = GameObject.Find("InfoCanvas").GetComponent<Canvas>();
            Assert.IsTrue(infoCanvas.enabled);

            yield return null;
        }

        // Map page button tests
        [UnityTest]
        public IEnumerator HomeButtonOnMapPage()
        {
            GameObject homeButton = GameObject.Find("HomeCanvas/NavBar/HomeButton");
            ClickUI(homeButton);

            Canvas homeCanvas = GameObject.Find("HomeCanvas").GetComponent<Canvas>();
            Assert.IsTrue(homeCanvas.enabled);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CameraButtonOnMapPage()
        {
            GameObject cameraButton = GameObject.Find("MapCanvas/NavBar/CameraButton");
            ClickUI(cameraButton);

            Canvas cameraCanvas = GameObject.Find("CameraCanvas").GetComponent<Canvas>();
            Assert.IsTrue(cameraCanvas.enabled);

            yield return null;
        }

        [UnityTest]
        public IEnumerator InfoButtonOnMapPage()
        {
            GameObject infoButton = GameObject.Find("MapCanvas/NavBar/InfoButton");
            ClickUI(infoButton);

            Canvas infoCanvas = GameObject.Find("InfoCanvas").GetComponent<Canvas>();
            Assert.IsTrue(infoCanvas.enabled);

            yield return null;
        }

        // Camera page button tests
        [UnityTest]
        public IEnumerator HomeButtonOnCameraPage()
        {
            GameObject homeButton = GameObject.Find("CameraCanvas/NavBar/HomeButton");
            ClickUI(homeButton);

            Canvas homeCanvas = GameObject.Find("HomeCanvas").GetComponent<Canvas>();
            Assert.IsTrue(homeCanvas.enabled);

            yield return null;
        }

        [UnityTest]
        public IEnumerator MapButtonOnCameraPage()
        {
            GameObject mapButton = GameObject.Find("CameraCanvas/NavBar/MapButton");
            ClickUI(mapButton);

            Canvas mapCanvas = GameObject.Find("MapCanvas").GetComponent<Canvas>();
            Assert.IsTrue(mapCanvas.enabled);

            yield return null;
        }

        [UnityTest]
        public IEnumerator InfoButtonOnCameraPage()
        {
            GameObject infoButton = GameObject.Find("CameraCanvas/NavBar/InfoButton");
            ClickUI(infoButton);

            Canvas infoCanvas = GameObject.Find("InfoCanvas").GetComponent<Canvas>();
            Assert.IsTrue(infoCanvas.enabled);

            yield return null;
        }



        // Info page button tests
        [UnityTest]
        public IEnumerator HomeButtonOnInfoPage()
        {
            GameObject homeButton = GameObject.Find("InfoCanvas/NavBar/HomeButton");
            ClickUI(homeButton);

            Canvas homeCanvas = GameObject.Find("HomeCanvas").GetComponent<Canvas>();
            Assert.IsTrue(homeCanvas.enabled);

            yield return null;
        }
        [UnityTest]
        public IEnumerator CameraButtonOnInfoPage()
        {
            GameObject cameraButton = GameObject.Find("InfoCanvas/NavBar/CameraButton");
            ClickUI(cameraButton);

            Canvas cameraCanvas = GameObject.Find("CameraCanvas").GetComponent<Canvas>();
            Assert.IsTrue(cameraCanvas.enabled);

            yield return null;
        }

        [UnityTest]
        public IEnumerator MapButtonOnInfoPage()
        {
            GameObject mapButton = GameObject.Find("InfoCanvas/NavBar/MapButton");
            ClickUI(mapButton);

            Canvas mapCanvas = GameObject.Find("MapCanvas").GetComponent<Canvas>();
            Assert.IsTrue(mapCanvas.enabled);

            yield return null;
        }
    }

}

