using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Alex.Scripts
{
    [RequireComponent(typeof(PlayerCameraManager))]
    
    public class PlayerController : BaseCharController
    {
        [Header("-- CHILD --")]
        // almacena el valor del control de movimiento de la cámara.
        private Vector2 _look;
        // almacena el valor de los controles de desplazamiento.
        private Vector3 _move;

        [Header("-- TEMP --")]
        // temporal
        [SerializeField] private GameObject sword;
        private bool _isAttacking;
    
        [Header("GameObject Components")]
        [SerializeField] private PlayerInput playerIn;
        [SerializeField] private PlayerCameraManager playerCam;
        // [SerializeField] private HealthManager healthMngr;
    
        #region LIFECYCLE FUNC

        protected override void Awake()
        {
            // Llama al awake del padre para obtener todos los componentes comunes.
            base.Awake();
            
            playerIn = GetComponent<PlayerInput>();
            playerCam = GetComponent<PlayerCameraManager>();
            
        }

        protected override void Start()
        {
            
        
        }

        protected override void Update()
        {
            

        }

        protected override void FixedUpdate()
        {
            Movement();
        
        }

        private void LateUpdate()
        {
            playerCam.CameraControls(playerIn.currentControlScheme, _look);
            
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
            if (context.started && MovementMngr.isGrounded)
            {
                rb.AddForce(0, /*Vector3.up * */MovementMngr.jumpForce, 0, ForceMode.Impulse);
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

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            // para evitar cualquier posible overlap
            if (_isAttacking)
                StopCoroutine(nameof(IAttack));

            StartCoroutine(nameof(IAttack));

        }

        /*public void OnTestKnockback(InputAction.CallbackContext context)
        {
            if (context.started)
                rb.AddForce(-transform.forward * 8, ForceMode.Impulse);
        
        }*/
    
        #endregion
        
        IEnumerator IAttack()
        {
            sword.SetActive(true);
            _isAttacking = true;
            yield return new WaitForSeconds(2);
            sword.SetActive(false);
            _isAttacking = false;

        }
        
        public void Movement()
        {
            Vector3 camFwd = playerCam.CamFwd;
            camFwd.y = 0;

            // Vector3 movement = transform.TransformDirection(_move) * acceleration;
            Vector3 movement = transform.TransformDirection(_move) * MovementMngr.speed;
            
            // rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z); // controla el movimiento del personaje atacando directamente a la velocidad del rigidbody
            
            // controla el movimiento del personaje a base de ejercer una aceleración en la direccion del personaje (+ clamp de velocidad) -> reemplazado temporalmente por ForceMode.VelocityChange.
            if (_move != Vector3.zero)
            {
                // rb.AddForce(movement * acceleration, ForceMode.Force);
                // rb.velocity = Vector3.ClampMagnitude(RbVelocityHorizontal(), walkSpeed) + RbVelocityVertical(); // !IMPORTANT! -> clamp de velocidad horizontal, manteniendo caída con gravedad
                
                rb.AddForce((movement - MovementMngr.RbVelocityHorizontal(rb.velocity)), ForceMode.VelocityChange);
                
            }
            else
                if (MovementMngr.isGrounded)
                    rb.AddForce(MovementMngr.RbVelocityHorizontal(rb.velocity) * -MovementMngr.deceleration, 
                        ForceMode.Force);
            
            // transform.forward = Vector3.Slerp(transform.forward, camFwd, charT);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(camFwd), playerCam.charT));
            
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), charT);
            // transform.rotation = Quaternion.LookRotation(movement);
        
        }
        
        #region TRIGGERS & COLLIDERS
        // posibilidad de reemplazar con raycast (agnóstico a tipo de tag o layermask).
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
                MovementMngr.isGrounded = true;

        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
                MovementMngr.isGrounded = false;
        
        }
    
        #endregion
    
    }
    
}
