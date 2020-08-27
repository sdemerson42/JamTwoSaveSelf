using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float conversationWaitTime;

    GameObject m_player;
    GameObject m_npc;
    WordBalloon m_playerBalloon;
    WordBalloon m_npcBalloon;
    OfficeWorkerBehavior m_npcBehavior;
    PlayerBehavior m_playerBehavior;

    public void Initialize(GameObject player, GameObject npc)
    {
        m_player = player;
        m_npc = npc;
        m_playerBalloon = player.GetComponentInChildren<WordBalloon>();
        m_npcBalloon = npc.GetComponentInChildren<WordBalloon>();
        m_playerBehavior = player.GetComponentInChildren<PlayerBehavior>();
        m_npcBehavior = npc.GetComponentInChildren<OfficeWorkerBehavior>();

        // Set word balloons of interacting characters to appropriate update mode
        m_playerBalloon.gameObject.GetComponent<Animator>().updateMode =
            AnimatorUpdateMode.UnscaledTime;
        m_npcBalloon.gameObject.GetComponent<Animator>().updateMode =
           AnimatorUpdateMode.UnscaledTime;

        // Turn off player's sound ping

        m_player.GetComponentInChildren<SoundPing>().gameObject.
            GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Start()
    {
        // NOTE: timeScale is set to 0f during interaction, so
        // coroutines won't work. All logic must be handled
        // via update calls, and animations must be set to
        // the appropriate update mode to function during the interaction.

        // Plan: Make sequence work via chained coroutines.

        StartCoroutine(PhaseOne());
    }

    string RandomString(string[] ary)
    {
        return ary[Random.Range(0, ary.Length)];
    }

    IEnumerator PhaseOne()
    {
        m_npcBalloon.Stop();
        yield return new WaitForSecondsRealtime(0.5f);
        m_npcBalloon.Speak(RandomString(m_npcBehavior.openingLine));
        yield return new WaitForSecondsRealtime(3f);
        m_npcBalloon.Stop();
    }
}
