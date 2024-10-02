using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityUtils;

public class RigidBodyLocomotion : MonoBehaviour {
    #region Fields
    public Transform tr;
    public Rigidbody rb;
    
    public float movementSpeed = 7f;
    public float airControlRate = 2f;
    public float jumpSpeed = 10f;
    public float jumpDuration = 0.2f;
    public float airFriction = 0.5f;
    public float groundFriction = 100f;
    public float gravity = 30f;
    public float slideGravity = 5f;
    public float slopeLimit = 30f;
    public bool useLocalMomentum;
        
    [SerializeField] Transform cameraTransform;
        
    Vector3 momentum, savedVelocity, savedMovementVelocity;

    #endregion
    void Awake() {
        tr = transform;
    }
}