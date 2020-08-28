using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

    int m_dialogueIndex;

    const float m_timeToRead = 3f;
    const float m_timeToClose = .5f;
    const float m_humanityWinReward = 20f;
    const float m_humanityLosePenalty = -20f;
    const float m_timeToSeeIconSelection = 2f;

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

        StartCoroutine(Opening());
    }

    string RandomString(string[] ary)
    {
        return ary[Random.Range(0, ary.Length)];
    }

    IEnumerator Opening()
    {
        m_npcBalloon.Stop();
        yield return new WaitForSecondsRealtime(m_timeToClose);
        m_npcBalloon.Speak(RandomString(m_npcBehavior.openingLine));
        yield return new WaitForSecondsRealtime(m_timeToRead);
        m_npcBalloon.Stop();
        yield return new WaitForSecondsRealtime(m_timeToClose);
        StartCoroutine(PlayerResponse());
    }

    IEnumerator PlayerResponse()
    {
        m_player.transform.GetChild(1).gameObject.SetActive(true);
        var interactionMenu = m_player.GetComponentInChildren<InteractionMenuLogic>();
        InteractionMenuLogic.InteractionIcon chosenAction;
        
        // Default spoken line is zombie noises...
        string playerLine = m_playerBehavior.zombieNoises[
            Random.Range(0, m_playerBehavior.zombieNoises.Length)];

        while (true)
        {
            float hInput = Input.GetAxisRaw("MenuHorizontal");
            float vInput = Input.GetAxisRaw("MenuVertical");

            if (hInput == -1f && vInput == 0f)
            {
                // Charm
                chosenAction = InteractionMenuLogic.InteractionIcon.Charm;
                interactionMenu.StartSelectionFX(chosenAction);
                m_dialogueIndex = 
                    Random.Range(0, m_npcBehavior.charmPCLine.Length);
                playerLine = m_npcBehavior.charmPCLine[m_dialogueIndex];
                break;
            }

            if (vInput == -1f && hInput == 0f)
            {
                // Think
                chosenAction = InteractionMenuLogic.InteractionIcon.Think;
                interactionMenu.StartSelectionFX(chosenAction);
                m_dialogueIndex =
                    Random.Range(0, m_npcBehavior.thinkPCLine.Length);
                playerLine = m_npcBehavior.thinkPCLine[m_dialogueIndex];
                break;
            }

            if (hInput == 1f && vInput == 0f)
            {
                // Intimidate
                chosenAction = InteractionMenuLogic.InteractionIcon.Intimidate;
                interactionMenu.StartSelectionFX(chosenAction);
                m_dialogueIndex =
                    Random.Range(0, m_npcBehavior.intimidatePCLine.Length);
                playerLine = m_npcBehavior.intimidatePCLine[m_dialogueIndex];
                break;
            }

            if (vInput == 1f && hInput == 0f)
            {
                // Bite
                chosenAction = InteractionMenuLogic.InteractionIcon.Bite;
                interactionMenu.StartSelectionFX(chosenAction);
                m_dialogueIndex = 0;
                break;
            }
            yield return null;
        }
        yield return new WaitForSecondsRealtime(m_timeToSeeIconSelection);
        interactionMenu.StopSelectionFX();
        m_player.transform.GetChild(1).gameObject.SetActive(false);

        // Word balloon
      
        m_playerBalloon.Speak(playerLine);
        yield return new WaitForSecondsRealtime(m_timeToRead);
        m_playerBalloon.Stop();
        yield return new WaitForSecondsRealtime(m_timeToClose);
       
        EvaluatePlayerResponse(chosenAction);
    }

    void EvaluatePlayerResponse(InteractionMenuLogic.InteractionIcon action)
    {
        // Success formula: Take base success chance for this action
        // and subtract 50% times current zombifcation ratio
        // (calculated by taking 100% - current humanity ratio)

        // NOTE: Bites are always 100% successful for now

        float baseSuccessChance = 1f;
        if (action == InteractionMenuLogic.InteractionIcon.Charm)
            baseSuccessChance = m_npcBehavior.baseCharmSuccess;
        if (action == InteractionMenuLogic.InteractionIcon.Intimidate)
            baseSuccessChance = m_npcBehavior.baseIntimidateSuccess;
        if (action == InteractionMenuLogic.InteractionIcon.Think)
            baseSuccessChance = m_npcBehavior.baseThinkSuccess;

        float successChance = baseSuccessChance;

        if (action != InteractionMenuLogic.InteractionIcon.Bite)
            successChance -= 
                (1f - m_playerBehavior.GetCurrentHumanityRatio()) / 2f;

        bool success = Random.Range(0f, 1f) <= successChance;

        StartCoroutine(EvaluateNPCResponse(action, success));
    }

    IEnumerator EvaluateNPCResponse(InteractionMenuLogic.InteractionIcon action,
        bool success)
    {
        string msg = "";

        if (action == InteractionMenuLogic.InteractionIcon.Bite)
        {
            // Bites always count as a humanity loss
            m_playerBehavior.AddToCurrentHumanity(m_humanityLosePenalty);
            Bite();
            CompleteInteraction();
            yield return null;
        }

        else if (action == InteractionMenuLogic.InteractionIcon.Charm)
        {
            msg = success ? m_npcBehavior.charmWinLine[m_dialogueIndex] :
                m_npcBehavior.charmLoseLine[m_dialogueIndex];
        }

        else if (action == InteractionMenuLogic.InteractionIcon.Intimidate)
        {
            msg = success ? m_npcBehavior.intimidateWinLine[m_dialogueIndex] :
                m_npcBehavior.intimidateLoseLine[m_dialogueIndex];
        }

        else if (action == InteractionMenuLogic.InteractionIcon.Think)
        {
            msg = success ? m_npcBehavior.thinkWinLine[m_dialogueIndex] :
                m_npcBehavior.thinkLoseLine[m_dialogueIndex];
        }

        m_npcBalloon.Speak(msg);
        yield return new WaitForSecondsRealtime(3f);
        m_npcBalloon.Stop();
        yield return new WaitForSecondsRealtime(0.5f);

        // Apply humanity penalty / reward to player

        m_playerBehavior.AddToCurrentHumanity(
            success ? m_humanityWinReward : m_humanityLosePenalty);

        // If success, render this NPC inactive for future interactions
        // and complete.

        if (success)
        {
            CompleteInteraction();
            yield return null;
        }

        // On failure, proceed to sudden-death bite chance...

        StartCoroutine(SuddenDeath());
    }

    IEnumerator SuddenDeath()
    {
        m_player.transform.GetChild(1).gameObject.SetActive(true);
        var interactionMenu = m_player.GetComponentInChildren<InteractionMenuLogic>();
        interactionMenu.SetSuddenDeath();
        float startTime = Time.unscaledTime;
        string playerLine = m_playerBehavior.zombieNoises[
           Random.Range(0, m_playerBehavior.zombieNoises.Length)];

        // Player has three seconds to bite...
        while (true)
        {
            if (Time.unscaledTime - startTime >= 3f)
            {
                // Out of time!
                interactionMenu.StopSelectionFX();
                interactionMenu.gameObject.SetActive(false);

                m_npcBalloon.Speak("HELP!");
                yield return new WaitForSecondsRealtime(m_timeToRead);
                m_npcBalloon.Stop();
                yield return new WaitForSecondsRealtime(m_timeToClose);

                for (int i = 0; i < m_npcBehavior.guardsToAlert.Length; ++i)
                {
                    m_npcBehavior.guardsToAlert[i].GetComponentInChildren<GuardBehavior>()
                        .State = GuardBehavior.GuardState.Panic;
                }

                CompleteInteraction();
                break;
            }

            float vInput = Input.GetAxisRaw("MenuVertical");
            if (vInput == 1f)
            {
                interactionMenu.StopSelectionFX();
                interactionMenu.StartSelectionFX(InteractionMenuLogic.InteractionIcon.Bite);
                yield return new WaitForSecondsRealtime(m_timeToSeeIconSelection);
                interactionMenu.StopSelectionFX();

                interactionMenu.gameObject.SetActive(false);
                m_playerBalloon.Speak(playerLine);
                yield return new WaitForSecondsRealtime(m_timeToRead);
                m_playerBalloon.Stop();
                yield return new WaitForSecondsRealtime(m_timeToClose);
                EvaluatePlayerResponse(InteractionMenuLogic.InteractionIcon.Bite);
                break;
            }
            yield return null;
        }
    }

    void Bite()
    {
        var position = m_npc.transform.position;
        var zombie = m_npcBehavior.zombieForm;
        Instantiate(zombie, position, Quaternion.identity);
        Destroy(m_npc);
    }

    void CompleteInteraction()
    {
        m_playerBalloon.gameObject.GetComponent<Animator>().updateMode =
            AnimatorUpdateMode.Normal;
        m_npcBalloon.gameObject.GetComponent<Animator>().updateMode =
           AnimatorUpdateMode.Normal;

        m_player.GetComponentInChildren<SoundPing>().gameObject.
            GetComponent<SpriteRenderer>().enabled = true;

        m_npc.transform.GetChild(2).GetComponent<CapsuleCollider2D>()
               .enabled = false;

        GameManager.instance.Unpause();

        Destroy(gameObject);
    }
}
