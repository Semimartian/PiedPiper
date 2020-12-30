using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorDelayer : MonoBehaviour
{
    [SerializeField] private float maxDelay;
    // Start is called before the first frame update
    void Start()
    {

        GetComponent<Animator>().enabled = false;
        Invoke("TurnOnAnimator", Random.Range(0, maxDelay));

    }

    private void TurnOnAnimator()
    {
        GetComponent<Animator>().enabled = true;
        Destroy(this);
    }
}
