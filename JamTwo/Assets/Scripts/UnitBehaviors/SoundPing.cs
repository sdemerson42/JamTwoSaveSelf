using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

/// <summary>
/// Sound detection ping for Player. Public values can
/// be modified by other scripts if we want noise to
/// scale with other activities.
/// </summary>
public class SoundPing : MonoBehaviour
{
    public float pingScale;
    public float scaleGrowthRate;
    public float secondsBetweenPings;

   
    void Awake()
    {
        StartPing();
    }

    public void StartPing()
    {
        StartCoroutine(Ping());
    }

    public void StopPing()
    {
        transform.localScale = Vector3.zero;
        StopCoroutine(Ping());
    }

    IEnumerator Ping()
    {
        while (true)
        {
            while (transform.localScale.x < pingScale)
            {
                float uniformScaleValue = scaleGrowthRate * Time.deltaTime;
                transform.localScale += new Vector3(
                    uniformScaleValue, uniformScaleValue, uniformScaleValue);
                yield return null;
            }
            transform.localScale = Vector3.zero;
            yield return new WaitForSeconds(secondsBetweenPings);
        }
    }
}
