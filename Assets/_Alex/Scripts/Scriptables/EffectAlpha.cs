using System;
using UnityEngine;

namespace _Alex.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Scriptable Objects/Effect")]
    public class EffectAlpha : ScriptableObject
    {
        public enum EffectType
        {
            Damaging,
            Healing
        }
        
        public EffectType effectType;
        public float healthValue;
        public int duration;
        
    }
    
}
