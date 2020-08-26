using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

/// <summary>
/// Standard GameManager-type script that will assist in coordinating
/// higher-level events. Can be accessed via a static singleton reference.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Navigation data

    [System.Serializable]
    public struct Waypoint
    {
        public int id;
        public Transform transform;
        public int[] connections;
    }

    public List<Waypoint> waypoints = new List<Waypoint>();

    public static GameManager instance;
    GameObject m_player;

    private void Awake()
    {
        // Singleton check
        if (instance == null) instance = this;
        else Destroy(gameObject);

        m_player = GameObject.Find("Player");
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
    }

    public Waypoint GetRandomPatrolPoint(int id)
    {
        // NOTE: For simplicity we aren't checking the
        // result of the predicate here; since Waypoint
        // is a struct if the searched-for object doesn't
        // exist the result will be a default Waypoint.

        var result = waypoints.Find(x => x.id == id);
        int newIndex = result.connections[Random.Range(0, result.connections.Length)];
        var newWaypoint = waypoints.Find(x => x.id == newIndex);
        return newWaypoint;
    }

    public int GetCurrentPatrolPointId(Vector3 destination)
    {
        var result = waypoints.Find(x => x.transform.position == destination);
        return result.id;
    }

    public Vector3 GetPatrolPoint(int id)
    {
        var result = waypoints.Find(x => x.id == id);
        return result.transform.position;
    }

}
