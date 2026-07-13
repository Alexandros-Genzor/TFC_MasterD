using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace _Alex.Scripts
{
    [Serializable]
    public class Effect : MonoBehaviour
    {
        public EffectType effectType;
        public float healthValue;
        
        public EffectDuration effectDuration;
        public int duration;
        [SerializeField] internal float healthValueTick;
        private float _healthValueTickCumulative = 0;
        public float HealthValueTick { get => healthValueTick; private set => healthValueTick = value; }
    
        #region LIFECYCLE FUNC
        void Start()
        {
            // en caso de establecer una duración de 0 automáticamente convertirá el efecto progresivo o cumulativo en instantáneo.
            if (duration <= 0) duration = 1;
            
            healthValueTick = healthValue / duration;

        }

        void Update()
        {
            // here be dragons
        
        }
    
        #endregion

        public float ApplyHealthValue()
        {
            switch (effectDuration)
            {
                case EffectDuration.Instant:
                    return healthValue;
                
                case EffectDuration.Progressive:
                    return healthValueTick;
                
                case EffectDuration.Cumulative:
                    return _healthValueTickCumulative+=healthValueTick;
                
                default:
                    return 0;

            }
            
        }

        public void ResetCumulativeHealthValue()
        {
            _healthValueTickCumulative = 0;

        }
    
    }
    
    public enum EffectType
    {
        Damaging,
        Healing
    }
    
    public enum EffectDuration
    {
        Instant,
        Progressive,
        Cumulative
        
    }
    
        
    [CustomEditor(typeof(Effect))]
    public class HealthPackInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (Effect)target;

            script.effectType = (EffectType)EditorGUILayout.EnumPopup("Effect Type", script.effectType);
            script.healthValue = EditorGUILayout.FloatField("Health Value", script.healthValue);
            script.effectDuration = (EffectDuration)EditorGUILayout.EnumPopup("Effect Duration", script.effectDuration);

            if (script.effectDuration == EffectDuration.Instant) return;

            script.duration = EditorGUILayout.IntField("Duration", script.duration);
            script.healthValueTick = EditorGUILayout.FloatField("Health Value Per Tick", script.HealthValueTick);

        }

    }
    
}
