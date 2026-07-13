using System;
using _Alex.Scripts.Scriptables;
using UnityEditor;
using UnityEngine;

namespace _Alex.Scripts
{
    /*public enum EffectDuration
    {
        Instant,
        Progressive,
        Cumulative
        
    }*/
    
    public class SimpleHealthPack : MonoBehaviour
    {
        public float healthValue = 5;
        // public Effect effect;
        public EffectType effect;
        public EffectDuration effectDuration;
        [HideInInspector] public int duration;
    
        #region LIFECYCLE FUNC
        void Start()
        {
        
        
        }

        void Update()
        {
        
        
        }
    
        #endregion

        /*private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                if (other.transform.parent.TryGetComponent<HealthManager>(out HealthManager hm))
                {
                    if (hm.Health >= hm.MaxHealth) return;
                
                    hm.AlterHealth(healValue, true);
                    this.gameObject.SetActive(false);
                    
                }
        
        }*/

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent<HealthManager>(out HealthManager hm))
            {
                if (hm.Health >= hm.MaxHealth && effect == EffectType.Healing) return;
                bool isHealing = effect == EffectType.Healing;
                /*if (effect.duration <= 0)
                    hm.AlterHealth(effect.healthValue, (effect.effectType == Effect.EffectCoreType.Healing));
                else
                    StartCoroutine(hm.IAlterHealth(effect));*/
                
                // hm.ManageHealth(this);

                
                this.gameObject.SetActive(false);
                
            }
            
        }
        
    }

    /*[CustomEditor(typeof(SimpleHealthPack))]
    public class HealthPackInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (SimpleHealthPack)target;
            
            script.healthValue = EditorGUILayout.FloatField("Health Value", script.healthValue);
            script.effect = (EffectType)EditorGUILayout.EnumPopup("Effect Type", script.effect);
            script.effectDuration = (EffectDuration)EditorGUILayout.EnumPopup("Effect Duration", script.effectDuration);

            if (script.effectDuration == EffectDuration.Instant) return;
            
            script.duration = EditorGUILayout.IntField("Duration", script.duration);

        }
        
    }*/
    
}
