using UnityEngine;
using System.Collections.Generic;

namespace SesiDefense.Maps
{
    /// <summary>
    /// Manages the pathfinding system for enemies on the map.
    /// Defines waypoints that enemies follow from spawn to base.
    /// </summary>
    public class PathController : MonoBehaviour
    {
        [SerializeField] private List<Transform> waypoints = new List<Transform>();
        [SerializeField] private float waypointReachDistance = 0.5f;
        [SerializeField] private bool loopPath = false;

        private int waypointCount = 0;

        private void OnEnable()
        {
            if (waypoints.Count == 0)
            {
                CollectWaypoints();
            }
            waypointCount = waypoints.Count;
        }

        /// <summary>
        /// Collects waypoints from child transforms.
        /// </summary>
        public void CollectWaypoints()
        {
            waypoints.Clear();
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Waypoint"))
                {
                    waypoints.Add(child);
                }
            }
            
            Logger.Log($"PathController collected {waypoints.Count} waypoints", LogLevel.Info);
        }

        /// <summary>
        /// Gets the next waypoint for an enemy to move toward.
        /// </summary>
        public Vector3 GetNextWaypoint(Vector3 currentPosition)
        {
            if (waypointCount == 0)
            {
                return currentPosition;
            }

            // Find closest waypoint
            int currentIndex = FindClosestWaypointIndex(currentPosition);
            int nextIndex = currentIndex + 1;

            if (nextIndex >= waypointCount)
            {
                if (loopPath)
                {
                    nextIndex = 0;
                }
                else
                {
                    return waypoints[waypointCount - 1].position; // Reached end
                }
            }

            return waypoints[nextIndex].position;
        }

        /// <summary>
        /// Finds the closest waypoint index to a position.
        /// </summary>
        private int FindClosestWaypointIndex(Vector3 position)
        {
            int closestIndex = 0;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < waypointCount; i++)
            {
                float distance = Vector3.Distance(position, waypoints[i].position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        /// <summary>
        /// Gets the final waypoint (base location).
        /// </summary>
        public Vector3 GetBasePath()
        {
            if (waypointCount > 0)
            {
                return waypoints[waypointCount - 1].position;
            }
            return Vector3.zero;
        }

        /// <summary>
        /// Gets all waypoints.
        /// </summary>
        public List<Vector3> GetAllWaypoints()
        {
            List<Vector3> positions = new List<Vector3>();
            foreach (Transform waypoint in waypoints)
            {
                positions.Add(waypoint.position);
            }
            return positions;
        }
    }
}
