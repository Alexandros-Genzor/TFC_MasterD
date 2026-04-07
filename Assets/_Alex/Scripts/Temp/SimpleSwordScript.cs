using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSwordScript : MonoBehaviour
{
    [SerializeField] private string tagTgt;
    [SerializeField] private float dmg = 10;
    
    #region LIFECYCLE FUNC
    void Start()
    {
        
        
    }

    void Update()
    {
        
        
    }
    
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagTgt))
        {
            other.gameObject.GetComponent<EnemyController>().AlterHealth(dmg);
            gameObject.SetActive(false);
            
        }
        
    }
    
}
