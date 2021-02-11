using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squasher : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Piper piper)) {
            piper.Die();
        }   
    }
}
