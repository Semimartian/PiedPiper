using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScene : MonoBehaviour
{
    [SerializeField] private ParticleSystem confettiShower;
    [SerializeField] private ParticleSystem leftConfetti;
    [SerializeField] private ParticleSystem rightConfetti;
    [SerializeField] private Animator animator;

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
        animator.SetTrigger("Start");
        leftConfetti.Play();
        yield return new WaitForSeconds(0.5f);
        rightConfetti.Play();
        yield return new WaitForSeconds(0.5f);
        confettiShower.Play();
    }
}
