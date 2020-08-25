using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionLogic : MonoBehaviour
{

    Transform m_characterTransform;
    Vector3 m_characterOffset;

    void Awake()
    {
        m_characterTransform = transform.parent.GetChild(0);
        m_characterOffset = transform.position - m_characterTransform.position;
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
}
