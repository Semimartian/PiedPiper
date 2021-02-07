using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    private Transform myTransform;
    private Transform target;
    private Vector3 offset;

    void Start()
    {
        myTransform = transform;
        offset = myTransform.localPosition;
        target = myTransform.parent;
        myTransform.parent = null;
    }

    private void FixedUpdate()
    {
        myTransform.position = (target.position + offset);
    }
}
