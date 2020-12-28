using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckerEnterance : MonoBehaviour
{
    [SerializeField] private Transform exit;
    [SerializeField] private AudioSource cuttingSound;
    private int rodentsInside = 0;
    private float volumeChangePerSecond = 1.8f;

    //[SerializeField] private ParticleSystem hairParticleSystem;
    private void OnTriggerEnter(Collider other)
    {
        ISuckable suckable = other.gameObject.GetComponentInParent<ISuckable>();
        if (suckable != null)
        {
            rodentsInside++;
            suckable.GetSucked();
            Invoke("SpawnHairs", 1.2f);
        }
    }

    private void SpawnHairs()
    {
        rodentsInside--;
        //hairParticleSystem.Play();
       Transform effect = Spawner.instance.SpawnHairStream().transform;
        effect.position = exit.position;

    }

    private void Update()
    {
        float volume = cuttingSound.volume;
        if (rodentsInside <= 0)
        {
            if (volume > 0)
            {
                cuttingSound.volume -= Time.deltaTime * volumeChangePerSecond;
            }
           /* if (cuttingSound.isPlaying)
            {
               // float 
                //cuttingSound.Stop();
            }*/
        }
        else
        {
            if (volume < 1)
            {
                cuttingSound.volume += Time.deltaTime * volumeChangePerSecond;
            }
            /* if (!cuttingSound.isPlaying)
             {
                // cuttingSound.Play();
             }*/
        }

        /*if (Input.GetKeyDown(KeyCode.P))
        {
            hairParticleSystem.Play();
        }*/
    }
}
