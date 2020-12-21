using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother : MonoBehaviour
{
    private Rigidbody rigidbody;
    [HideInInspector]public Transform myTransform;
    [Header("Speeds and Forces")]
    [SerializeField] private float rotationSpeedPerSecond;
    [SerializeField] private float forwardSpeedPerSecond;
    private float currentForwardSpeed;
    [SerializeField] private float AccelarationPerSecond = 0.5f;
    [SerializeField] private float deaccelerationPerSecond = 0.5f;
    [SerializeField] private float jumpForce ;

    [SerializeField] private Animator animator;
    public bool IsMoving
    {
        get { return currentForwardSpeed != 0; }
    }
    private bool jumpRequest = false;
    public static Mother instance;

    void Start()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible =false;

        myTransform = transform;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetMouseButtonDown(1))
        {
            jumpRequest = true;
        }
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;

        if (Input.GetMouseButton(0))
        {
            animator.SetBool("IsWalking", true);

            float mouseMovement = Input.GetAxisRaw("Mouse X");

            Quaternion currentRotationQurternion = rigidbody.rotation;
            if (mouseMovement != 0)
            {
                /*myTransform.Rotate(new Vector3
                    (0, rotationPerSecond * mouseMovement * deltaTime, 0));*/

                Vector3 currentRotation = currentRotationQurternion.eulerAngles;
                currentRotation += new Vector3
                    (0, rotationSpeedPerSecond * mouseMovement * deltaTime, 0);
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
            animator.SetBool("IsWalking", false);
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

        if (jumpRequest)
        {
            Jump();
            jumpRequest = false;
        }
    }


    private void Jump()
    {
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetTrigger("Jump");
        animator.SetBool("InAir", true);
    }

    private void Land()
    {
        animator.SetBool("InAir", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Land();
    }

}
