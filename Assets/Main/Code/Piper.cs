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
    [HideInInspector] public Transform myTransform;
    [Header("Speeds and Forces")]
    [SerializeField] private float rotationSpeedPerSecond;
    [SerializeField] float rotationRange = 45;
    private float? previousFingerX;
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
    [SerializeField] private Gradient musicalStaminaColours;
    /*[SerializeField] private Color maxMusicalStaminaColour;
    [SerializeField] private Color minMusicalStaminaColour;*/
    private Color currentMusicalStaminaColour;
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
    private bool isDancing = false;
    private bool startedGame = false;
    [SerializeField] private MusicalStaminaUI musicalStaminaUI;
    [SerializeField] private ParticleSystem noteParticles;
    [SerializeField] private AudioSource musicAudioSource;

    #region DeathRelated:
    [SerializeField] private Transform livingBody;
    [SerializeField] private Rigidbody hat;
    [SerializeField] private Animator deadBody;
    #endregion

    [SerializeField] private Collider gameCollider;
    [SerializeField] private Transform gameOverCollidersParent;

    private List<Collider> gameOverColliders;

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

        if (isPlaying && !isDancing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startedGame = true;
                ToggleWalking(true);
            }
           /* else if (Input.GetMouseButtonUp(0))
            {
                ToggleWalking(false);
            }*/

            if (startedGame)
            {
                UpdateMusicalStamina(ref deltaTime);
            }
            //TODO: We can invoke these less frequently than Update() 
            DetermineMusicalStaminaColour();
            UpdateNoteParticles();
            UpdateMusicVolume();
            musicalStaminaUI.UpdateUI(musicalStamina, currentMusicalStaminaColour);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(TurnBack());
        }
    }

    private void UpdateNoteParticles()
    {
        noteParticles.startColor = currentMusicalStaminaColour;
    }

    private void UpdateMusicVolume()
    {
        musicAudioSource.volume = musicalStamina;
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
            StartCoroutine(StopPlaying());
        }
    }

    private void DetermineMusicalStaminaColour()
    {
        currentMusicalStaminaColour = musicalStaminaColours.Evaluate(musicalStamina);
          //  Color.Lerp(minMusicalStaminaColour, maxMusicalStaminaColour, musicalStamina);
    }

    private void Die()
    {
        Debug.Log("IsSkeleton");
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

    private IEnumerator StopPlaying()
    {
        Debug.Log("I shall play no longer!");

        isPlaying = false;
        animator.SetTrigger("Panic");
        ToggleWalking(false);
        musicalStamina = 0;

        SwapColliders();
        noteParticles.Stop();

        rigidbody.isKinematic = true;

        GameManager.OnPiperPanic();
        yield return new WaitForSeconds(2);
        Die();
        GameManager.OnGameOver();
    }

    public void Dance()
    {
        ToggleWalking(false);
        isDancing = true;
        animator.SetTrigger("Dance");
        StartCoroutine(TurnBack());

        //mainCamera.ModifyLerpedMovementOnAllAxesBy(Vector3.right * 1f);
        mainCamera.endTransition();
    }

    private IEnumerator TurnBack()
    {
        Vector3 lookAtPosition = rigidbody.position + Vector3.back;
        Vector3 newForward = (lookAtPosition - rigidbody.position).normalized;

        Quaternion finalRotation = Quaternion.LookRotation(newForward);
        float rotationSpeed = 120f;//HARDCODED
        while (true)
        {
            float deltaTime = Time.deltaTime;
            myTransform.rotation =
                Quaternion.RotateTowards(rigidbody.rotation, finalRotation, rotationSpeed * deltaTime);
            /* rigidbody.rotation =
                 Quaternion.RotateTowards(rigidbody.rotation, finalRotation,)*/
            yield return null;
        }
       
    }


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

    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        HandleMovement(ref deltaTime);
    }

    private void HandleMovement(ref float deltaTime)
    {
        float? fingerX = null; //Why doesnt this work? (Input.GetMouseButton(0) ? Input.mousePosition.x : null);
        if (Input.GetMouseButton(0))
        {
            fingerX = Input.mousePosition.x;
        }

        if (isWalking)
        {
            if(fingerX != null)
            {
                float fingerMovement = 
                    (previousFingerX == null ? 0 : ((float)fingerX - (float)previousFingerX));
                //TODO: Figure out wether we need to multiply by deltatime

                if (fingerMovement != 0)
                {
                    Quaternion rotationQuaternion = rigidbody.rotation;
                    Vector3 rotationEuler = rotationQuaternion.eulerAngles;
                    rotationEuler += new Vector3(0, rotationSpeedPerSecond * fingerMovement /** deltaTime*/, 0);
                    //Clamp
                    {
                        float clampedY = rotationEuler.y;
                        if (clampedY > 180)
                        {
                            clampedY -= 360;
                        }
                        clampedY = Mathf.Clamp(clampedY, -rotationRange, rotationRange);
                        rotationEuler.y = clampedY;
                        //TODO: This is not pretty and when I move the mouse fast it seems to break
                    }
                    // Debug.Log($"rotationEuler: {rotationEuler.y.ToString("f2")}");
                    rotationQuaternion = Quaternion.Euler(rotationEuler);
                    rigidbody.rotation = rotationQuaternion;
                }
            }
           

            currentForwardSpeed += AccelarationPerSecond * deltaTime;
            if (currentForwardSpeed > forwardSpeedPerSecond)
            {
                currentForwardSpeed = forwardSpeedPerSecond;
            }
        }//TODO: Something smells bad down here:
        else
        {
            currentForwardSpeed -= deaccelerationPerSecond * deltaTime;
            if (currentForwardSpeed < 0)
            {
                currentForwardSpeed = 0;
            }
        }

        if ( currentForwardSpeed != 0)
        {
            Vector3 Movement = myTransform.forward * currentForwardSpeed * deltaTime;
            //  rigidbody.AddForce (Movement, ForceMode.VelocityChange);
            rigidbody.MovePosition(rigidbody.position + Movement);
        }

        previousFingerX = fingerX;
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
