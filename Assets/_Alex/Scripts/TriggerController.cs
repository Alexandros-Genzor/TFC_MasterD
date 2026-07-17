using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Alex.Scripts.Temp
{
    public class TriggerController : MonoBehaviour
    {
        public event System.Action<Collider> TriggerIn;
        public event System.Action<Collider> TriggerOut;
        
        // public UnityEvent<Collider> TriggerIn;
        // public UnityEvent<Collider> TriggerOut;
        
        public void OnTriggerEnter(Collider other)
        {
            TriggerIn?.Invoke(other);
        
        }

        public void OnTriggerExit(Collider other)
        {
            TriggerOut?.Invoke(other);
            
        }
        
    }
    
}
