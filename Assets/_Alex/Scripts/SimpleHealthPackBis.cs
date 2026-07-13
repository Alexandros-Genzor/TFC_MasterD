using System;
using _Alex.Scripts.Temp;
using UnityEngine;

namespace _Alex.Scripts
{
    [RequireComponent(typeof(Effect))]
    
    // contenedor de efecto y encargado de evaluar colisiones y triggers
    public class SimpleHealthPackBis : MonoBehaviour
    {
        [SerializeField] private Effect effect;
        [SerializeField] private TriggerController pickupTrigger;
    
        #region LIFECYCLE FUNC

        void Awake()
        {
            effect = GetComponent<Effect>();
            
        }

        void Start()
        {
        
        
        }

        void Update()
        {
        
        
        }

        void OnEnable()
        {
            pickupTrigger.TriggerIn += OnPickupEnter;
            // pickupTrigger.TriggerIn.AddListener(OnPickupEnter);
            effect.ResetCumulativeHealthValue();
            
        }

        private void OnDisable()
        {
            pickupTrigger.TriggerIn -= OnPickupEnter;
            // pickupTrigger.TriggerIn.RemoveListener(OnPickupEnter);
            
        }

        #endregion

        void OnPickupEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent<HealthManager>(out HealthManager hm))
            {
                if (hm.Health >= hm.MaxHealth && effect.effectType == EffectType.Healing) return;
                hm.ManageHealth(effect);
                
                this.gameObject.SetActive(false);
                
            }
            
        }
    
        /*private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent<HealthManager>(out HealthManager hm))
            {
                if (hm.Health >= hm.MaxHealth && effect.effectType == EffectType.Healing) return;
                    hm.ManageHealth(effect);
                
                    transform.parent.gameObject.SetActive(false);
                
            }
            
        }*/
        
    }
    
}
