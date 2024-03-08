using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools;
using static UnityEngine.UI.Image;
using CENSIS.Locations;
using CENSIS.Utility;

namespace Tests
{
    public class LocationValidationTests
    {
        Location loc = new Location(
            "TestLoc",
            "Clue",
            "Info",
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
                    new float[] { 55.6827422f, -4.5150968f },
                }
            }
        );
    
        [Test]
        public void IsAtLocation()
        {
            Vector3 playerLoc = new Vector3(55.6830365f, -4.5154432f);
            Assert.IsTrue(LocationValidator.AtLocation(playerLoc, loc, new Vector3(0,0,0)));
        }
    
        [Test]
        public void IsNotAtLocation()
        {
            Vector3 playerLoc = new Vector3(-5, -5);
            Assert.IsFalse(LocationValidator.AtLocation(playerLoc, loc, new Vector3(0,0,0)));
        }
    
        [Test]
        public void InLocation()
        {
            Vector3 playerLoc = new Vector3(55.68291f, -4.51589f);
            Assert.IsFalse(LocationValidator.AtLocation(playerLoc, loc, new Vector3(0,0,0)));
        }
    
        [Test]
        public void IsLookingAtLocation()
        {
            Vector3 playerLoc = new Vector3(55.6830365f, -4.5154432f);
            Vector3 origin = BoundaryBoxes.ConvertToUnityCartesian(playerLoc);
            Camera cam = Camera.main;
            cam.transform.position = BoundaryBoxes.ConvertToUnityCartesian(playerLoc, origin);
            loc.centre = new Vector2(55.68286f, -4.51571f);
            cam.transform.LookAt(BoundaryBoxes.ConvertToUnityCartesian(loc.centre, origin));
            Assert.IsTrue(LocationValidator.LookingAtLocation(playerLoc, loc, origin));
        }
    
        [Test]
        public void IsNotLookingAtLocation()
        {
            Vector3 playerLoc = new Vector3(55.6830365f, -4.5154432f);
            Vector3 origin = BoundaryBoxes.ConvertToUnityCartesian(playerLoc);
            Camera cam = Camera.main;
            cam.transform.position = BoundaryBoxes.ConvertToUnityCartesian(playerLoc, origin);
            loc.centre = new Vector2(55.68286f, -4.51571f);
            cam.transform.LookAt(-loc.centre);
            Assert.IsFalse(LocationValidator.LookingAtLocation(playerLoc, loc, origin));
        }
    
        [Test]
        public void IsLookingButNotAtLocation()
        {
            Vector3 playerLoc = new Vector3(55.68236f, -4.51621f);
            Vector3 origin = BoundaryBoxes.ConvertToUnityCartesian(playerLoc);
            Camera cam = Camera.main;
            cam.transform.position = playerLoc;
            loc.centre = new Vector2(55.68286f, -4.51571f);
            cam.transform.LookAt(loc.centre);
            Assert.IsFalse(LocationValidator.LookingAtLocation(playerLoc, loc, origin));
        }
    
        [Test]
        public void IsLookingAtLocationInLocation()
        {
            Vector3 playerLoc = new Vector3(55.68291f, -4.51589f);
            Vector3 origin = BoundaryBoxes.ConvertToUnityCartesian(playerLoc);
            Camera cam = Camera.main;
            cam.transform.position = playerLoc;
            loc.centre = new Vector2(55.68286f, -4.51571f);
            cam.transform.LookAt(loc.centre);
            Assert.IsFalse(LocationValidator.LookingAtLocation(playerLoc, loc, origin));
        }
    
        [UnityTest]
        public IEnumerator LocationValidationTestsWithEnumeratorPasses()
        {
            yield return null;
        }
    }
}

