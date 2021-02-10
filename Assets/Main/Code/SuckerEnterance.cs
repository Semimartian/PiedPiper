using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckerEnterance : MonoBehaviour
{
    [SerializeField] private Transform exit;
    [SerializeField] private AudioSource cuttingSound;
    [SerializeField] private int maxSucked;
    private bool isOpen = true;
    private int rodentsInside = 0;
    private int totalSucked = 0;
    private float volumeChangePerSecond = 1.8f;

    public bool IsOpen { get => isOpen; }

    //[SerializeField] private ParticleSystem hairParticleSystem;
    private void OnTriggerEnter(Collider other)
    {
        if (isOpen && other.gameObject.TryGetComponent(out Suckable suckable))
        {
            rodentsInside++;
            totalSucked++;
            suckable.GetSucked();
            Invoke("SpawnHairs", 1.2f);
            if (totalSucked >= maxSucked)
            {
                isOpen = false;
            }
        }
    }

    private void SpawnHairs()
    {
        rodentsInside--;
        //hairParticleSystem.Play();
       Transform effect = Spawner.instance.SpawnHairStream().transform;//TODO: innefficient
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
