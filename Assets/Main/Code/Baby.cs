using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : Suckable
{
    public bool isAlive;
   [HideInInspector]  public Transform myTransform;
    [HideInInspector] public Transform closestKin;
    private const float FORWARD_SPEED_PER_SECOND = 2f;

    private const float ACCELERATION_PER_SECOND = 8f;
    private const float DEACCELERATION_PER_SECOND = 8f;

    private float currentSpeed = 0;
    public const float DESIRED_DISTANCE_FROM_KIN = 1f;

    public const float ACCEPTABLE_DISTANCE_FROM_KIN = 1f;

    private bool isFrightened = false;
    bool isBurning = false;

    public bool IsFrightened
    {
        get
        {
            return isFrightened;
        }
    }

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject graphics;
    [SerializeField] private GameObject collider;


    private void Awake()
    {
        myTransform = transform;
        rigidbody = GetComponent<Rigidbody>();
    }

    public void CheckForKinDistance(ref float deltaTime)
    {
        float modifier = deltaTime *
            ((Vector3.Distance(myTransform.position, closestKin.position) > DESIRED_DISTANCE_FROM_KIN)
            ? ACCELERATION_PER_SECOND : -DEACCELERATION_PER_SECOND);
        currentSpeed += modifier;
        currentSpeed= Mathf.Clamp(currentSpeed, 0, FORWARD_SPEED_PER_SECOND);

    }
   

    public void GoTowardsKin(ref float deltaTime)
    {
        myTransform.LookAt(closestKin);
        if (currentSpeed > 0)
        {
            WalkForward(ref deltaTime);

        }
        else
        {
            animator.SetBool("IsWalking", false);

        }
    }

    private void WalkForward(ref float deltaTime)
    {
        Vector3 Movement = myTransform.forward * currentSpeed * deltaTime;
        //  rigidbody.AddForce (Movement, ForceMode.VelocityChange);
        rigidbody.MovePosition(rigidbody.position + Movement);
        animator.SetBool("IsWalking", true);
    }

    public void BecomeFrightened(ref Vector3 FrighteningOrigin)
    {
        Debug.Log("FRIGHT");
        isFrightened = true;
        Vector3 myPosition = myTransform.position;
        Vector3 direction =
            (myPosition - FrighteningOrigin).normalized;
        myTransform.LookAt(myPosition + direction);
    }

    public void FrightendRoutine(ref float deltaTime)
    {
        currentSpeed = FORWARD_SPEED_PER_SECOND;
        WalkForward(ref  deltaTime);
    }

    #region Idle:
    private struct IdleRoutineData
    {
        public float nextRotationTime;
        public float nextMovementSwitchTime;
        public bool isMoving;

    }

    private IdleRoutineData idleRoutineData;

    public void IdleRoutine(ref float time, ref float deltaTime)
    {
        bool isInAcceptableRange = (Vector3.Distance(myTransform.position, closestKin.position) < ACCEPTABLE_DISTANCE_FROM_KIN);

        if (!isInAcceptableRange)
        {
            myTransform.LookAt(closestKin);
            idleRoutineData.isMoving = true;
        }
        else if (time > idleRoutineData.nextRotationTime)
        {
            idleRoutineData.nextRotationTime = time + Random.Range(0, 1.5f);

             Vector3 currentEuler = rigidbody.rotation.eulerAngles;
             currentEuler.y = Random.Range(0, 360);

             rigidbody.rotation = Quaternion.Euler(currentEuler);
        }


        if (time > idleRoutineData.nextMovementSwitchTime)
        {
            idleRoutineData.nextMovementSwitchTime = time + Random.Range(0, 1.5f);
            idleRoutineData.isMoving = Random.Range(0, 2) == 0 ? false : true;
        }

        if (idleRoutineData.isMoving )
        {
            currentSpeed = FORWARD_SPEED_PER_SECOND;
            WalkForward(ref deltaTime);

        }
        else
        {
            animator.SetBool("IsWalking", false);

        }
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if(isAlive&& !isBurning && collision.gameObject.tag == "Hot")
        {
            isBurning = true;
            float delay = Random.Range(0, 1.2f);
            Invoke("Burn", delay);
        }
    }

    private void Burn()
    {
        Die();
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.AddForce(Vector3.up * 1, ForceMode.Impulse);

        Vector3 myPosition = this.myTransform.position;

        Transform flameTransform = Spawner.instance.SpawnFlame().transform;
        flameTransform.transform.position = myPosition;

        Invoke("TurnIntoDrumStick",0.2f);
    }

    private void TurnIntoDrumStick()
    {
        DeathCry();
        graphics.SetActive(false);
        collider.SetActive(false);
        Vector3 myPosition = this.myTransform.position;

        Transform puffTransform = Spawner.instance.SpawnPuff().transform;
        puffTransform.position = myPosition;

        Transform drumStickTransform = Spawner.instance.SpawnDrumStick().transform;
        drumStickTransform.rotation = myTransform.rotation;
        drumStickTransform.position = myPosition;
        drumStickTransform.SetParent(myTransform);

    }

    public void GetSucked()
    {
        Die();
        DeathCry();
        gameObject.SetActive(false);

    }

    private void DeathCry()
    {
        SoundManager.PlayOneShotSoundAt(SoundNames.ChickDeath, myTransform.position);
    }

    private void Die()
    {
        isAlive = false;
    }
}

