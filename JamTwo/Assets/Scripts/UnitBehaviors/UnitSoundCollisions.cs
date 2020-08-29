using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSoundCollisions : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        switch(gameObject.tag)
        {
            case "Zombie":
                ZombieLogic(collision);
                break;
            case "OfficeWorker":
                OfficeWorkerLogic(collision);
                break;
        }
    }

    private void ZombieLogic(Collider2D collision)
    {
        // ARGH!! Guards and office workers should have
        // inherited from a common base class!!!
        if (collision.tag == "Guard")
        {
            var guardBehavior = collision.gameObject.GetComponent<GuardBehavior>();
            guardBehavior.LookAtNoise(gameObject);
        }

        if (collision.tag == "OfficeWorker")
        {
            var officeWorkerBehavior = collision.gameObject.GetComponent<OfficeWorkerBehavior>()
                .State = OfficeWorkerBehavior.OfficeWorkerState.Panic;
        }
    }

    private void OfficeWorkerLogic(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var interaction = transform.parent.
                GetComponentInChildren<OfficeWorkerBehavior>().interaction;
            var instance = Instantiate(interaction, transform.position, Quaternion.identity);
            instance.GetComponent<Interaction>().Initialize(
                collision.transform.parent.gameObject, transform.parent.gameObject);
            GameManager.instance.Pause();
        }
    }
}
