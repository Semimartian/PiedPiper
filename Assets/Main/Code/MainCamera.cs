using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Transform myTransform;

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Vector3 lerpedMovementOnAllAxes;
    private Transform audioListener;
   // [SerializeField] private float lerpedMovementAmount = 0.5f;
    // [SerializeField] private float ZOffsetFromTarget;

    private void Awake()
    {
        myTransform = transform;
        CreateAudioListener();
    }


    private void CreateAudioListener()
    {
        GameObject listenerGO = new GameObject("AudioListener");
        listenerGO.AddComponent<AudioListener>();
        audioListener = listenerGO.transform;
        audioListener.position = myTransform.position - new Vector3(0,0, offset.z);
        audioListener.SetParent(myTransform);
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;

        Vector3 currentPosition = myTransform.position;
        Vector3 targetPosition = target.position + offset;

        Vector3 lerpT = lerpedMovementOnAllAxes * deltaTime;
        Vector3 newPosition = new Vector3(
            Mathf.Lerp(currentPosition.x, targetPosition.x, lerpT.x),
            Mathf.Lerp(currentPosition.y, targetPosition.y, lerpT.y),
            Mathf.Lerp(currentPosition.z, targetPosition.z, lerpT.z)
            );

          //  Vector3.Lerp(myTransform.position, targetPosition, lerpedMovementAmount * deltaTime);
        myTransform.position = newPosition;

        /* Vector3 newPosition = myTransform.position ;
         newPosition.z = target.position.z + ZOffsetFromTarget;
         myTransform.position = newPosition;*/



    }
}
