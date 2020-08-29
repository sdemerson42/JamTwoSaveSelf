using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionMenuLogic : MonoBehaviour
{
    public enum InteractionIcon
    {
        Charm, Think, Intimidate, Bite
    }

    public GameObject charmIcon;
    public GameObject thinkIcon;
    public GameObject intimidateIcon;
    public GameObject biteIcon;
    public Text text;

    Transform m_characterTransform;
    Vector3 m_characterOffset;
    const float m_flashSeconds = .15f;
    Color m_suddenDeathInactiveColor;

    void Awake()
    {
        m_characterTransform = transform.parent.GetChild(0);
        m_characterOffset = transform.position - m_characterTransform.position;
        gameObject.SetActive(false);
        m_suddenDeathInactiveColor = new Color(.2f, .2f, .2f);
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
    }

    void SetPosition()
    {
        transform.position = m_characterTransform.position + m_characterOffset;
    }

    public void StartSelectionFX(InteractionIcon icon)
    {
        StartCoroutine(AnimateIcon(icon));
    }

    public void StopSelectionFX()
    {
        biteIcon.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        intimidateIcon.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        charmIcon.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        thinkIcon.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        text.enabled = false;
        StopAllCoroutines();
    }

    public void SetSuddenDeath()
    {
        intimidateIcon.GetComponent<UnityEngine.UI.Image>().color = m_suddenDeathInactiveColor;
        charmIcon.GetComponent<UnityEngine.UI.Image>().color = m_suddenDeathInactiveColor;
        thinkIcon.GetComponent<UnityEngine.UI.Image>().color = m_suddenDeathInactiveColor;
        StartCoroutine(Countdown());
    }

    IEnumerator AnimateIcon(InteractionIcon icon)
    {
        GameObject selectedIcon;

        if (icon == InteractionIcon.Bite) selectedIcon = biteIcon;
        else if (icon == InteractionIcon.Charm) selectedIcon = charmIcon;
        else if (icon == InteractionIcon.Think) selectedIcon = thinkIcon;
        else selectedIcon = intimidateIcon;

        var image = selectedIcon.GetComponent<UnityEngine.UI.Image>();

        while(true)
        {
            image.color = new Color(1f, .1f, .1f);
            yield return new WaitForSecondsRealtime(m_flashSeconds);
            
            image.color = new Color(1f, 1f, 1f);
            yield return new WaitForSecondsRealtime(m_flashSeconds);
        }
    }

    IEnumerator Countdown()
    {
        text.enabled = true;
        for (int i = 3; i > 0; --i)
        {
            text.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
