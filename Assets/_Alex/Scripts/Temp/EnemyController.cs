using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("GameObject Components")]
    [SerializeField] private Rigidbody rb;

    [Header("Movement")]
    public Transform targetPos;
    public bool doFollowTarget;

    public float speed = 1; // renombrar a acceleration
    // public float stopDistance;
    public float maxSpeed = 3;


    [Header("Health & Damage")] 
    public float minHealth = 0;
    public float maxHealth = 100;
    [SerializeField] private float health;
    public float Health {get => health; set => health = Mathf.Clamp(value, minHealth, maxHealth);}
    // temp
    public bool gotDmg = false;
    public float dmg = 10; // temporal
    
    #region LIFECYCLE FUNC

    private void Awake()
    {
        // rb = GetComponent<Rigidbody>();
        
    }

    void Start()
    {
        health = maxHealth;
        
    }

    void Update()
    {
        if (gotDmg)
        {
            AlterHealth(dmg);
            gotDmg = false;

        }
        
        if (health <= 0)
            gameObject.SetActive(false);
        
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        
    }

    private void FixedUpdate()
    {
        if (doFollowTarget)
        {
            // _rb.MovePosition(targetPos.position * speed * Time.fixedDeltaTime);
            // Debug.Log(targetPos.position);
            
            // weird movement behaviour
            // _rb.AddRelativeForce(transform.forward * speed * Time.fixedDeltaTime, ForceMode.Force);
            rb.AddForce(transform.forward * speed, ForceMode.Force);
            
            // Debug.Log(_rb.velocity.magnitude);

            /*if (Vector3.Distance(targetPos.position, transform.position) > stopDistance)
            {
                Vector3 travelDir = targetPos.position - transform.position;
                travelDir.Normalize();
            
                _rb.MovePosition(transform.position + (travelDir * speed * Time.fixedDeltaTime));
                
            }*/
            
        } 
        else
            rb.AddForce(rb.velocity * -speed, ForceMode.Force);
        
    }

    #endregion
    
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

    private void OnTriggerEnter(Collider other)
    {
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
            transform.LookAt(new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z)); // transform.position.y para que siempre mire hacia el horizonte (y no hacia arriba o abajo).

            targetPos = other.transform;

        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            doFollowTarget = false;
        
    }

    private void OnCollisionEnter(Collision other)
    {
        // reemplazar con eventos o mensajes
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerController>().AlterHealth(dmg);
        
    }
    
}
