using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Alex.Scripts
{
    [RequireComponent(typeof(HealthManager))]
    
    public class PlayerControllerV2 : MonoBehaviour
    {        
        [SerializeField] protected Rigidbody rb;
    
        [Header("Movement")]
        // public float walkSpeed;
        // public float deceleration;
        public MovementManager movementMngr;
        
        public float jumpForce = 10;
        [SerializeField] protected bool isGrounded;
        
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
        [SerializeField] private HealthManager healthMngr;
    
        #region LIFECYCLE FUNC

        private void Awake()
        {
            healthMngr = GetComponent<HealthManager>();
            playerIn = GetComponent<PlayerInput>();
            
            if (rb == null)
                rb = GetComponent<Rigidbody>();
            
        }

        void Start()
        {
            if (cam == null)
                cam = Camera.main;
        
        }

        void Update()
        {
            // CameraControl();
        
        }

        private void FixedUpdate()
        {
            Movement();
        
        }

        private void LateUpdate()
        {
            CameraControl();
            
        }

        private void OnEnable()
        {
            playerIn.onActionTriggered += Controls;

        }

        private void OnDisable()
        {
            playerIn.onActionTriggered -= Controls;
            
        }

        #endregion
    
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
                    // OnJump(context);
                
                    break;
            
                case "Look":
                    OnLook(context);
                
                    break;
            
                /*case "Attack":
                OnAttack(context);
                
                break;*/
            
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

        /*public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        // para evitar cualquier posible overlap
        if (_isAttacking)
            StopCoroutine(nameof(IAttack));

        StartCoroutine(nameof(IAttack));

    }*/

        /*public void OnTestKnockback(InputAction.CallbackContext context)
        {
            if (context.started)
                rb.AddForce(-transform.forward * 8, ForceMode.Impulse);
        
        }*/
    
        #endregion
        
        public void Movement()
        {
            Vector3 camFwd = cam.transform.forward;
            camFwd.y = 0;

            // Vector3 movement = transform.TransformDirection(_move) * acceleration;
            Vector3 movement = transform.TransformDirection(_move) * movementMngr.speed;
            
            // rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z); // controla el movimiento del personaje atacando directamente a la velocidad del rigidbody
            
            // controla el movimiento del personaje a base de ejercer una aceleración en la direccion del personaje (+ clamp de velocidad) -> reemplazado temporalmente por ForceMode.VelocityChange.
            if (_move != Vector3.zero)
            {
                // rb.AddForce(movement * acceleration, ForceMode.Force);
                // rb.velocity = Vector3.ClampMagnitude(RbVelocityHorizontal(), walkSpeed) + RbVelocityVertical(); // !IMPORTANT! -> clamp de velocidad horizontal, manteniendo caída con gravedad
                
                rb.AddForce((movement - movementMngr.RbVelocityHorizontal(rb.velocity)), ForceMode.VelocityChange);
                
            }
            else
                if (isGrounded)
                    rb.AddForce(movementMngr.RbVelocityHorizontal(rb.velocity) * -movementMngr.deceleration, 
                        ForceMode.Force);
            
            transform.forward = Vector3.Slerp(transform.forward, camFwd, charT);
            
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), charT);
            // transform.rotation = Quaternion.LookRotation(movement);
        
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
        
        /*#region UTILS
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
    
        #endregion*/
        
        #region TRIGGERS & COLLIDERS
        // posibilidad de reemplazar con raycast (agnóstico a tipo de tag o layermask).
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
                isGrounded = true;

        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
                isGrounded = false;
        
        }
    
        #endregion
    
    }
    
}
