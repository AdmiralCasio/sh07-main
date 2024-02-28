using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoundaryBoxes
{
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
            if (
                (
                    (polygon[i].y <= point.y + tolerance && point.y < polygon[j].y + tolerance)
                    || (polygon[j].y <= point.y + tolerance && point.y < polygon[i].y + tolerance)
                )
                && (
                    point.x
                    < (polygon[j].x - polygon[i].x)
                        * (point.y - polygon[i].y)
                        / (polygon[j].y - polygon[i].y)
                        + polygon[i].x
                        + tolerance
                )
            )
            {
                inside = !inside;
            }
            else if (
                polygon[i].y == point.y
                && polygon[j].y == point.y
                && point.x >= Math.Min(polygon[i].x, polygon[j].x) - tolerance
                && point.x <= Math.Max(polygon[i].x, polygon[j].x) + tolerance
            )
            {
                inside = true;
                break; // Exit the loop if the point is on an edge of the polygon
            }
        }
        return inside;
    }

    public static Vector2[] ConvertToCartesian(Vector2[] latlongs)
    {
        List<Vector2> carts = new List<Vector2>();
        foreach (Vector2 latlong in latlongs)
        {
            carts.Add(ConvertToCartesian(latlong));
        }
        return carts.ToArray();
    }

    public static Vector2 ConvertToCartesian(Vector2 latLong)
    {
        // WGS-84 ellipsoid constants
        double a = 6378137; // equatorial radius in meters
        double f = 1 / 298.257223563; // flattening
        double b = a * (1 - f); // polar radius
        double e = Math.Sqrt(1 - (b * b) / (a * a)); // eccentricity

        // input in radians
        double lat = latLong.x * Math.PI / 180;
        double lon = latLong.y * Math.PI / 180;

        // radius of curvature in the prime vertical
        double N = a / Math.Sqrt(1 - e * e * Math.Sin(lat) * Math.Sin(lat));

        // cartesian coordinates
        double x = N * Math.Cos(lat) * Math.Cos(lon);
        double y = N * Math.Cos(lat) * Math.Sin(lon);
        double z = (b * b) / (a * a) * N * Math.Sin(lat);

        return new Vector2((float)x, (float)y);
    }
    public static Vector3 ConvertToUnityCartesian(Vector2 latLong)
    {
        // WGS-84 ellipsoid constants
        double a = 6378137; // equatorial radius in meters
        double f = 1 / 298.257223563; // flattening
        double b = a * (1 - f); // polar radius
        double e = Math.Sqrt(1 - (b * b) / (a * a)); // eccentricity

        // input in radians
        double lat = latLong.x * Math.PI / 180;
        double lon = latLong.y * Math.PI / 180;

        // radius of curvature in the prime vertical
        double N = a / Math.Sqrt(1 - e * e * Math.Sin(lat) * Math.Sin(lat));

        // cartesian coordinates
        double x = N * Math.Cos(lat) * Math.Cos(lon);
        double y = N * Math.Cos(lat) * Math.Sin(lon);
        double z = (b * b) / (a * a) * N * Math.Sin(lat) + 1;

        return new Vector3((float)x, (float)z, (float)y);
    }

    public static Vector3 ConvertToUnityCartesian(Vector2 latLong, Vector3 origin)
    {
        return ConvertToUnityCartesian(latLong) - origin;
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
