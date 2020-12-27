using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISuckable 
{
    Rigidbody GetRigidBody();
    void GetSucked();
    Transform GetTransform();
}