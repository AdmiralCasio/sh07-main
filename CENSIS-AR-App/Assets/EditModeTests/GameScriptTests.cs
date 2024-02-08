using System.Collections;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class GameScriptTests
{
    [SetUp]
    public void Setup()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/AppScene.unity");
    }

    

}
