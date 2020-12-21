using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Attraction
{
    public Transform attractopnPoint;
    public float radius;
    public float force;
}
public class Sucker : MonoBehaviour
{
    public Attraction attractionField;
    //public Rigidbody rigidbody;

    public void Initialise()
    {
       // rigidbody = GetComponent<Rigidbody>();
        if (attractionField.attractopnPoint == null)
        {
            attractionField.attractopnPoint = transform;
        }
    }
}
