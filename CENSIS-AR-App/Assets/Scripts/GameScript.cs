using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    [SerializeField] string filename;
    int buildingIndex;
    //List<Building> buildings = new List<Building>();

    // Start is called before the first frame update
    void Start()
    {
        //buildings = FileHandler.ReadFromJSON<Building>(filename);

        // gets the index of the current building or 0 
        int buildingIndex = PlayerPrefs.GetInt("CurrentBuilding", 0);
    }

    public void BuildingFound()
    {
        // show info
        // Debug.Log($"Building name: {buildings[buildingIndex].name}, Building info: {buildings[buildingIndex].information}");
        // show next button
    }

    private void Next()
    {
        buildingIndex += 1;

        PlayerPrefs.SetInt("CurrentBuilding", buildingIndex);
        PlayerPrefs.Save();
        // show clue
        ShowClue();
        // Debug.Log($"Building clue: {buildings[buildingIndex].clue});
    }

    private void ShowClue()
    {
        // show clue;
    }

}
