using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSurface : MonoBehaviour
{
    public byte burnChance;

    private void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
