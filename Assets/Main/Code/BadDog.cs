using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadDog : MonoBehaviour
{
    Transform myTransform;

    [SerializeField] private Frightener frightener;
    [SerializeField] private SphereCollider rangeSphere;
    [SerializeField] private Animator animator;
  // [SerializeField] private AudioSource audioSource;
    private bool isBarking = false;
    [SerializeField] private float barkInterval = 2;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
        animator.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mothersPosition = Mother.instance.myTransform.position;
        if (!isBarking)
        {
            if (Vector3.Distance(transform.position, mothersPosition) < rangeSphere.radius)
            {
                isBarking = true;
                Bark();
            }
        }
        else
        {
            Vector3 currentLookPosition = myTransform.forward + myTransform.position;
            Vector3 newLookPosition = Vector3.MoveTowards
                (currentLookPosition, mothersPosition, Time.deltaTime * 1.5f);

            myTransform.LookAt(newLookPosition);
        }
    }

    private void Bark()
    {
        /*animator.Play("Bark");

        Invoke("Bark", barkInterval);*/
       // animator.SetBool("IsBarking", true);

        animator.SetTrigger("Bark");
        float nextBarkDelay = Random.Range(0.4f, 1.2f);
        Invoke("Bark", nextBarkDelay);

    }

    public void Frighten()
    {
        //audioSource.Play();
        SoundManager.PlayOneShotSoundAt(SoundNames.Bark, transform.position);
        frightener.Frighten();

    }
}
