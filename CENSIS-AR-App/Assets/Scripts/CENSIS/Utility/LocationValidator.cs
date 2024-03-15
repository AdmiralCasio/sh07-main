using CENSIS.Locations;
using UnityEngine;

namespace CENSIS.Utility
{
    public static class LocationValidator
    {
        /// <summary>
        /// Determines whether the given point is within the <see cref="Location.outer"/> and outwith the <see cref="Location.inner"/> boundary box of the given location.
        /// </summary>
        /// <param name="position">position to be tested</param>
        /// <param name="location">the location which contains the boundary box <c>position</c> is to be tested in</param>
        /// <param name="origin">the camera origin point in unity space</param>
        /// <returns>A boolean representing whether <paramref name="position"/> is within the inner and outwith the outer boundary box(es) of <paramref name="location"/></returns>

        public static bool AtLocation(Vector2 position, Location location, Vector3 origin)
        {
            bool inInner = false;
            bool inOuter = false;

            foreach (var box in location.outer)
            {
                if (InBox(position, box.points, origin))
                {
                    inOuter = true;
                }
            }

            foreach (var box in location.inner)
            {
                if (InBox(position, box.points, origin))
                {
                    inInner = true;
                }
            }

            return inOuter && !inInner;
        }

        /// <summary>
        /// Determines whether the user at <c>position</c> is within the outer and outwith the inner boundary boxes of <c>location</c> and is looking at <see cref="Location.centre"/> with <see cref="Camera.main"/>
        /// </summary>
        /// <param name="position">the position of the user</param>
        /// <param name="location">the location to be tested</param>
        /// <param name="origin">the origin point of the Unity world space in GPS coordinates</param>
        /// <param name="cam">Main camera</param>
        /// <returns>A boolean representing whether or not the user at <c>position</c> is looking at the centre of <c>location</c></returns>
        public static bool LookingAtLocation(Vector2 position, Location location, Vector3 origin, Camera cam)
        {
            return AtLocation(position, location, origin)
                && LocationVisibility.IsVisible(
                    BoundaryBoxes.ConvertToUnityCartesian(location.centre, origin),
                    cam
                );
        }

        public static bool InBox(Vector2 position, Vector2[] box, Vector3 origin)
        {
            return BoundaryBoxes.IsPointInPolygonGPS(position, box, origin);
        }
    }
}
