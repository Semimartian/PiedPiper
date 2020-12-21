using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frightener : MonoBehaviour
{
    [SerializeField] private SphereCollider rangeSphere;
   // [SerializeField] private float frightenFrequency;
    private Collider[] collidersInRange;
    private const int collidersInRangeSize =32;
    // Start is called before the first frame update
    void Start()
    {
        collidersInRange = new Collider[collidersInRangeSize];
        Frighten();
    }

    public void Frighten()
    {
        Vector3 centre = transform.position;
        int numberOfCollidersInRange =  Physics.OverlapSphereNonAlloc(centre, rangeSphere.radius, collidersInRange);
        for (int i = 0; i < numberOfCollidersInRange; i++)
        {
            Baby baby = collidersInRange[i].gameObject.GetComponentInParent<Baby>();
            if (baby!= null)
            {
                baby.BecomeFrightened(ref centre);
            }
        }

        Debug.Log( numberOfCollidersInRange + " colliders out of " + collidersInRangeSize);
       // Invoke("Frighten", frightenFrequency);
    }
}
