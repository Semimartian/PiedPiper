using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicalStaminaUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Image circleImage;

    public void UpdateUI(float musicalStamina, Color colour)
    {
        fillImage.fillAmount = musicalStamina;
        fillImage.color = colour;
        circleImage.color = colour;
    }
}
