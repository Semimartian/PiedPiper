using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public static Spawner instance;
    [SerializeField] private GameObject drumStickPreFab;
    [SerializeField] private GameObject flamePreFab;
    [SerializeField] private GameObject puffPreFab;
    [SerializeField] private GameObject featherStreamPreFab;
    
    private void Awake()
    {
        instance = this;
    }

    public GameObject SpawnDrumStick()
    {
        return Instantiate(drumStickPreFab);
    }

    public GameObject SpawnFlame()
    {
        return Instantiate(flamePreFab);
    }

    public GameObject SpawnPuff()
    {
        return Instantiate(puffPreFab);
    }

    public GameObject SpawnFeatherStream()
    {
        return Instantiate(featherStreamPreFab);
    }
}
