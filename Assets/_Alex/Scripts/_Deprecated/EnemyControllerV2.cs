using UnityEngine;

namespace _Alex.Scripts
{
    [RequireComponent(typeof(HealthManager))]
    
    public class EnemyControllerV2 : MonoBehaviour
    {
        [Header("-- MOVEMENT --")]
        public MovementManager movementMngr;
        [SerializeField] private bool doFollowTgt;
        
        [Header("GameObject Components")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private HealthManager healthMngr;
        
        #region LIFECYCLE FUNC

        private void Awake()
        {
            healthMngr = gameObject.GetComponent<HealthManager>();
        
            if (rb == null)
                rb = GetComponent<Rigidbody>();
        
        }

        void Start()
        {
        
        
        }

        void Update()
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, movementMngr.speed);
        
        }

        protected void FixedUpdate()
        {
            Movement();
            
        }

        #endregion

        private void Movement()
        {
            if (doFollowTgt)
            {
                // _rb.MovePosition(targetPos.position * speed * Time.fixedDeltaTime);
                // Debug.Log(targetPos.position);
            
                // weird movement behaviour
                // _rb.AddRelativeForce(transform.forward * speed * Time.fixedDeltaTime, ForceMode.Force);
            
                // rb.AddForce(transform.forward * speed, ForceMode.Force);
            
                // Debug.Log(_rb.velocity.magnitude);

                /*if (Vector3.Distance(targetPos.position, transform.position) > stopDistance)
                {
                    Vector3 travelDir = targetPos.position - transform.position;
                    travelDir.Normalize();

                    _rb.MovePosition(transform.position + (travelDir * speed * Time.fixedDeltaTime));

                }*/

                Vector3 movement = transform.forward * movementMngr.speed;
                movement = new Vector3(movement.x, 0, movement.z);
                rb.AddForce((movement - movementMngr.RbVelocityHorizontal(rb.velocity)), ForceMode.VelocityChange);

            } 
            else
                // rb.AddForce(rb.velocity * -speed, ForceMode.Force);
                rb.AddForce(movementMngr.RbVelocityHorizontal(rb.velocity) * -movementMngr.deceleration, 
                    ForceMode.Force);
            
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // transform.LookAt(other.gameObject.transform.position);
            
                // targetPos = other.transform;
                doFollowTgt = true;

            }
        
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                transform.LookAt(new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z)); // transform.position.y para que siempre mire hacia el horizonte (y no hacia arriba o abajo).

                // targetPos = other.transform;

            }
        
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
                doFollowTgt = false;
        
        }
    
    }
    
}
