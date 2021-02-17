using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject floor;
    private MeshRenderer floorMeshRenderer;
    private BoxCollider collider;

    void Start()
    {
        floorMeshRenderer = floor.GetComponent<MeshRenderer>();
        collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + floorMeshRenderer.bounds.size.z), Quaternion.identity, transform.parent);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collider.enabled = false;
            Instantiate(prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + floorMeshRenderer.bounds.size.z), Quaternion.identity, transform.parent);
        }
    }
}
