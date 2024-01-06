using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoundaryBoxes : MonoBehaviour
{
    public Vector2[] polygonVertices;
    public Transform userLocation;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("BOUNDARY BOXES SCRIPT STARTED");
        Debug.Log("BOUNDARY BOX : CS BUILDING TEST");
        TestPoint();

    }

    public bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
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



    public Vector2[] ConvertToCartesian(Vector2[] latLongs)
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

            cartesians[i] = new Vector2((float)x, (float)y);
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
            new Vector2(55.8740145f,-4.2920211f),
            new Vector2(55.8740176f,-4.2917896f),
            new Vector2(55.8740208f,-4.2915686f),
            new Vector2(55.8739457f,-4.2914601f),
            new Vector2(55.8737967f,-4.2918125f),
            new Vector2(55.8738249f,-4.2918538f),
            new Vector2(55.8737938f,-4.2919222f),
            new Vector2(55.8739423f,-4.2921254f),
            new Vector2(55.8739423f,-4.2921254f),
            new Vector2(55.8739947f,-4.2920999f),
            new Vector2(55.8739961f,-4.2920208f),
            new Vector2(55.8740145f,-4.2920211f)
         };

        //Array.Reverse(examplePolygon);
        Vector2[] exampleCartPolygon = ConvertToCartesian(examplePolygon);
        Vector2[] examplePosition = { new Vector2(55.87393f, -4.29185f) }; 
        Vector2 exampleCartPosition = ConvertToCartesian(examplePosition)[0];
        bool isinside = IsPointInPolygon(exampleCartPosition, exampleCartPolygon);
        IsInsidePrint(isinside);


    }

}
