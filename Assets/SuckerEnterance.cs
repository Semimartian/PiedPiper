using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckerEnterance : MonoBehaviour
{
    [SerializeField] private Transform exit;
    [SerializeField] private AudioSource cuttingSound;
    private int chicksInside = 0;
    private float volumeChangePerSecond = 1.8f;
    private void OnTriggerEnter(Collider other)
    {
        Baby baby = other.gameObject.GetComponentInParent<Baby>();
        if (baby != null)
        {
            chicksInside++;
            baby.GetSucked();
            Invoke("SpawnFeathers", 1.2f);
        }
    }

    private void SpawnFeathers()
    {
        chicksInside--;

        Transform effect = Spawner.instance.SpawnFeatherStream().transform;
        effect.position = exit.position;
    }

    private void Update()
    {
        float volume = cuttingSound.volume;
        if (chicksInside <= 0)
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
    }
}
