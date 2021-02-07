using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    public enum CameraStates
    {
        FollowingPlayer, Static, Victory
    }
    private CameraStates state;
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
        state = CameraStates.FollowingPlayer;
    }

    public void ChangeState(CameraStates newState)
    {
        state = newState;
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

        if(state == CameraStates.FollowingPlayer)
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

    public void ModifyLerpedMovementOnAllAxesBy(Vector3 value)
    {
        lerpedMovementOnAllAxes += value;
    }

    [SerializeField] private AnimationCurve transitionCurve;
    [SerializeField] private Transform endSceneAnchor;
    public void TransitionTo(Transform anchor)
    {
        StartCoroutine(TransitionCoroutine(anchor));
    }

    public void endTransition()
    {
        ChangeState(CameraStates.Victory);
        StartCoroutine(TransitionCoroutine(endSceneAnchor));
    }

    private IEnumerator TransitionCoroutine(Transform anchor)
    {
        Vector3 originalPosition = myTransform.position;
        Quaternion originalRotation = myTransform.rotation;

        float time = 0;
        float endTime = transitionCurve.keys[transitionCurve.length - 1].time;

        while (time < endTime)
        {
            //float deltaTime = Time.deltaTime;
            time += Time.fixedDeltaTime;

            float t = transitionCurve.Evaluate(time);
            myTransform.position = Vector3.Lerp(originalPosition, anchor.position, t);
            // Vector3.MoveTowards(transform.position, target.position, speed * deltaTime);
            myTransform.rotation = Quaternion.Lerp(originalRotation, anchor.rotation, t);
            //Quaternion.RotateTowards(transform.rotation, target.rotation, rotationSpeed * deltaTime);

            yield return new WaitForFixedUpdate();

        }
    }
}
