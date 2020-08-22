using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSoundCollisions : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Guard")
        {
            var guardBehavior = collision.gameObject.GetComponent<GuardBehavior>();
            guardBehavior.LookAtNoise(gameObject);
        }
    }
}
