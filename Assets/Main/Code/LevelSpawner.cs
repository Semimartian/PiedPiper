using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
   // [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject floor;
    private MeshRenderer floorMeshRenderer;
    private BoxCollider triggerCollider;

    void Start()
    {
        floorMeshRenderer = floor.GetComponent<MeshRenderer>();
        triggerCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            DuplicateForward();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DuplicateForward();
            triggerCollider.enabled = false;
        }
    }

    private void DuplicateForward()
    {
        Vector3 position = transform.position;
        Vector3 clonePosition = new Vector3(position.x, position.y, position.z + floorMeshRenderer.bounds.size.z);
        Instantiate(gameObject, clonePosition, Quaternion.identity, transform.parent);
    }
}
