using UnityEngine;

namespace _Alex.Scripts
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField] private float minHealth = 0;
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float health;
        public float Health {get => health; set => health = Mathf.Clamp(value, minHealth, maxHealth);}
    
        #region LIFECYCLE FUNC
        void Start()
        {
            Health = maxHealth;

        }

        void Update()
        {
            if (Health <= 0)
                this.gameObject.SetActive(false);
        
        }
    
        #endregion
    
        /// <summary>
        /// Modifica el valor de vida del personaje.
        /// </summary>
        /// <param name="healthChange">Valor para modificar la vida.</param>
        /// <param name="isHealing">Define si "healthChange" es daño o curación (default: daño).</param>
        public void AlterHealth(float healthChange, bool isHealing = false)
        {
            Health += healthChange * (isHealing ? 1 : -1);
        
        }
    
    }
    
}
