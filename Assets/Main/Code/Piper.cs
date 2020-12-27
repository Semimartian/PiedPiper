using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piper : MonoBehaviour
{
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
    [SerializeField] private MusicalStaminaUI musicalStaminaUI;
    [SerializeField] private ParticleSystem noteParticles;

    public static Piper instance;

    void Start()
    {
        instance = this;
       // Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible =false;

        myTransform = transform;
        rigidbody = GetComponent<Rigidbody>();
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
        ToggleWalking(false);
        musicalStamina = 0;

        noteParticles.Stop();
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
