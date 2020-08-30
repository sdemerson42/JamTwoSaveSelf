using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

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

    public Text loseText;
    public Light2D globalLight;

    public static GameManager instance;
    GameObject m_player;

    private void Awake()
    {
        // Singleton check
        if (instance == null) instance = this;
        else Destroy(gameObject);

        m_player = GameObject.Find("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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

    public void LoseGame(string msg)
    {
        loseText.text = msg;
        Pause();
        StartCoroutine(FadeAndRestart());
    }

    IEnumerator FadeAndRestart()
    {
        while(true)
        {
            globalLight.intensity -= .003f;
            if (globalLight.intensity <= 0f)
            {
                Unpause();
                SceneManager.LoadSceneAsync("Main");
            }
            yield return true;
        }
    }

}
