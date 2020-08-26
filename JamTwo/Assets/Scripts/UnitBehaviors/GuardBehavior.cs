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

    public float maxSightDistance;

    public enum GuardState
    {
        Asleep, Patrol
    }

    Rigidbody2D m_rigidBody;
    Animator m_animator;
    WordBalloon m_wordBalloon;

    GuardState m_behaviorState;
    public GuardState State
    {
        get => m_behaviorState;
        set
        {
            if (value != m_behaviorState)
            {
                m_behaviorState = value;
                if (value == GuardState.Patrol)
                {
                    StopAllCoroutines();
                    m_patrolPointIndex = 0;
                    m_animator.SetBool("isWalking", true);

                    StartCoroutine(Patrol());
                }
            }
        }
    }

    int m_patrolPointIndex;
    const float m_turningDistance = .1f;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_wordBalloon = transform.parent.GetComponentInChildren<WordBalloon>();

        // Initialize behavior

        State = GuardState.Patrol;
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

            var scale = transform.localScale;
            float absX = Mathf.Abs(scale.x);
            if (moveVector.x < 0f) scale.x = absX * -1f;
            else if (moveVector.x > 0f) scale.x = absX;
            transform.localScale = scale;

            yield return null;
        }
    }

    public void LookAtNoise(GameObject noise)
    {
        var noisePosition = noise.transform.position;
        var direction = noisePosition - transform.position;
        int mask = LayerMask.GetMask("Mapwalls");
        float distance = Mathf.Min(maxSightDistance, Vector3.Distance(
            noisePosition, transform.position));
        var result = Physics2D.Raycast(transform.position, direction, distance, mask);

        // Exit immediately if ray hits a wall.
        if (result) return;

       // Behavior upon spotting Player / Zombie goes here

    }
}
