using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// This script may be attached to any GameObject with a
/// 2D Light component. Causes a flickering effect on
/// FixedUpdate() that can be customized in the engine.
/// </summary>
public class FlickeringLight : MonoBehaviour
{

    public float intensityVariance;
    public uint fixedFrameLapse;

    Light2D m_light;
    float m_intensityMin;
    float m_intensityMax;
    uint m_frameCounter;

    void Awake()
    {
        m_light = GetComponent<Light2D>();
        float initialIntensity = m_light.intensity;
        m_intensityMin = initialIntensity - intensityVariance;
        m_intensityMax = initialIntensity + intensityVariance;
        m_frameCounter = 0;
    }

    void FixedUpdate()
    {
        if ((++m_frameCounter) % fixedFrameLapse == 0)
        {
            float intensity = Random.Range(m_intensityMin, m_intensityMax);
            m_light.intensity = intensity;
        }
    }
}
