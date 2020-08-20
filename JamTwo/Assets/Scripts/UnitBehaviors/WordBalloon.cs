using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WordBalloon : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject textObject;
   
    Text m_text;
    Animator m_animator;
    const float m_visibleTime = 4.5f;

    void Awake()
    {
        m_text = textObject.GetComponent<Text>();
        m_animator = GetComponent<Animator>();
    }

    public void Speak(string message)
    {
        StartCoroutine(DisplayText(message));
    }

    IEnumerator DisplayText(string message)
    {
        m_animator.SetTrigger("appear");
        m_text.text = message;
        yield return new WaitForSeconds(m_visibleTime);
        m_animator.SetTrigger("disappear");
    }
    
}
