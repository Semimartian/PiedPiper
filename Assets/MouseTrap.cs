using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrap : MonoBehaviour
{

    public void Trigger()
    {
        Rigidbody triggeredTrap = Spawner.instance.SpawnTriggeredMouseTrap();
        triggeredTrap.position = transform.position;
        triggeredTrap.rotation = transform.rotation;
        triggeredTrap.isKinematic = false;
        triggeredTrap.constraints = RigidbodyConstraints.None;
        triggeredTrap.AddForce(Vector3.up * 1, ForceMode.Impulse);

        Destroy(gameObject);
    }
}
