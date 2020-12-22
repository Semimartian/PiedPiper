using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalStaminaPickUp : MonoBehaviour
{
    [SerializeField] private float staminaBoost;

    public float Collect()
    {
        Destroy(this.gameObject);
        return staminaBoost;
    }
}
