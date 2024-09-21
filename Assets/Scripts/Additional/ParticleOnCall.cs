using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnCall : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_Particles;

    public void PlayParticles()
    {
        m_Particles.Play();
    }
}
