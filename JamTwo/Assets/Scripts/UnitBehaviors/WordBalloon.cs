using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// This class (and prefab) should be attached to any GameObject
/// that has spoken dialogue in the game.
/// </summary>
public class WordBalloon : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject textObject;
   
    Text m_text;
    Animator m_animator;
    const float m_visibleTime = 4.5f;

    bool m_isVisible = false;

    void Awake()
    {
        m_text = textObject.GetComponent<Text>();
        m_animator = GetComponent<Animator>();
    }

    public void Speak(string message)
    {
        StartCoroutine(DisplayText(message));
    }

    public void Stop()
    {
        if (m_isVisible) m_animator.SetTrigger("disappear");

        m_isVisible = false;
        StopCoroutine(DisplayText(""));
    }

    IEnumerator DisplayText(string message)
    {
        m_isVisible = true;
        m_animator.SetTrigger("appear");
        m_text.text = message;
        yield return new WaitForSeconds(m_visibleTime);
        Stop();
    }
    
}
