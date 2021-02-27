using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScene : MonoBehaviour
{
   [SerializeField] private ParticleSystem particleSystem;
    private void OnEnable()
    {
        StartCoroutine(PlayCoRoutine());
    }
    /*public void Play()
    {
        StartCoroutine(PlayCoRoutine());
    }*/

    private IEnumerator PlayCoRoutine()
    {
        Debug.Log("YAY");
        particleSystem.Play();
        yield return null;
    }
}
