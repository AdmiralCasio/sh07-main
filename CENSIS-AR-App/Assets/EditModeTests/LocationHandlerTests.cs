using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using CENSIS.Runtime;
using CENSIS.Locations;

namespace EditModeTests
{
    public class LocationHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/AppScene.unity");
        }
    
        [Test]
        public void IsFinalLocation_WhenIsNotFinal_ReturnsFalse()
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
    
            Assert.IsFalse(LocationHandler.IsFinalLocation());
        }
    
        [Test]
        public void IsFinalLocation_WhenIsFinal_ReturnsTrue()
        {
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
    
            LocationHandler.LocationIndex = 1;
    
            Assert.IsTrue(LocationHandler.IsFinalLocation());
        }
    
        [Test]
        public void GetCurrLocation_ReturnsCurrentLocation()
        {
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
    
            LocationHandler.LocationIndex = 0;
            Assert.IsTrue(LocationHandler.GetCurrLocation() == loc1);
    
            LocationHandler.LocationIndex = 1;
            Assert.IsTrue(LocationHandler.GetCurrLocation() == loc2);
        }
    }

}

