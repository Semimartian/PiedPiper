using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadDogAnimationReporter : MonoBehaviour
{
    [SerializeField] private BadDog badDog;
    public void MakeMakeDogFrighten()
    {
        badDog.Frighten();
    }
}
