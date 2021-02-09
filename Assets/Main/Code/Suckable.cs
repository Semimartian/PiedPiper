﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Suckable : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float suckChance;
    private bool isSuckable = true;

    public abstract Rigidbody GetRigidBody();
    public abstract void GetSucked();
    public abstract Transform GetTransform();
}