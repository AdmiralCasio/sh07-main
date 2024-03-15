using System;
using System.Collections.Generic;
using UnityEngine;

namespace CENSIS.Utility
{
    public static class BoundaryBoxes
    {
        /// <summary>
        /// Determines whether GPS cooridnate <paramref name="point"/> is in <paramref name="polygon"/> expressed in GPS coordinates
        /// </summary>
        /// <param name="point">the point to test</param>
        /// <param name="polygon">the polygon being tested</param>
        /// <param name="origin">the camera origin point in unity space</param>
        /// <returns>A boolean representing whether or not <paramref name="point"/> is in <paramref name="polygon"/></returns>
        public static bool IsPointInPolygonGPS(Vector2 point, Vector2[] polygon, Vector3 origin)
        {
            Vector3 pointCart = ConvertToUnityCartesian(point, origin);
            Vector3[] polygonCart = ConvertToUnityCartesian(polygon, origin);
            return IsPointInPolygon(pointCart, polygonCart);
        }

        /// <summary>
        /// Determines whether <paramref name="point"/> is in <paramref name="polygon"/>
        /// </summary>
        /// <param name="point">the point to test</param>
        /// <param name="polygon">the polygon being tested</param>
        /// <returns>A boolean representing whether or not <paramref name="point"/> is in <paramref name="polygon"/></returns>
        public static bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
        {
            bool inside = false;
            float tolerance = 0.001f; // Add a small tolerance value

            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (
                    (
                        (polygon[i].y <= point.y + tolerance && point.y < polygon[j].y + tolerance)
                        || (
                            polygon[j].y <= point.y + tolerance
                            && point.y < polygon[i].y + tolerance
                        )
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
                    polygon[i].y.Equals(point.y)
                    && polygon[j].y.Equals(point.y)
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

        /// <summary>
        /// Determines whether <paramref name="point"/> is in <paramref name="polygon"/>
        /// </summary>
        /// <param name="point">the point to test</param>
        /// <param name="polygon">the polygon being tested</param>
        /// <returns>A boolean representing whether or not <paramref name="point"/> is in <paramref name="polygon"/></returns>
        private static bool IsPointInPolygon(Vector3 point, Vector3[] polygon)
        {
            bool inside = false;
            float tolerance = 0.001f; // Add a small tolerance value

            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (
                    (
                        (polygon[i].y <= point.y + tolerance && point.y < polygon[j].y + tolerance)
                        || (
                            polygon[j].y <= point.y + tolerance
                            && point.y < polygon[i].y + tolerance
                        )
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
                    polygon[i].y.Equals(point.y)
                    && polygon[j].y.Equals(point.y)
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
        
        /// <summary>
        /// Converts the array of GPS coordinates <paramref name="latlongs"/> to Unity world space coordinates
        /// </summary>
        /// <param name="latlongs">the GPS coordinates to convert</param>
        /// <returns>The array of GPS coordinates <paramref name="latlongs"/> as a an array of <see cref="Vector2"/> in Unity world space</returns>
        public static Vector2[] ConvertToCartesian(Vector2[] latlongs)
        {
            List<Vector2> carts = new List<Vector2>();
            foreach (Vector2 latlong in latlongs)
            {
                carts.Add(ConvertToCartesian(latlong));
            }
            return carts.ToArray();
        }
        /// <summary>
        /// Converts the array of GPS coordinates <paramref name="latLong"/> to Unity world space coordinates
        /// </summary>
        /// <param name="latLong">the GPS coordinates to convert</param>
        /// <returns>The array of GPS coordinates <paramref name="latLong"/> as a an array of <see cref="Vector2"/> in Unity world space</returns>
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

            return new Vector2((float)x, (float)y);
        }

        /// <summary>
        /// Converts the GPS coordinate <paramref name="latLong"/> to Unity world space coordinates including calculated altitude
        /// </summary>
        /// <param name="latLong">the GPS coordinate to be converted</param>
        /// <returns>The GPS coordinate <paramref name="latLong"/> as a <see cref="Vector3"/> in Unity world space</returns>

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
            double z = (b * b) / (a * a) * N * Math.Sin(lat);

            return new Vector3((float)x, (float)z, (float)y);
        }

        /// <summary>
        /// Converts the GPS coordinate <paramref name="latLong"/> to Unity world space coordinates, normalising the value using <paramref name="origin"/> as a zero point
        /// </summary>
        /// <param name="latLong">the GPS coordinate to be converted</param>
        /// <param name="origin">the origin point used to normalise <paramref name="latLong"/></param>
        /// <returns>The GPS coordinate <paramref name="latLong"/> converted to Unity world space and normalised using <paramref name="origin"/></returns>
        public static Vector3 ConvertToUnityCartesian(Vector2 latLong, Vector3 origin)
        {
            return ConvertToUnityCartesian(latLong) - origin;
        }

        /// <summary>
        /// Converts the GPS coordinate <paramref name="latlongs"/> to Unity world space coordinates, normalising the value using <paramref name="origin"/> as a zero point
        /// </summary>
        /// <param name="latlongs">the GPS coordinate to be converted</param>
        /// <param name="origin">the origin point used to normalise <paramref name="latlongs"/></param>
        /// <returns>The GPS coordinate <paramref name="latlongs"/> converted to Unity world space and normalised using <paramref name="origin"/></returns>
        private static Vector3[] ConvertToUnityCartesian(Vector2[] latlongs, Vector3 origin)
        {
            List<Vector3> carts = new List<Vector3>();
            foreach (Vector2 latlong in latlongs)
            {
                carts.Add(ConvertToUnityCartesian(latlong) - origin);
            }

            return carts.ToArray();
        }
    }
}
