using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    private Sucker[] suckers;
    private ISuckable[] suckables;
    private float boundsExpansion = 3f;

    void Start()
    {
        MonoBehaviour[] monoBehaviours = FindObjectsOfType<MonoBehaviour>();
        List<ISuckable> suckablesList = new List<ISuckable>();
        for (int i = 0; i < monoBehaviours.Length; i++)
        {
            MonoBehaviour mono = monoBehaviours[i];
            if (monoBehaviours[i] is ISuckable)
            {
                suckablesList.Add((ISuckable)mono);
            }
        }
        suckables = suckablesList.ToArray();
        suckers = FindObjectsOfType<Sucker>();
        for (int i = 0; i < suckers.Length; i++)
        {
            suckers[i].Initialise();
        }

        Debug.Log("Suckers:" + suckers.Length);
        Debug.Log("Suckables:" + suckables.Length);

        // InitailaiseMetalObjects();

    }
    private void FixedUpdate()
    {
        // float deltaTime = Time.deltaTime;
        ManageSuckers();
      /* ManageMagnetoAgainstMagnets();
        magneto.MyFixedUpdate();*/
    }
    private void ManageSuckers()
    {
        for (int i = 0; i < suckers.Length; i++)
        {
            //bool isMagneto = /*metalObjectsToAttractMagneto &&*/( magnets[j] == magneto);
            Attraction attraction = suckers[i].attractionField;
            Vector3 attractionPoint = attraction.attractopnPoint.position;
            Bounds bounds = new Bounds
                (attractionPoint, Vector3.one * attraction.radius * boundsExpansion);
            float attractionDistance = attraction.radius;

            // Vector3 magnetPosition = magnets[j].attrractivePoint.position;
            for (int j = 0; j < suckables.Length; j++)
            {
                ISuckable suckable = suckables[j];
                //if (!metalObject.IsAttachedTo(magnets[j]))
                {
                    Transform suckableTransform = suckable.GetTransform();
                    Vector3 suckablePosition = suckableTransform.position;
                    if (bounds.Contains(suckablePosition))
                    {
                        float distanceFromSucker =
                         Vector3.Distance(attractionPoint, suckablePosition);
                        if (distanceFromSucker <= attractionDistance)
                        {
                            // Debug.Log("distanceFromMagnet" + distanceFromMagnet);
                            float attractionSpeed =
                               (Mathf.Abs(distanceFromSucker - attractionDistance) / attractionDistance) * attraction.force;
                            suckable.GetRigidBody().AddForce
                               ((attractionPoint - suckablePosition).normalized//TODO: Check if this really is the direction
                               * (attractionSpeed /** deltaTime*/), ForceMode.Force);
                            // Debug.Log("Sucking");
                        }
                    }
                    
                }
            }
        }
    }
}
