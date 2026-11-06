using System;
using _Alex.Scripts.Temp;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Alex.Scripts
{
    public class PickupManager : MonoBehaviour
    {
        [SerializeField] internal GameObject handle;
        [SerializeField] private SimpleSwordScript weaponScript;
        [SerializeField] private bool hasBeenDropped; // flag para evitar volver a coger el arma inmediatamente después de soltarla.
        [SerializeField] private GameObject pickupTrigger;
        [SerializeField] private TriggerController trigger;

    
        #region LIFECYCLE FUNC

        private void Awake()
        {
            weaponScript = GetComponent<SimpleSwordScript>();
            trigger = pickupTrigger.GetComponent<TriggerController>();
            
        }

        void Start()
        {
            EnableEvents();
        
        }

        void Update()
        {
            

        }

        private void OnEnable()
        {


        }

        private void OnDisable()
        {

            
        }

        #endregion

        private void OnEnter(Collider other)
        {
            if (hasBeenDropped) return;

            if (other.CompareTag("Player"))
            {
                var player = other.transform.parent.GetComponent<PlayerController>();
                if (player.sword != null) return; // comprueba si el jugador tiene un arma "equipada", y en caso de ser así evita que este arma sea recolectada
                
                DisableEvents();

                player.sword = this.gameObject;
                
                transform.SetParent(player.Hand);
                transform.position = player.Hand.position;
                transform.rotation = player.Hand.rotation;
                
                pickupTrigger.SetActive(false);
                weaponScript.enabled = true;
                
                gameObject.SetActive(false);
                
            }
            
        }

        private void OnExit(Collider other)
        {
            if (other.CompareTag("Player") && hasBeenDropped)
                hasBeenDropped = false;
                
            
        }

        public void DropWeapon(float playerY)
        {
            transform.SetParent(null);
            // this.transform.position = playerPosition; 
            
            pickupTrigger.SetActive(true);
            hasBeenDropped = true;

            weaponScript.enabled = false;

            transform.position = new Vector3(transform.position.x, playerY, transform.position.z); // planes para reemplazar la asignación de la Y por habilitar físicas de rigidbody.
            transform.rotation = Quaternion.identity;
            
            EnableEvents();
            
        }

        private void EnableEvents()
        {
            trigger.TriggerIn += OnEnter;
            trigger.TriggerOut += OnExit;
            
        }
        
        private void DisableEvents()
        {
            trigger.TriggerIn -= OnEnter;
            trigger.TriggerOut -= OnExit;
            
        }
    
    }
    
}
