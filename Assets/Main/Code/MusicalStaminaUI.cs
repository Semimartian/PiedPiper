using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicalStaminaUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void UpdateUI(float musicalStamina)
    {
        fillImage.fillAmount = musicalStamina;
    }
}
