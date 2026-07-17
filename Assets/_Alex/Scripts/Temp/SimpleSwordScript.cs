using System;
using UnityEngine;

namespace _Alex.Scripts.Temp
{
    public class SimpleSwordScript : MonoBehaviour
    {
        [SerializeField] private string tagTgt;
        [SerializeField] private float dmg = 10;

        [SerializeField] private TriggerController swordTrigger;
    
        #region LIFECYCLE FUNC

        private void Awake()
        {
        
        
        }

        void Start()
        {
        
        
        }

        void Update()
        {
        
        
        }

        private void OnEnable()
        {
            swordTrigger.TriggerIn += OnHit;

        }

        private void OnDisable()
        {
            swordTrigger.TriggerIn -= OnHit;
            
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(tagTgt))
            {
                other.gameObject.GetComponentInParent<HealthManager>().AlterHealth(dmg);
                gameObject.SetActive(false);
            
            }
        
        }
        
        private void OnHit(Collider other)
        {
            if (other.CompareTag(tagTgt))
            {
                other.gameObject.GetComponentInParent<HealthManager>().AlterHealth(dmg);
                gameObject.SetActive(false);
            
            }
        
        }
    
    }
    
}
