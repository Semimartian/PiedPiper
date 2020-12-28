using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodentsUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text;

    public void UpdateText(int numberOfChicks)
    {
        text.text = numberOfChicks.ToString();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
