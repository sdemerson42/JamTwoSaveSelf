using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WordBalloon : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject textObject;
    public GameObject panelObject;

    Text m_text;
    const float m_visibleTime = 4.5f;

    void Awake()
    {
        m_text = textObject.GetComponent<Text>();
        Debug.Log(m_text);
    }

    public void Speak(string message)
    {
        StartCoroutine(DisplayText(message));
    }

    IEnumerator DisplayText(string message)
    {
        textObject.SetActive(true);
        panelObject.SetActive(true);
        m_text.text = message;
        yield return new WaitForSeconds(m_visibleTime);
        textObject.SetActive(false);
        panelObject.SetActive(false);
    }
    
}
