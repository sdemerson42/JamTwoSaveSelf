using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;

/// <summary>
/// Master behavioral logic for the Player.
/// </summary>
public class PlayerBehavior : MonoBehaviour
{
    public CapsuleCollider2D soundTrigger;

    public float walkSpeed;
    public float idlePingScale;
    public float walkingPingScale;

    Rigidbody2D m_rigidBody;
    SoundPing m_soundPing;
    Animator m_animator;

    // Start is called before the first frame update
    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_soundPing = GetComponentInChildren<SoundPing>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Default behavior for now: Simple movement
        Walk();
    }

    void Walk()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector2 newVelocity = new Vector2(hInput, vInput).normalized * walkSpeed;
        m_rigidBody.velocity = newVelocity;

        // Adjust ping / trigger

        if (hInput == 0f && vInput == 0f)
        {
            m_soundPing.pingScale = idlePingScale;
            soundTrigger.transform.localScale = new Vector3(
                idlePingScale, idlePingScale, idlePingScale);
            
            m_animator.SetBool("isWalking", false);

        }
        else
        {
            m_soundPing.pingScale = walkingPingScale;
            soundTrigger.transform.localScale = new Vector3(
                walkingPingScale, walkingPingScale, walkingPingScale);

            m_animator.SetBool("isWalking", true);
            var scale = transform.localScale;
            if (hInput < 0f) scale.x = Mathf.Abs(scale.x) * -1f;
            else if (hInput > 0f) scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}
