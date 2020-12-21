using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{

    [SerializeField] private Animator fenceAnimator;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayEndScene();
        }
    }

    private void PlayEndScene()
    {
        Debug.Log("PlayEndScene");
        fenceAnimator.Play("Open");
    }
}
