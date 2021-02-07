using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrap : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    // TODO: This doesn't look like something that should differ from one instance to the other. Make it static or move it to some data container. 
    private bool triggered = false;
    public void Trigger()
    {
        if (triggered)
        {
            Debug.LogWarning("Tried to trigger the same trap more than once");
            return;
        }
        triggered = true;

        SoundManager.PlayOneShotSoundAt(SoundNames.MouseTrap, transform.position);
        Rigidbody triggeredTrap = Spawner.instance.SpawnTriggeredMouseTrap();
        triggeredTrap.transform.position = transform.position;
        triggeredTrap.position = transform.position;
        triggeredTrap.rotation = transform.rotation;
        triggeredTrap.isKinematic = false;
        triggeredTrap.constraints = RigidbodyConstraints.None;
        triggeredTrap.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        Destroy(gameObject);
    }
}
