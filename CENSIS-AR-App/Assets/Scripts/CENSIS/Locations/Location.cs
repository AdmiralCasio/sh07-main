using System;
using UnityEngine;

namespace CENSIS.Locations
{
    [Serializable]
    public class Location
    {
        public struct BoundingBox
        {
            public Vector2[] points { get; set; }
        }

        public BoundingBox[] inner { get; }
        public BoundingBox[] outer { get; }
        public string name { get; }
        public string clue { get; }
        public string information { get; }
        public Vector2 centre { get; set; }

        public Location(
            String name,
            string clue,
            string information,
            float[] centre,
            float[][][] inner,
            float[][][] outer
        )
        {
            this.name = name;
            this.inner = new BoundingBox[inner.Length];

            for (int i = 0; i < inner.Length; i++)
            {
                Vector2[] temp = new Vector2[inner[i].Length];
                for (int j = 0; j < inner[i].Length; j++)
                {
                    temp[j] = new Vector2(inner[i][j][0], inner[i][j][1]);
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
            this.centre = new Vector2(centre[0], centre[1]);
        }
    }
}
