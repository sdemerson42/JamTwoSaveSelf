using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Standard GameManager-type script that will assist in coordinating
/// higher-level events. Can be accessed via a static singleton reference.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    static GameObject m_player; 

    private void Awake()
    {
        // Singleton check
        if (instance == null) instance = this;
        else Destroy(gameObject);

        m_player = GameObject.Find("Player");
    }

    public static void Pause()
    {
        Time.timeScale = 0f;
    }

    public static void Unpause()
    {
        Time.timeScale = 1f;
    }

}
