using UnityEngine;

namespace _Alex.Scripts.Temp
{
    public class TriggerController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Player on sight!");
        
        }
    
    }
}
