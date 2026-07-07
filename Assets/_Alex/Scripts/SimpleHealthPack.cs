using UnityEngine;

namespace _Alex.Scripts
{
    public class SimpleHealthPack : MonoBehaviour
    {
        public float healValue = 5;
    
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
            if (other.CompareTag("Player"))
                if (other.transform.parent.TryGetComponent<HealthManager>(out HealthManager hm))
                {
                    if (hm.Health >= hm.MaxHealth) return;
                
                    hm.AlterHealth(healValue, true);
                    this.gameObject.SetActive(false);
                    
                }
        
        }
    
    }
    
}
