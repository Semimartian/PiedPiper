using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTriggerCollider : MonoBehaviour
{

    public UnityEvent OnPlayerEnter;
    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnPlayerEnter.Invoke();
            gameObject.SetActive(false);
        }
    }
}
