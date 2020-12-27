using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Piper : MonoBehaviour
{
    /*public enum PlayerState
    {
        Standing, Marching, Defenceles, Dead
    }*/

    private Rigidbody rigidbody;
    [HideInInspector]public Transform myTransform;
    [Header("Speeds and Forces")]
    [SerializeField] private float rotationSpeedPerSecond;
    [SerializeField] private float forwardSpeedPerSecond;
    private float currentForwardSpeed;
    [SerializeField] private float AccelarationPerSecond = 0.5f;
    [SerializeField] private float deaccelerationPerSecond = 0.5f;

    [SerializeField] private Animator animator;
    private bool isWalking = false;
    public bool IsMoving
    {
        get { return currentForwardSpeed != 0; }
    }
    [Header("Musical Stamina")]
    [SerializeField] private float musicalStaminaReductionPerSecond;
    private float musicalStamina = 1;
    private bool isPlaying = true;
    public bool IsPlaying
    {
        get { return isPlaying; }
    }
    private bool isAlive = true;
    public bool IsAlive
    {
        get { return isAlive; }
    }
    [SerializeField] private MusicalStaminaUI musicalStaminaUI;
    [SerializeField] private ParticleSystem noteParticles;

    #region DeathRelated:
    [SerializeField] private Transform livingBody;
    [SerializeField] private Rigidbody hat;
    [SerializeField] private Animator deadBody;
    #endregion

    private MainCamera mainCamera;
    public static Piper instance;

    void Start()
    {
        instance = this;
       // Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible =false;

        myTransform = transform;
        rigidbody = GetComponent<Rigidbody>();
        InitialiseColliders();
        mainCamera = FindObjectOfType<MainCamera>();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if (isPlaying)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ToggleWalking(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ToggleWalking(false);
            }

            UpdateMusicalStamina(ref deltaTime);
            musicalStaminaUI.UpdateUI(musicalStamina);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Die();
        }   
    }

    private void Die()
    {
        isAlive = false;

        rigidbody.isKinematic = true;

        hat.transform.SetParent(null);
        hat.isKinematic = false;
        Collider[] hatColliders = hat.GetComponentsInChildren<Collider>();
        for (int i = 0; i < hatColliders.Length; i++)
        {
            hatColliders[i].enabled = true;
        }

        livingBody.gameObject.SetActive(false);
        deadBody.gameObject.SetActive(true);
        deadBody.SetTrigger("Play");

        mainCamera.ChangeState(MainCamera.CameraStates.Static);

    }

    private void ToggleWalking(bool value)
    {
        isWalking = value;
        animator.SetBool("IsWalking", value);
    }

    private void UpdateMusicalStamina(ref float deltaTime)
    {
        musicalStamina -= musicalStaminaReductionPerSecond * deltaTime;
        if (musicalStamina <= 0)
        {
            StopPlaying();
        }
    }
    private void StopPlaying()
    {
        Debug.Log("I shall play no longer!");

        isPlaying = false;
        animator.SetTrigger("Panic");
        ToggleWalking(false);
        musicalStamina = 0;

        SwapColliders();
        noteParticles.Stop();

        rigidbody.isKinematic = true;


    }


    [SerializeField] private Collider gameCollider;
    [SerializeField] private Transform gameOverCollidersParent;

    private List<Collider> gameOverColliders;

    private void InitialiseColliders()
    {
        gameOverColliders = new List<Collider>();
        GetColliders(gameOverCollidersParent, gameOverColliders);
        for (int i = 0; i < gameOverColliders.Count; i++)
        {
            gameOverColliders[i].enabled = false;
        }
    }

    private void GetColliders(Transform obj, List<Collider> colliders)
    {
        var objColliders = obj.GetComponents<Collider>();
        if (objColliders.Length > 0)
        {
            colliders.AddRange(objColliders);
        }
        int childCount = obj.childCount;
        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                GetColliders(obj.GetChild(i), colliders);
            }
        }

    }

    private void SwapColliders()
    {
        for (int i = 0; i < gameOverColliders.Count; i++)
        {
            gameOverColliders[i].enabled = true;
        }
        gameCollider.enabled = false;

        
    }

    private float previousMouseX;

    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;

        float mouseX = Input.mousePosition.x;

        if (isWalking)
        {

            float mouseMovement = //Input.GetAxisRaw("Mouse X");
                mouseX - previousMouseX;

            Quaternion currentRotationQurternion = rigidbody.rotation;
            if (mouseMovement != 0)
            {
                /*myTransform.Rotate(new Vector3
                    (0, rotationPerSecond * mouseMovement * deltaTime, 0));*/

                Vector3 currentRotation = currentRotationQurternion.eulerAngles;
                currentRotation += new Vector3
                    (0, rotationSpeedPerSecond * mouseMovement /** deltaTime*/, 0);
                currentRotationQurternion = Quaternion.Euler(currentRotation);
                rigidbody.rotation = currentRotationQurternion;
            }

            currentForwardSpeed += AccelarationPerSecond * deltaTime;
            if (currentForwardSpeed > forwardSpeedPerSecond)
            {
                currentForwardSpeed = forwardSpeedPerSecond;
            }
        }
        else
        {
            currentForwardSpeed -= deaccelerationPerSecond * deltaTime;
            if (currentForwardSpeed < 0)
            {
                currentForwardSpeed = 0;
            }
        }

        if(currentForwardSpeed != 0)
        {
            Vector3 Movement = myTransform.forward * currentForwardSpeed * deltaTime;
            //  rigidbody.AddForce (Movement, ForceMode.VelocityChange);
            rigidbody.MovePosition(rigidbody.position + Movement);
        }

        previousMouseX = mouseX;

    }


    /*  private void Jump()
      {
          rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
          animator.SetTrigger("Jump");
         // animator.SetBool("InAir", true);
          SoundManager.PlayOneShotSoundAt(SoundNames.GooseJump, myTransform.position);

      }*/

    /*private void Land()
    {
        animator.SetBool("InAir", false);
    }*/

    private void OnCollisionEnter(Collision collision)
    {
       // Land();
    }

    private void OnTriggerEnter(Collider other)
    {
        MusicalStaminaPickUp musicalStaminaPickUp = 
            other.gameObject.GetComponentInParent<MusicalStaminaPickUp>();
        if (musicalStaminaPickUp != null)
        {
            musicalStamina += musicalStaminaPickUp.Collect();
            if (musicalStamina > 1)
            {
                musicalStamina = 1;
            }
        }
    }
}
