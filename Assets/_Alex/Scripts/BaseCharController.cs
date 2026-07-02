using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _Alex.Scripts
{
    [RequireComponent(typeof(HealthManager))]

    public abstract class BaseCharController : MonoBehaviour
    {
        [Header("-- PARENT --")]
        [Header("GameObject Components")]
        [SerializeField] protected Rigidbody rb;
        [SerializeField] protected HealthManager health;
    
        [Header("Movement")]
        [SerializeField] protected MovementManager MovementMngr;
    
        #region LIFECYCLE FUNC

        protected virtual void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody>();
            if (health == null) health = GetComponent<HealthManager>();
            
        }

        protected virtual void Start()
        {

        
        }

        protected virtual void Update()
        {

        
        }

        protected virtual void FixedUpdate()
        {
        
        
        }
    
        #endregion

        protected virtual void Movement(bool condition)
        {
            if (condition)
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

                Vector3 movement = transform.forward * MovementMngr.speed;
                movement = new Vector3(movement.x, 0, movement.z);
                rb.AddForce((movement - MovementMngr.RbVelocityHorizontal(rb.velocity)), ForceMode.VelocityChange);

            } 
            else
                // rb.AddForce(rb.velocity * -speed, ForceMode.Force);
                rb.AddForce(MovementMngr.RbVelocityHorizontal(rb.velocity) * -MovementMngr.deceleration, 
                    ForceMode.Force);
            
        } 
    
    }
    
}
