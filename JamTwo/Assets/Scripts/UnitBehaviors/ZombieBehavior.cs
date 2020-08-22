using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Basic behavior for zombies. Should wander aimlessly around
/// the map, attempting to avoid walls but not always succeeding.
/// </summary>
public class ZombieBehavior : MonoBehaviour
{
    public float walkSpeed;
    public float minSecondsToWalk;
    public float maxSecondsToWalk;
    public float minSecondsToWait;
    public float maxSecondsToWait;

    const float m_wallCastDistance = 5f;
   
    Rigidbody2D m_rigidBody;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(Wander());
    }

    IEnumerator Wander()
    {
        while (true)
        {
            float waitTime = Random.Range(minSecondsToWait, maxSecondsToWait);
            yield return new WaitForSeconds(waitTime);
            var direction = CalculateNewWalkDirection();
            float walkTime = Random.Range(minSecondsToWalk, maxSecondsToWalk);
            m_rigidBody.velocity = direction.normalized * walkSpeed;
            yield return new WaitForSeconds(walkTime);
        }
    }

    Vector2 CalculateNewWalkDirection()
    {
        // 1 in 5 chance of deliberately walking into a wall

        var wanderRoll = Random.Range(0, 5);

        // Choose a direction
        
        Vector2 walkDirection = new Vector2(0f, 0f);
        int mask = LayerMask.GetMask("Mapwalls");

        // Attempt to find a clear path, unless we've
        // deliberately chosen a wall collision...

        const int maxTries = 10;
        for (int i = 0; i < maxTries; ++i)
        {
            walkDirection.x = Random.Range(-1f, 1f);
            walkDirection.y = Random.Range(-1f, 1f);
            var result = Physics2D.Raycast(
                transform.position, walkDirection, m_wallCastDistance, mask);
            if (result)
            {
                // If we're intentionally hitting a wall, break here.
                // Keep trying otherwise.
                if (wanderRoll == 0) break;
            }
            else
            {
                if (wanderRoll != 0) break;
            }
        }

        return walkDirection;
    }

}
