using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Camera cam;
    public Vector2 camSensitivity;
    public Vector3 camOffset;

    private Vector2 _rotation;

    public GameObject camTgt;
    public GameObject camAnchor;

    public float t;
    
    public float lowerLimitV, upperLimitV;

    public float walkSpeed;
    
    public float minHealth = 0, maxHealth = 100;
    [SerializeField] private float health;
    public float Health {get => health; set => health = Mathf.Clamp(value, minHealth, maxHealth);}
    // temp
    public bool gotDmg = false;
    public float dmg = -10;
    
    #region LIFECYCLE FUNC

    private void Awake()
    {
        
        
    }

    void Start()
    {
        health = maxHealth;

        if (cam == null)
            cam = Camera.main;

    }

    void Update()
    {
        Controls();
        Movement();
        CameraControl();
        
        if (gotDmg)
            AlterHealth(dmg);
        
    }
    
    #endregion

    public void AlterHealth(float healthChange)
    {
        health += healthChange;
        
    }

    private void Controls()
    {
        
        
    }

    private void Movement()
    {
        
        
    }

    private void CameraControl()
    {
        
        
    }

}
