using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // exclusivo de jugador
    #region PLAYER_ONLY
    [Header("Camera & Rotation")]
    public Camera cam;
    public float camSensitivity;
    // multiplicador de sensibilidad de la cámara con joystick
    public float camGamepadMult = 3;
    // aplica un offset de distancia al ancla de la cámara respecto al eje (si se usa provoca que la cámara se atasque).
    public Vector3 camOffset;
    
    public GameObject camTgt;
    public GameObject camAnchor;
    
    private Vector2 _rotation;
    
    // controla el trailing (velocidad del lerp) de la cámara respecto a su anchor.
    public float camT;
    // controla el trailing (velocidad del lerp) del body respecto al forward de la cámara.
    public float charT;
    
    // límite de depresión y elevación de la cámara.
    public float lowerLimitV, upperLimitV;

    // DEPRECATED
    // private InputAction _charMove;
    // private InputAction _charJump;
    // private InputAction _charLook;

    // almacena el valor de los controles de desplazamiento.
    private Vector3 _move;
    // almacena el valor del control de movimiento de la cámara.
    private Vector2 _look;

    [Header("-- TEMP --")]
    // temporal
    [SerializeField] private GameObject sword;
    private bool _isAttacking;
    
    [Header("GameObject Components")]
    [SerializeField] private PlayerInput playerIn;
    
    #endregion
    
    // temporal en esta clase -> mover a clase padre para todos personajes
    [SerializeField] private Rigidbody rb;
    
    [Header("Movement")]
    public float walkSpeed;
    public float deceleration;
    public float jumpForce = 10;
    [SerializeField] private bool isGrounded;

    [Header("Health & Damage")] 
    [SerializeField] private float minHealth = 0;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health;
    public float Health {get => health; set => health = Mathf.Clamp(value, minHealth, maxHealth);}
    // temp
    public bool gotDmg = false;
    public float dmg = 10; // temporal
    
    #region LIFECYCLE FUNCTIONS
    // asignación de componentes (que no vayan a ser añadidos / eliminados en runtime) y que serán usados en código.
    private void Awake()
    {
        // rb = GetComponent<Rigidbody>(); // manualmente asignado en inspector
        // playerIn = GetComponent<PlayerInput>(); // manualmente asignado en inspector
        
    }

    void Start()
    {
        // Inicializa la vida del personaje (no compatible con persistencia entre escenas).
        Health = maxHealth;

        // Automáticamente asigna la MainCamera de la escena si no se ha asignado una manualmente.
        if (cam == null)
            cam = Camera.main;

    }

    void Update()
    {
        CameraControl();

        // puede que sea redundante :/
        if (gotDmg)
        {
            AlterHealth(dmg);
            gotDmg = false;

        }
        
        if (health <= 0)
            gameObject.SetActive(false);
        
    }

    private void FixedUpdate()
    {
        Movement();
        
    }

    #endregion
    
    // Contiene todas las funciones llamadas por el sistema de eventos de PlayerInput.
    #region INPUT_EVENTS

    // para uso con eventos por c# de PlayerInput
    public void Controls(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
                OnMove(context);
                
                break;
            
            case "Jump":
                OnJump(context);
                
                break;
            
            case "Look":
                OnLook(context);
                
                break;
            
            case "Attack":
                OnAttack(context);
                
                break;
            
        }
        
    }
    
    /// <summary>
    /// Registro de entrada de control de salto (Input System)
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            rb.AddForce(0, /*Vector3.up * */jumpForce, 0, ForceMode.Impulse);
            // rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

        }
        
    }

    /// <summary>
    /// Registro de entrada de controles de desplazamiento (Input System)
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        _move = new Vector3(moveInput.x, 0, moveInput.y);
        
    }

    /// <summary>
    /// Registro de entrada de controles de cámara (Input System)
    /// </summary>
    public void OnLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
        
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        // para evitar cualquier posible overlap
        if (_isAttacking)
            StopCoroutine(nameof(IAttack));

        StartCoroutine(nameof(IAttack));

    }

    public void OnTestKnockback(InputAction.CallbackContext context)
    {
        if (context.started)
            rb.AddForce(-transform.forward * 8, ForceMode.Impulse);
        
    }
    
    #endregion

    IEnumerator IAttack()
    {
        sword.SetActive(true);
        _isAttacking = true;
        yield return new WaitForSeconds(2);
        sword.SetActive(false);
        _isAttacking = false;

    }

    // mover a clase padre (cuando se haga la clase padre)
    /// <summary>
    /// Modifica el valor de vida del personaje.
    /// </summary>
    /// <param name="healthChange">Valor para modificar la vida.</param>
    /// <param name="isHealing">Define si "healthChange" es daño o curación (default: daño).</param>
    public void AlterHealth(float healthChange, bool isHealing = false)
    {
        Health += healthChange * (isHealing ? 1 : -1);
        
    }
    
    // sistema de movimiento mediante transform / sin uso de rigidbody ni físicas -- DEPRECATED debido al uso de sistema de movimiento mediante RigidBody.
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

        // Vector3 movement = transform.TransformDirection(_move) * acceleration;
        Vector3 movement = transform.TransformDirection(_move) * walkSpeed;
        
        // rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z); // controla el movimiento del personaje atacando directamente a la velocidad del rigidbody
        
        // controla el movimiento del personaje a base de ejercer una aceleración en la direccion del personaje (+ clamp de velocidad) -> reemplazado temporalmente por ForceMode.VelocityChange.
        if (_move != Vector3.zero)
        {
            // rb.AddForce(movement * acceleration, ForceMode.Force);
            // rb.velocity = Vector3.ClampMagnitude(RbVelocityHorizontal(), walkSpeed) + RbVelocityVertical(); // !IMPORTANT! -> clamp de velocidad horizontal, manteniendo caída con gravedad
            
            rb.AddForce((movement - RbVelocityHorizontal()), ForceMode.VelocityChange);
            
        }
        else
            rb.AddForce(RbVelocityHorizontal() * -deceleration, ForceMode.Force);
        
        transform.forward = Vector3.Slerp(transform.forward, camFwd, charT);
        
        Debug.Log(rb.velocity.magnitude);
        
    }

    private void CameraControl()
    {
        // camAnchor.transform.position = camOffset + camTgt.transform.position;
        
        // aplica el multiplicador de sensibilidad de la cámara cuándo detecta que el esquema de entrada actual es un gamepad.
        _rotation += _look * (camSensitivity * (playerIn.currentControlScheme == "Controller" ? camGamepadMult : 1));
        // limita el ángulo de elevación / depresión de la cámara (eliminar cuándo se use cámara de cinemachine con springarm).
        _rotation.y = Mathf.Clamp(_rotation.y, lowerLimitV, upperLimitV);
        
        cam.transform.position = Vector3.Lerp(cam.transform.position, camAnchor.transform.position, 
            Mathf.SmoothStep(0, 1, camT));

        camTgt.transform.eulerAngles = new Vector3(_rotation.y, _rotation.x, 0);
        
        cam.transform.LookAt(camTgt.transform.position);
        
        Debug.Log(playerIn.currentControlScheme);

    }
    
    // mover toda región a clase padre
    #region UTILS
    /// <summary>
    /// Devuelve los componentes horizontales de la velocidad del rigidbody (X, 0, Z).
    /// </summary>
    /// <returns></returns>
    private Vector3 RbVelocityHorizontal()
    {
        return new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    /// <summary>
    /// Devuelve el componente vertical de la velocidad del rigidbody (0, Y, 0).
    /// </summary>
    /// <returns></returns>
    private Vector3 RbVelocityVertical()
    {
        return new Vector3(0, rb.velocity.y, 0);
    }
    
    #endregion
    
    #region TRIGGERS & COLLIDERS

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            isGrounded = true;

    }

    #endregion

}
