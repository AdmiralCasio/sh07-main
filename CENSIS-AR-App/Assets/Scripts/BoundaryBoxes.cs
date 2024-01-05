using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoundaryBoxes : MonoBehaviour
{
    public Vector3[] polygonVertices;
    public Transform userLocation;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("BOUNDARY BOXES SCRIPT STARTED");
        Debug.Log("BOUNDARY BOX : CS BUILDING TEST");
        TestPoint();
        Debug.Log("BOUNDARY BOX : EXTRA TEST CASES");
        Vector2[] polygon = new Vector2[] { new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10), new Vector2(10, 0) };
        Vector2 point1 = new Vector2(5, 5);
        Vector2 point2 = new Vector2(15, 5);
        Vector2 point3 = new Vector2(0, 0);
        Vector2 point4 = new Vector2(10, 15);
        Vector2 point5 = new Vector2(5, 10);
        Vector2 point6 = new Vector2(5, 0);

        bool result1 = IsPointInPolygon(point1, polygon); // returns true
        bool result2 = IsPointInPolygon(point2, polygon); // returns false
        bool result3 = IsPointInPolygon(point3, polygon); // returns true
        bool result4 = IsPointInPolygon(point4, polygon); // returns false
        bool result5 = IsPointInPolygon(point5, polygon); // returns true
        bool result6 = IsPointInPolygon(point6, polygon); // returns true

        IsInsidePrint(result1); // true
        IsInsidePrint(result2); // false
        IsInsidePrint(result3); // true
        IsInsidePrint(result4); // false
        IsInsidePrint(result5); // true
        IsInsidePrint(result6); // true

    }

    bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    {
        bool inside = false;
        float tolerance = 0.001f; // Add a small tolerance value

        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            if (((polygon[i].y <= point.y + tolerance && point.y < polygon[j].y + tolerance) || (polygon[j].y <= point.y + tolerance && point.y < polygon[i].y + tolerance)) &&
                (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x + tolerance))
            {
                inside = !inside;
            }
            else if (polygon[i].y == point.y && polygon[j].y == point.y && point.x >= Math.Min(polygon[i].x, polygon[j].x) - tolerance && point.x <= Math.Max(polygon[i].x, polygon[j].x) + tolerance)
            {
                inside = true;
                break; // Exit the loop if the point is on an edge of the polygon
            }
        }
        return inside;
    }



    Vector2[] ConvertToCartesian(Vector2[] latLongs)
    {
        double R = 6371000; // radius of the earth in meters
        int n = latLongs.Length;
        Vector2[] cartesians = new Vector2[n];

        for (int i = 0; i < n; i++)
        {
            double latRad = latLongs[i].x * Math.PI / 180;
            double lonRad = latLongs[i].y * Math.PI / 180;

            double x = R * Math.Cos(latRad) * Math.Cos(lonRad);
            double y = R * Math.Cos(latRad) * Math.Sin(lonRad);
            double z = R * Math.Sin(latRad);

            cartesians[i] = new Vector2((float)x, (float)z);
        }

        return cartesians;
    }

    void IsInsidePrint(bool isInside)
    {
        if (isInside)
        {
            Debug.Log("BOUNDARY BOX : User is within this boundary box");
        }
        else
        {
            Debug.Log("BOUNDARY BOX: User is not within this boundary box");
        }
    }

    void TestPoint()
    {
        Vector2[] examplePolygon = new Vector2[]
         {
            new Vector2(55.873973596942015f, -4.292135331723896f),
            new Vector2(55.873782764067884f, -4.291896762586177f),
            new Vector2(55.87362637209446f, -4.291949588730184f),
            new Vector2(55.87350749483508f, -4.292231220666218f),
            new Vector2(55.87369408682638f, -4.292416293081326f),
            new Vector2(55.87373772514658f, -4.292349237858461f),
            new Vector2(55.87387014457666f, -4.292475301677447f),
            new Vector2(55.87394086840546f, -4.292335826813887f),
            new Vector2(55.87413197257516f, -4.292633552003409f),
            new Vector2(55.87456834865173f, -4.292193669741016f),
            new Vector2(55.87448634041885f, -4.291720259867589f),
            new Vector2(55.87443653399497f, -4.291489738037023f),
            new Vector2(55.87430426510995f, -4.291064433617194f),
            new Vector2(55.873988164231972f, -4.291438104850677f),
            new Vector2(55.874039879970475f, -4.291520556493378f),
            new Vector2(55.874035288127324f, -4.292075245593535f)
         };
        //Array.Reverse(examplePolygon);
        Vector2[] exampleCartPolygon = ConvertToCartesian(examplePolygon);
        Vector2[] examplePosition = { new Vector2(55.873782764067884f, -4.291896762586167f) }; // 55.873782764067884, -4.291896762586167
        Vector2 exampleCartPosition = ConvertToCartesian(examplePosition)[0];
        bool isinside = IsPointInPolygon(exampleCartPosition, exampleCartPolygon);
        IsInsidePrint(isinside);


    }

}
