using System;
using UnityEngine;

[Serializable]
public class Location
{
    public struct BoundingBox
    {
        public Vector2[] points;    
    }

    public BoundingBox[] inner;
    public BoundingBox[] outer;
    public string name;
    public string clue;
    public string information;
    public Vector2 centre;

    public Location(String name,string clue, string information, float [][][] inner, float[][][] outer)
    {
        this.name = name;
        this.inner = new BoundingBox[inner.Length];
        
        for (int i = 0; i < inner.Length; i++)
        {
            //Debug.Log("LOCATION INNER length: " + inner.Length);
            //Debug.Log("LOCATION INNER i: " +  i);
            Vector2[] temp = new Vector2[inner[i].Length];
            for (int j = 0; j < inner[i].Length; j++)
            {
                //Debug.Log("LOCATION INNER j: " + j);
                //Debug.Log("LOCATION INNER  i length: " + inner[i].Length);
                //Debug.Log("LOCATION INNER j[]: " + inner[i][j][0] + " , " + inner[i][j][1]);
                temp[j] = new Vector2(inner[i][j][0], inner[i][j][1]);
                //this.inner[i].points[j] = new Vector2(inner[i][j][0], inner[i][j][1]);
            }
            this.inner[i].points = temp;
        }
        

        this.outer = new BoundingBox[outer.Length];
        for (int i = 0; i < outer.Length; i++)
        {
            Vector2[] temp = new Vector2[outer[i].Length];
            for (int j = 0; j < outer[i].Length; j++)
            {
                temp[j] = new Vector2(outer[i][j][0], outer[i][j][1]);
            }
            this.outer[i].points = temp;
        }
        this.clue = clue;
        this.information = information;
        this.centre = centre;

    }
}
