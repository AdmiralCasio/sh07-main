using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoundaryBoxes
{
    void Start()
    {
        Debug.Log("BOUNDARY BOXES SCRIPT STARTED");
    }

    public static bool IsPointInPolygonGPS(Vector2 point, Vector2[] polygon)
    {
        Vector2 pointCart = ConvertToCartesian(point);
        Vector2[] polygonCart = ConvertToCartesian(polygon);
        return IsPointInPolygon(pointCart, polygonCart);
    }
    
    public static bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
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

    public static Vector2[] ConvertToCartesian(Vector2[] latLongs)
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

    public static Vector2 ConvertToCartesian(Vector2 latLong)
    {
        double R = 6371000; // radius of the earth in meters

        double latRad = latLong.x * Math.PI / 180;
        double lonRad = latLong.y * Math.PI / 180;

        double x = R * Math.Cos(latRad) * Math.Cos(lonRad);
        double y = R * Math.Cos(latRad) * Math.Sin(lonRad);
        double z = R * Math.Sin(latRad);

        Vector2 cartesian = new Vector2((float)x, (float)y);
        return cartesian;
    }

    private void IsInsidePrint(bool isInside)
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

}
