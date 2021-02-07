using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalStaminaPickUp : MonoBehaviour
{
    [SerializeField] private float staminaBoost;

    public float Collect()
    {
        SoundManager.PlayOneShotSoundAt(SoundNames.MusicalStaminaCollection, transform.position);
        Destroy(this.gameObject);
        return staminaBoost;
    }
}
