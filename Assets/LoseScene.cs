using System.Collections;
using UnityEngine;

public class LoseScene : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        StartCoroutine(PlayCoRoutine());
    }

    private IEnumerator PlayCoRoutine()
    {
        Debug.Log("NO...");
        animator.SetTrigger("Start");
        yield return null;
    }
}
