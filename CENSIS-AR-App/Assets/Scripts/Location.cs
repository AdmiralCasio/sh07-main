using System;
using UnityEngine;

[Serializable]
public class Location
{
    public struct BoundingBox
    {
        public Vector2[] inner;
        public Vector2[] outer;
    }

    public string name;
    public BoundingBox boundingBox;
    public string clue;
    public string information;
    public Vector2 centre;

    public Location(String name, Vector2[] inner, Vector2[] outer, string clue, string information)
    {
        this.name = name;
        this.boundingBox.inner = inner;
        this.boundingBox.outer = outer;
        this.clue = clue;
        this.information = information;
        this.centre = centre;
    }
}
