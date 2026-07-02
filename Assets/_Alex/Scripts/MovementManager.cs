using System;
using UnityEngine;

namespace _Alex.Scripts
{
    [Serializable]
    public struct MovementManager
    {
        public float speed;
        public float deceleration;
        public float jumpForce;

        public bool isGrounded;
        
        #region UTILS
        // poosiblemente obsoleto
        /// <summary>
        /// Devuelve los componentes horizontales de la velocidad del rigidbody (X, 0, Z).
        /// </summary>
        /// <returns></returns>
        public Vector3 RbVelocityHorizontal(Vector3 rbVelocity)
        {
            return new Vector3(rbVelocity.x, 0, rbVelocity.z);
        }

        // poosiblemente obsoleto
        /// <summary>
        /// Devuelve el componente vertical de la velocidad del rigidbody (0, Y, 0).
        /// </summary>
        /// <returns></returns>
        public Vector3 RbVelocityVertical(Vector3 rbVelocity)
        {
            return new Vector3(0, rbVelocity.y, 0);
        }
        
        /// <summary>
        /// Devuelve los componentes horizontales de la velocidad del Vector3 (X, 0, Z).
        /// </summary>
        /// <returns></returns>
        public Vector3 GetVelocityHorizontal(Vector3 velocity)
        {
            return new Vector3(velocity.x, 0, velocity.z);
        }

        /// <summary>
        /// Devuelve el componente vertical de la velocidad del Vector3 (0, Y, 0).
        /// </summary>
        /// <returns></returns>
        public Vector3 GetVelocityVertical(Vector3 velocity)
        {
            return new Vector3(0, velocity.y, 0);
        }
    
        #endregion
        
    }
    
}
