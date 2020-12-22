using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterFlyBush : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;

    public void Play()
    {
        particleSystem.Play();
    }
}
