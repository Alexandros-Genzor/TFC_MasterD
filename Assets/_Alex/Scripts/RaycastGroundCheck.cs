using UnityEngine;

namespace _Alex.Scripts
{
    public class RaycastGroundCheck : MonoBehaviour
    {
        public float rayLength = 0.5f;
        public LayerMask layer;
        public new string tag = "Untagged";

        [SerializeField] private bool isGrounded;
 
        [SerializeField] private Vector3 originOffset;
        [SerializeField] private bool doUseLayer = false;
    
        #region LIFECYCLE FUNC
        void Start()
        {
        
        
        }

        void Update()
        {
            Debug.DrawRay(transform.position + originOffset, Vector3.down, Color.red);
        
        }
    
        #endregion
            
        public bool IsGrounded()
        {
            bool res = false;
            
            if (Physics.Raycast(transform.position + originOffset, Vector3.down, out RaycastHit hit, rayLength))
            {
                GameObject hitObj = hit.collider.gameObject;
                
                res = (doUseLayer ? hitObj.layer == layer.value : hitObj.CompareTag(this.tag));
                
            }

            return res;

        }
    
    }
    
}
