using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorryQuad : MonoBehaviour
{
    private bool isActive = false;
    private Transform cameraTransform;
    // Update is called once per frame

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        transform.LookAt(cameraTransform);
    }

    public void Appear()
    {
        isActive = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Animator>().SetTrigger("Play");
    }
}
