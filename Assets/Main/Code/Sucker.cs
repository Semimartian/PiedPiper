using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Attraction
{
    public Transform attractionPoint;
    public float radius;
    public float force;
}

public class Sucker : MonoBehaviour
{
    [SerializeField] private SuckerEnterance suckerEnterance;
    public Attraction attractionField;
    //public Rigidbody rigidbody;

    public void Initialize()
    {
       // rigidbody = GetComponent<Rigidbody>();
        if (attractionField.attractionPoint == null)
        {
            attractionField.attractionPoint = transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (suckerEnterance.IsOpen && other.gameObject.TryGetComponent(out Suckable suckable)) {
            Vector3 attractionPoint = attractionField.attractionPoint.position;
            Vector3 suckablePosition = suckable.transform.position;
            float distanceFromSucker = Vector3.Distance(attractionPoint, suckablePosition);//TODO: Use squared distance to save calculations
            if (distanceFromSucker <= attractionField.radius)
            {
                // Debug.Log("distanceFromMagnet" + distanceFromMagnet);
                float attractionSpeed =
                    (Mathf.Abs(distanceFromSucker - attractionField.radius) / attractionField.radius) * attractionField.force;
                suckable.GetRigidBody().AddForce
                    ((attractionPoint - suckablePosition).normalized//TODO: Check if this really is the direction
                    * (attractionSpeed /** deltaTime*/), ForceMode.Force);
                // Debug.Log("Sucking");
            }
        }
    }
}