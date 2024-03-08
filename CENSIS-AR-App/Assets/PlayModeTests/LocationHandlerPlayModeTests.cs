using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using CENSIS.Runtime;
using Location = CENSIS.Locations.Location;

public class LocationHandlerPlayModeTests
{
    [UnitySetUp]
    IEnumerator SetUp()
    {
        SceneManager.LoadScene("Assets/Scenes/AppScene.unity");
        yield return null;
    }

    [UnityTest]
    public IEnumerator testNextLocation()
    {
        LocationHandler.LocationIndex = 0;
        LocationHandler.locations = new List<Location>();
        Location loc1 = new Location(
            "testLocation",
            "clue text",
            "information",
            new float[] { 55.87394f, -4.29181f },
            new float[][][]
            {
                new float[][]
                {
                    new float[] { 55.6829478f, -4.5160826f },
                    new float[] { 55.6830784f, -4.5155368f },
                    new float[] { 55.6827737f, -4.5153076f },
                    new float[] { 55.6826432f, -4.5158534f },
                    new float[] { 55.6829478f, -4.5160826f },
                }
            },
            new float[][][]
            {
                new float[][]
                {
                    new float[] { 55.6827422f, -4.5150968f },
                    new float[] { 55.6831358f, -4.5154564f },
                    new float[] { 55.6829316f, -4.5165759f },
                    new float[] { 55.6824903f, -4.5160380f },
                    new float[] { 55.6827422f, -4.5150968f }
                }
            }
        );
        Location loc2 = new Location(
            "testLocation2",
            "clue text2",
            "information2",
            new float[] { 55.87394f, -4.29181f },
            new float[][][]
            {
                new float[][]
                {
                    new float[] { 55.6829478f, -4.5160826f },
                    new float[] { 55.6830784f, -4.5155368f },
                    new float[] { 55.6827737f, -4.5153076f },
                    new float[] { 55.6826432f, -4.5158534f },
                    new float[] { 55.6829478f, -4.5160826f },
                }
            },
            new float[][][]
            {
                new float[][]
                {
                    new float[] { 55.6827422f, -4.5150968f },
                    new float[] { 55.6831358f, -4.5154564f },
                    new float[] { 55.6829316f, -4.5165759f },
                    new float[] { 55.6824903f, -4.5160380f },
                    new float[] { 55.6827422f, -4.5150968f }
                }
            }
        );

        LocationHandler.locations.Add(loc1);
        LocationHandler.locations.Add(loc2);

        LocationHandler.NextLocation();

        Assert.AreEqual(LocationHandler.GetCurrLocation(), loc2);
        yield return null;
    }
}
