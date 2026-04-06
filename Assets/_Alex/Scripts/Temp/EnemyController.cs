using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody _rb;

    public Transform targetPos;
    public bool doFollowTarget;

    public float speed = 1; // renombrar a acceleration
    public float stopDistance;
    public float maxSpeed = 3;
    
    #region LIFECYCLE FUNC

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
    }

    void Start()
    {
        
        
    }

    void Update()
    {
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed);
        
    }

    private void FixedUpdate()
    {
        if (doFollowTarget)
        {
            // _rb.MovePosition(targetPos.position * speed * Time.fixedDeltaTime);
            // Debug.Log(targetPos.position);
            
            // weird movement behaviour
            // _rb.AddRelativeForce(transform.forward * speed * Time.fixedDeltaTime, ForceMode.Force);
            _rb.AddForce(transform.forward * speed, ForceMode.Force);
            
            // Debug.Log(_rb.velocity.magnitude);

            /*if (Vector3.Distance(targetPos.position, transform.position) > stopDistance)
            {
                Vector3 travelDir = targetPos.position - transform.position;
                travelDir.Normalize();
            
                _rb.MovePosition(transform.position + (travelDir * speed * Time.fixedDeltaTime));
                
            }*/
            
        } 
        else
            _rb.AddForce(_rb.velocity * -speed, ForceMode.Force);
        
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        
        if (other.gameObject.CompareTag("Player"))
        {
            // transform.LookAt(other.gameObject.transform.position);
            
            // targetPos = other.transform;
            doFollowTarget = true;

        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.LookAt(new Vector3(other.transform.position.x, 0, other.transform.position.z));

            targetPos = other.transform;

        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            doFollowTarget = false;
        
    }
    
}
