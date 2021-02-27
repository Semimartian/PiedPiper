using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawnable : MonoBehaviour
{
    public UnityEvent OnSpawn;
    public void Spawn()
    {
        OnSpawn.Invoke();
    }

}
