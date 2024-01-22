using System;
using UnityEngine;

[Serializable]
public class Location
{
    public struct BoundingBox
    {
        public Vector2[] points;

        public BoundingBox(Vector2[] points)
        {
            this.points = points;
        }
    }

    

    public string name;
    public BoundingBox[] inner;
    public BoundingBox[] outer;
    public string clue;
    public string information;
    public Vector2 centre;

    public Location(String name, BoundingBox[] inner, BoundingBox[] outer, string clue, string information, Vector2 centre)
    {
        this.name = name;
        this.inner = inner;
        this.outer = outer;
        this.clue = clue;
        this.information = information;
        this.centre = centre;
    }
}
