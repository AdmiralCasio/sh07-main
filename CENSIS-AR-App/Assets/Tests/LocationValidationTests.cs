using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools;

public class LocationValidationTests
{

    Location loc = new Location("TestLoc", new Vector2[]{
        new Vector2(55.6829478f,-4.5160826f),
        new Vector2(55.6830784f, -4.5155368f),
        new Vector2(55.6827737f, -4.5153076f),
        new Vector2(55.6826432f, -4.5158534f),
        new Vector2(55.6829478f,-4.5160826f)
    }, new Vector2[]{
        new Vector2(55.6827422f, -4.5150968f),
        new Vector2(55.6831358f, -4.5154564f),
        new Vector2(55.6829316f, -4.5165759f),
        new Vector2(55.6824903f, -4.5160380f),
        new Vector2(55.6827422f, -4.5150968f),
    }, "Clue", "Info");

    [Test]
    public void IsAtLocation()
    {
        Vector3 playerLoc = new Vector3(55.6830365f, -4.5154432f);
        Assert.IsTrue(LocationValidator.AtLocation(playerLoc, loc));
    }

    [Test]
    public void IsNotAtLocation() 
    {
        Vector3 playerLoc = new Vector3(-5, -5);
        Assert.IsFalse(LocationValidator.AtLocation(playerLoc, loc));
    }

    [Test]
    public void InLocation()
    {
        Vector3 playerLoc = new Vector3(1, 3);
        Assert.IsFalse(LocationValidator.AtLocation(playerLoc, loc));
    }

    [Test]
    public void IsLookingAtLocation()
    {
        Vector3 playerLoc = new Vector3(55.6830365f, -4.5154432f);
        Camera cam = Camera.main;
        cam.transform.position = playerLoc;
        loc.centre = new Vector2(55.68286f, -4.51571f);
        cam.transform.LookAt(loc.centre);
        Assert.IsTrue(LocationValidator.LookingAtLocation(playerLoc, loc));
    }

    [Test]
    public void IsNotLookingAtLocation()
    {
        Vector3 playerLoc = new Vector3(55.6830365f, -4.5154432f);
        Camera cam = Camera.main;
        cam.transform.position = playerLoc;
        loc.centre = new Vector2(55.68286f, -4.51571f);
        cam.transform.LookAt(-loc.centre);
        Assert.IsFalse(LocationValidator.LookingAtLocation(playerLoc, loc));
    }

    [Test]
    public void IsLookingButNotAtLocation()
    {
        Vector3 playerLoc = new Vector3(55.68236f, -4.51621f);
        Camera cam = Camera.main;
        cam.transform.position = playerLoc;
        loc.centre = new Vector2(55.68286f, -4.51571f);
        cam.transform.LookAt(loc.centre);
        Assert.IsFalse(LocationValidator.LookingAtLocation(playerLoc, loc));
    }

    //[UnityTest]
    //public IEnumerator LocationValidationTestsWithEnumeratorPasses()
    //{
    //    yield return null;
    //}
}
