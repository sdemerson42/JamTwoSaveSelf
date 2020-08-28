using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

/// <summary>
/// Master behavioral logic for the Player.
/// </summary>
public class PlayerBehavior : MonoBehaviour
{
    public CapsuleCollider2D soundTrigger;
    public Transform interactionsTransform;

    public float walkSpeed;
    public float idlePingScale;
    public float walkingPingScale;
    public float maxPingIncrease;
    public float startingHumanity;
    public float humanityLossRate;
    public string[] zombieNoises;

    Rigidbody2D m_rigidBody;
    SoundPing m_soundPing;
    Animator m_animator;
    WordBalloon m_wordBalloon;
    SpriteRenderer m_spriteRenderer;

    float m_humanity;

    // Start is called before the first frame update
    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_soundPing = GetComponentInChildren<SoundPing>();
        m_animator = GetComponent<Animator>();
        m_wordBalloon = 
            transform.parent.GetComponentInChildren<WordBalloon>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_humanity = startingHumanity;
    }

    void Start()
    {
        StartCoroutine(LoseHumanity());
    }

    IEnumerator LoseHumanity()
    {
        while (true)

        {
            yield return new WaitForSeconds(1f);
            m_humanity = Mathf.Max(0f, m_humanity - humanityLossRate);

            float humanityRatio = m_humanity / startingHumanity;
            Color color = new Color(humanityRatio, 1f, humanityRatio);
            m_spriteRenderer.color = color;
        }
    }

    public float GetCurrentHumanityRatio()
    {
        return m_humanity / startingHumanity;
    }

    public void AddToCurrentHumanity(float value)
    {
        m_humanity = Mathf.Clamp(m_humanity + value, 0f, startingHumanity);
    }

    // Update is called once per frame
    void Update()
    {
        // Default behavior for now: Simple movement
        Walk();
    }

    void Walk()
    {
        // Ignore consequences of input during
        // pause state

        if (Time.timeScale == 0f) return;

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector2 newVelocity = new Vector2(hInput, vInput).normalized * walkSpeed;
        m_rigidBody.velocity = newVelocity;

        // Adjust ping / trigger

        if (hInput == 0f && vInput == 0f)
        {
            var adjustedScale = idlePingScale + maxPingIncrease *
                (1f - GetCurrentHumanityRatio());
            m_soundPing.pingScale = adjustedScale;
            soundTrigger.transform.localScale = new Vector3(
                adjustedScale, adjustedScale, adjustedScale);
            
            m_animator.SetBool("isWalking", false);

        }
        else
        {
            var adjustedScale = walkingPingScale + maxPingIncrease *
                (1f - GetCurrentHumanityRatio());
            m_soundPing.pingScale = adjustedScale;
            soundTrigger.transform.localScale = new Vector3(
                adjustedScale, adjustedScale, adjustedScale);

            m_animator.SetBool("isWalking", true);
            var scale = transform.localScale;
            if (hInput < 0f) scale.x = Mathf.Abs(scale.x) * -1f;
            else if (hInput > 0f) scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}
