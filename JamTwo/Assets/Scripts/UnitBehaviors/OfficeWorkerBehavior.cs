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
    public float runSpeed;
    public Transform firstPanicPoint;

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

    int m_panicPointId;
    const float m_turningDistance = .1f;

    Rigidbody2D m_rigidBody;

    public enum OfficeWorkerState
    {
        Asleep,
        Idle,
        Panic
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
                if (value == OfficeWorkerState.Panic)
                {
                    StopAllCoroutines();
                    m_wordBalloon.gameObject.SetActive(false);
                    transform.parent.GetChild(2).gameObject
                        .SetActive(false);
                    GetComponent<Animator>().SetTrigger("isRunning");
                    StartCoroutine(Panic());
                }
            }
        }
    }

    WordBalloon m_wordBalloon;

    private void Awake()
    {
        m_wordBalloon = transform.parent.GetComponentInChildren<WordBalloon>();
        m_rigidBody = GetComponent<Rigidbody2D>();
        
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

    IEnumerator Panic()
    {
        m_panicPointId = GameManager.instance.GetCurrentPatrolPointId(
            firstPanicPoint.position);

        while (true)
        {
            var destination = GameManager.instance.GetPatrolPoint(m_panicPointId);
            var distanceFromDestination =
                Vector3.Distance(destination, transform.position);
            if (distanceFromDestination <= m_turningDistance)
            {
                m_panicPointId =
                    GameManager.instance.GetRandomPatrolPoint(m_panicPointId).id;
                destination = GameManager.instance.GetPatrolPoint(m_panicPointId);
            }

            var moveVector = (destination - transform.position).normalized *
                runSpeed;
            m_rigidBody.velocity = moveVector;

            var scale = transform.localScale;
            float absX = Mathf.Abs(scale.x);
            if (moveVector.x < 0f) scale.x = absX * -1f;
            else if (moveVector.x > 0f) scale.x = absX;
            transform.localScale = scale;
            yield return null;
        }
    }
}
