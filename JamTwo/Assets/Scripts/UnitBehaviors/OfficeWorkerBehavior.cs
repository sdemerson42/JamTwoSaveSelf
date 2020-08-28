using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerBehavior : MonoBehaviour
{
    public float idleWaitToSpeakMin;
    public float idleWaitToSpeakMax;
    public string[] banalities;
    public GameObject interaction;
    public GameObject zombieForm;
    public GameObject[] guardsToAlert;

    // Interaction data

    public string[] openingLine;
    public float baseThinkSuccess;
    public string[] thinkPCLine;
    public string[] thinkWinLine;
    public string[] thinkLoseLine;
    public float baseIntimidateSuccess;
    public string[] intimidatePCLine;
    public string[] intimidateWinLine;
    public string[] intimidateLoseLine;
    public float baseCharmSuccess;
    public string[] charmPCLine;
    public string[] charmWinLine;
    public string[] charmLoseLine;


    public enum OfficeWorkerState
    {
        Asleep,
        Idle
    }

    OfficeWorkerState m_state;
    public OfficeWorkerState State
    {
        get => m_state;
        set
        {
            if (value != m_state)
            {
                m_state = value;
                if (value == OfficeWorkerState.Idle)
                {
                    StopAllCoroutines();
                    StartCoroutine(Idle());
                }
            }
        }
    }

    WordBalloon m_wordBalloon;

    private void Awake()
    {
        m_wordBalloon = transform.parent.GetComponentInChildren<WordBalloon>();
        
        State = OfficeWorkerState.Idle;
    }

    IEnumerator Idle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(
                idleWaitToSpeakMin, idleWaitToSpeakMax));
            string msg = banalities[Random.Range(0, banalities.Length)];
            m_wordBalloon.Speak(msg);
        }
    }
}
