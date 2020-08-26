using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerLogic : MonoBehaviour
{
    public float idleWaitToSpeakMin;
    public float idleWaitToSpeakMax;
    public string[] banalities;

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
