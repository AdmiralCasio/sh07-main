using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Web;
using UnityEngine.UI;

public class NavBarSuit : InputTestFixture
    {
        Mouse mouse;
        public override void Setup()
        {
            base.Setup();
            EditorSceneManager.OpenScene("Assets/Scenes/AppScene.unity");
        }

     

        public Canvas GetCanvas(string canvas)
        {
            return GameObject.Find(canvas).GetComponent<Canvas>();
        }


        [UnityTest]
        public IEnumerator TestHomeButton()
        {
            Button homeButton = GameObject.Find("NavbarAndTopBar/NavBar/HomeButton").GetComponent<Button>();
            homeButton.onClick.Invoke();

            // check home canvas is enabled
            Assert.IsTrue(GetCanvas("HomeCanvas").enabled);

            // check other canvases are not enabled
            Assert.IsFalse(GetCanvas("CameraCanvas").enabled);
            Assert.IsFalse(GetCanvas("MapCanvas").enabled);
            Assert.IsFalse(GetCanvas("InfoCanvas").enabled);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestCameraButton()
        {
            Button cameraButton = GameObject.Find("NavbarAndTopBar/NavBar/CameraButton").GetComponent<Button>();
            cameraButton.onClick.Invoke();

            // check camera canvas is enabled
            Assert.IsTrue(GetCanvas("CameraCanvas").enabled);

            // check other canvases are not enabled
            Assert.IsFalse(GetCanvas("HomeCanvas").enabled);
            Assert.IsFalse(GetCanvas("MapCanvas").enabled);
            Assert.IsFalse(GetCanvas("InfoCanvas").enabled);

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestMapButton()
        {
            Button mapButton = GameObject.Find("NavbarAndTopBar/NavBar/MapButton").GetComponent<Button>();
            mapButton.onClick.Invoke();

            // check map canvas is enabled
            Assert.IsTrue(GetCanvas("MapCanvas").enabled);

            // check other canvases are not enabled
            Assert.IsFalse(GetCanvas("CameraCanvas").enabled);
            Assert.IsFalse(GetCanvas("HomeCanvas").enabled);
            Assert.IsFalse(GetCanvas("InfoCanvas").enabled);

            yield return null;
        }


        [UnityTest]
        public IEnumerator TestInfoButton()
        {
            Button infoButton = GameObject.Find("NavbarAndTopBar/NavBar/InfoButton").GetComponent<Button>();
            infoButton.onClick.Invoke();

            // check info canvas is enabled
            Assert.IsTrue(GetCanvas("InfoCanvas").enabled);

            // check other canvases are not enabled
            Assert.IsFalse(GetCanvas("CameraCanvas").enabled);
            Assert.IsFalse(GetCanvas("MapCanvas").enabled);
            Assert.IsFalse(GetCanvas("HomeCanvas").enabled);

            yield return null;
        }
    }


