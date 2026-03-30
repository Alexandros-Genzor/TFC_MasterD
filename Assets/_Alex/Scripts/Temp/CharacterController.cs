using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    // exclusivo de jugador
    #region PLAYER_ONLY
    public Camera cam;
    public float camSensitivity;
    public Vector3 camOffset;

    private Vector2 _rotation;

    public GameObject camTgt;
    public GameObject camAnchor;
        
    public float t;
    
    public float lowerLimitV, upperLimitV;

    [SerializeField] private PlayerInput playerIn;

    private InputAction _charMove;
    private InputAction _charJump;
    private InputAction _charLook;

    private Vector2 _move;
    private Vector2 _look;
    
    #endregion

    // temporal en esta clase -> mover a clase padre para todos personajes
    [SerializeField] private Rigidbody rb;
    
    public float walkSpeed;
    public float jumpForce = 10;
    
    public float minHealth = 0, maxHealth = 100;
    [SerializeField] private float health;
    public float Health {get => health; set => health = Mathf.Clamp(value, minHealth, maxHealth);}
    // temp
    public bool gotDmg = false;
    public float dmg = -10; // temporal
    
    #region LIFECYCLE FUNC

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerIn = GetComponent<PlayerInput>();
        
        // _charMove  = playerIn.actions.FindAction("Move");
        // _charJump  = playerIn.actions.FindAction("Jump");
        // _charLook  = playerIn.actions.FindAction("Look");
        
    }

    void Start()
    {
        health = maxHealth;

        if (cam == null)
            cam = Camera.main;

    }

    void Update()
    {
        // Controls();
        Movement();
        CameraControl();
        
        if (gotDmg)
            AlterHealth(dmg);
        
    }
    
    #endregion
    
    #region INPUT_EVENTS

    /*public void Controls(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
            
        }
        
    }*/
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            rb.AddForce(0, /*Vector3.up * */jumpForce, 0, ForceMode.Impulse);
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
        
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
        
    }
    
    #endregion

    public void AlterHealth(float healthChange)
    {
        health += healthChange;
        
    }
    
    
    /*public void Movement()
    {
        Vector3 camFwd = cam.transform.forward;
        camFwd.y = 0;
        
        Vector2 input = playerIn.actions["Move"].ReadValue<Vector2>();

        Vector3 fwd = transform.forward * input.y;
        Vector3 rgt = transform.right * input.x;
        
        transform.position += Vector3.ClampMagnitude((fwd + rgt), 1) * walkSpeed * Time.deltaTime;

        transform.forward = Vector3.Slerp(transform.forward, camFwd, 0.05f);
        
    }*/
    
    public void Movement()
    {
        Vector3 camFwd = cam.transform.forward;
        camFwd.y = 0;
        
        // Vector2 input = _charMove.ReadValue<Vector2>();

        // Vector3 fwd = (transform.forward * input.y);
        // Vector3 rgt = (transform.right * input.x);
        
        // Vector3 movement = (transform.forward * input.y + transform.right * input.x);
        Vector3 movement = (transform.forward * _move.y + transform.right * _move.x);
        
        // transform.position += (fwd + rgt) * walkSpeed * Time.deltaTime;
        transform.position += movement * walkSpeed * Time.deltaTime;

        transform.forward = Vector3.Slerp(transform.forward, camFwd, 0.05f);
        
    }

    /*private void Movement()
    {
        Vector3 camFwd = cam.transform.forward;
        camFwd.y = 0;

        Vector2 input = _charMove.ReadValue<Vector2>();
        
        Vector3 movement = new Vector3(input.x, 0, input.y).normalized  * walkSpeed;
        // Vector3 movement = new Vector3(input.x, 0, input.y) * walkSpeed;
        
        rb.velocity = movement;
        // rb.velocity = movement * walkSpeed;
        // rb.velocity = movement.normalized;
        // rb.AddForce(movement * walkSpeed, ForceMode.Force);
        
    }*/

    private void CameraControl()
    {
        // Debug.Log(_look);
        
        // camAnchor.transform.position = camOffset + camTgt.transform.position;
        
        _rotation += _look * camSensitivity;
        _rotation.y = Mathf.Clamp(_rotation.y, lowerLimitV, upperLimitV);
        
        cam.transform.position = Vector3.Lerp(cam.transform.position, camAnchor.transform.position, 
            Mathf.SmoothStep(0, 1, t));

        camTgt.transform.eulerAngles = new Vector3(_rotation.y, _rotation.x, 0);
        
        cam.transform.LookAt(camTgt.transform.position);

        // _rotation.y += 

    }

}
