using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    [SerializeField] string filename;
    //List<Building> buildings = new List<Building>();

    // Start is called before the first frame update
    void Start()
    {
        //buildings = FileHandler.ReadFromJSON<Building>(filename);
    }

}
