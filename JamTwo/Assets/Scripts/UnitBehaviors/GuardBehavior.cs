using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AI for Guard-type characters.
/// </summary>
public class GuardBehavior : MonoBehaviour
{

    [System.Serializable]
    public struct PatrolData
    {
        public Transform transform;
        public float waitTime;
    };

    public float patrolSpeed;
    public PatrolData[] patrolPoints;

    public enum GuardStates
    {
        Patrol
    }

    Rigidbody2D m_rigidBody;
    Animator m_animator;
    WordBalloon m_wordBalloon;

    GuardStates m_behaviorState;
    int m_patrolPointIndex;
    const float m_turningDistance = .1f;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_wordBalloon = GetComponentInChildren<WordBalloon>();

        // Initialize behavior

        SetBehaviorState(GuardStates.Patrol);
    }

    public void SetBehaviorState(GuardStates state)
    {
        if (state == GuardStates.Patrol)
        {
            m_patrolPointIndex = 0;
            m_animator.SetBool("isWalking", true);

            StartCoroutine(Patrol());
        }

        m_behaviorState = state;
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            var destination = patrolPoints[m_patrolPointIndex].transform.position;
            var distanceFromDestination =
                Vector3.Distance(destination, transform.position);
            if (distanceFromDestination <= m_turningDistance)
            {
                // Wait for specified amount of time, then set new destination
                m_animator.SetBool("isWalking", false);
                m_rigidBody.velocity = Vector2.zero;

                // TEMP CODE
                m_wordBalloon.Speak("Everyone finds us so intimidating. Nobody ever says, \"Wow, nice gun!\"");
                // END TEMP

                yield return new WaitForSeconds(patrolPoints[m_patrolPointIndex].waitTime);

                m_patrolPointIndex = (m_patrolPointIndex + 1) % patrolPoints.Length;
                destination = patrolPoints[m_patrolPointIndex].transform.position;
                m_animator.SetBool("isWalking", true);
            }

            // For now, calculate movement vector each frame in case Guard
            // is bumped off path. Ideally there shouldn't be much turbulence,
            // but for now use a linear patrol approach.

            var moveVector = (destination - transform.position).normalized *
                patrolSpeed;
            m_rigidBody.velocity = moveVector;

            yield return null;
        }
    }
}
