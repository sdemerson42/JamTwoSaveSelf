using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    GameObject m_player;
    GameObject m_npc;
    WordBalloon m_playerBalloon;
    WordBalloon m_npcBalloon;
    InteractionMenuLogic m_playerMenu;

    int m_stateCounter;

    public void Initialize(GameObject player, GameObject npc)
    {
        m_player = player;
        m_npc = npc;
        m_playerBalloon = player.GetComponentInChildren<WordBalloon>();
        m_npcBalloon = npc.GetComponentInChildren<WordBalloon>();

        // Get reference to player's interaction menu
        m_player.transform.GetChild(1).gameObject.SetActive(true);
        m_player.GetComponentInChildren<InteractionMenuLogic>();

        // Set word balloons of interacting characters to appropriate update mode
        m_playerBalloon.gameObject.GetComponent<Animator>().updateMode =
            AnimatorUpdateMode.UnscaledTime;
        m_npcBalloon.gameObject.GetComponent<Animator>().updateMode =
           AnimatorUpdateMode.UnscaledTime;

        // Turn off player's sound ping

        m_player.GetComponentInChildren<SoundPing>().gameObject.
            GetComponent<SpriteRenderer>().enabled = false;

        m_stateCounter = 0;
    }

    private void Update()
    {
        // NOTE: timeScale is set to 0f during interaction, so
        // coroutines won't work. All logic must be handled
        // via update calls, and animations must be set to
        // the appropriate update mode to function during the interaction.

        // A local state counter will be used as a crude state machine.

        if (m_stateCounter == 0)
        {
            ++m_stateCounter;
            m_npcBalloon.Speak("HELLO!");
        }


    }
}
