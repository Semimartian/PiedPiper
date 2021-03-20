using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSurface : MonoBehaviour
{
    private static byte maxBurnChance = 5;
    private static byte minBurnChance = 2;

    public bool ShouldBurn()
    {
        return (Random.Range(0, maxBurnChance) < minBurnChance);
    }

    private void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
