using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace PlayModeTests
{
    public class NavBarPlayModeTests
    {
        [UnitySetUp]
        public IEnumerator Setup()
        {
            SceneManager.LoadScene("Assets/Scenes/AppScene.unity");
            yield return null;
        }
    
        [UnityTest]
        public IEnumerator ActivateCanvas_OnStartup_HomeActiveOthersInactive()
        {
            // check home canvas is enabled
            Assert.IsTrue(GameObject.Find("HomeCanvas").GetComponent<Canvas>().enabled);
    
            // check other canvases are not enabled
            Assert.IsFalse(GameObject.Find("MapCanvas").GetComponent<Canvas>().enabled);
            Assert.IsFalse(GameObject.Find("InfoCanvas").GetComponent<Canvas>().enabled);
            Assert.IsFalse(GameObject.Find("CameraCanvas").GetComponent<Canvas>().enabled);
    
            yield return null;
        }
    }

}
