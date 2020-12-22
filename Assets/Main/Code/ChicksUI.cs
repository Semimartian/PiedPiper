using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChicksUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text;

    public void UpdateText(int numberOfChicks)
    {
        text.text = numberOfChicks.ToString();
    }

}
