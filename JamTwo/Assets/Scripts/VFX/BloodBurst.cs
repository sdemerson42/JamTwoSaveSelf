using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBurst : MonoBehaviour
{

    ParticleSystem m_particles;
    void Awake()
    {
        m_particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_particles.IsAlive()) Destroy(gameObject);
    }
}
