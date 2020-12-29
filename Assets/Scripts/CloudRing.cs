using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudRing : MonoBehaviour
{
    [SerializeField] private Transform anchor;
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, anchor.position.z);
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }
}
