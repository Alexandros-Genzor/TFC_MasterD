using System.Collections;
using _Alex.Scripts.Scriptables;
using UnityEngine;

namespace _Alex.Scripts
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField] private float minHealth = 0;
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float health = -1;
        public float Health {get => health; set => health = Mathf.Clamp(value, minHealth, maxHealth);}
        public float MaxHealth {get => maxHealth;}
    
        #region LIFECYCLE FUNC
        void Start()
        {
            // si health es -1 automáticamente adquiere el valor de maxHealth. En caso de establecer una salud inicial del personaje.
            if (Health == -1)
                Health = maxHealth;

        }

        void Update()
        {
            if (Health <= 0)
                this.gameObject.SetActive(false);
        
        }
    
        #endregion
    
        /// <summary>
        /// Modifica el valor de vida del personaje mediante valores arbitrarios.
        /// </summary>
        /// <param name="healthChange">Valor para modificar la vida.</param>
        /// <param name="isHealing">Define si "healthChange" es daño o curación (default: daño).</param>
        public void AlterHealth(float healthChange, bool isHealing = false)
        {
            Health += healthChange * (isHealing ? 1 : -1);
        
        }
        
        /// <summary>
        /// Modifica el valor de vida del personaje mediante un efecto.
        /// </summary>
        /// <param name="ef">Efecto de alteración de vida.</param>
        public void AlterHealth(Effect ef)
        {
            // Debug.Log(ef.ApplyHealthValue());
            
            Health += ef.ApplyHealthValue() * ((ef.effectType == EffectType.Healing) ? 1 : -1);

        }

        /// <summary>
        /// <p>Manager del sistema de salud del personaje, encargado de evaluar el efecto por su tipo de duración.</p>
        /// <p>En caso de ser un efecto de duración instantánea, inmediatamente llamará al método AlterHealth().</p>
        /// En caso de ser progresivo o cumulativo, iniciará una corrutina encargada de aplicar el cambio de salud en el periodo de tiempo establecido por el efecto.
        /// </summary>
        /// <param name="ef">Efecto.</param>
        public void ManageHealth(Effect ef)
        {
            if (ef.effectDuration == EffectDuration.Instant)
                AlterHealth(ef);
            else
                StartCoroutine(IAlterHealth(ef));
            
        }

        // intento de hacer un sistema de alteracion de salud por tiempo
        public IEnumerator IAlterHealth(Effect ef)
        {
            for (int i = 0; i < ef.duration; i++)
            {
                AlterHealth(ef);
                Debug.Log("tick!");
                
                if (health == maxHealth || health <= minHealth) yield break;
                
                yield return new WaitForSeconds(1f);
            
            }

        }
    
    }
    
}
