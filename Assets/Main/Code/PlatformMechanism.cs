using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMechanism : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void Turn(bool value)
    {
        animator.SetBool("IsOn", value);
    }
}
