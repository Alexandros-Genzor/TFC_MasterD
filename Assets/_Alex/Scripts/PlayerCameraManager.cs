using System;
using UnityEngine;

namespace _Alex.Scripts
{
    public class PlayerCameraManager : MonoBehaviour
    {
        [Header("Camera & Rotation")]
        public Camera cam;
        public float camSensitivity;
        // multiplicador de sensibilidad de la cámara con joystick
        public float camGamepadMult = 3;
        // aplica un offset de distancia al ancla de la cámara respecto al eje (si se usa provoca que la cámara se atasque).
        public Vector3 camOffset;
    
        public Transform camTgt;
        [Obsolete] public Transform camAnchor;
    
        private Vector2 _rotation;
    
        // controla el trailing (velocidad del lerp) de la cámara respecto a su anchor.
        public float camT;
        // controla el trailing (velocidad del lerp) del body respecto al forward de la cámara.
        public float charT;
    
        // límite de depresión y elevación de la cámara.
        public float lowerLimitV, upperLimitV;
    
        public Vector3 CamFwd { get => camTgt.forward; }
    
        #region LIFECYCLE FUNC
        void Start()
        {
            if (cam == null) cam = Camera.main;
        
        }

        void Update()
        {
        
        
        }

        private void LateUpdate()
        {
        
        
        }

        #endregion
    
        // Para usar con cámara de CineMachine
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheme">Recibe el nombre del esquema de controles en uso actualmente para poder aplicar un multiplicador de sensibilidad en caso de usar un esquema de gamepad.</param>
        /// <param name="look">Recibe los valores de entrada de control correspondientes a la acción de mirar.</param>
        public void CameraControls(string scheme, Vector2 look)
        {
            // camAnchor.transform.position = camOffset + camTgt.transform.position;
            Vector3 desiredPos = camOffset + camTgt.transform.position;
            
            // aplica el multiplicador de sensibilidad de la cámara cuándo detecta que el esquema de entrada actual es un gamepad.
            _rotation += look * (camSensitivity * (scheme == "Controller" ? camGamepadMult : 1));
            // limita el ángulo de elevación / depresión de la cámara (eliminar cuándo se use cámara de cinemachine con springarm).
            _rotation.y = Mathf.Clamp(_rotation.y, lowerLimitV, upperLimitV);
            
            // camTgt.rotation = Quaternion.Euler(_rotation.y, _rotation.x, camTgt.eulerAngles.z);
            camTgt.transform.eulerAngles = new Vector3(_rotation.y, _rotation.x, 0);
        
            Debug.Log(scheme);

        }
    
        [Obsolete]
        // Para usar con sistema casero de cámara.
        public void CameraControlsOld(string scheme, Vector2 look)
        {
            // camAnchor.transform.position = camOffset + camTgt.transform.position;
            Vector3 desiredPos = camOffset + camTgt.transform.position;
            
            // aplica el multiplicador de sensibilidad de la cámara cuándo detecta que el esquema de entrada actual es un gamepad.
            _rotation += look * (camSensitivity * (scheme == "Controller" ? camGamepadMult : 1));
            // limita el ángulo de elevación / depresión de la cámara (eliminar cuándo se use cámara de cinemachine con springarm).
            _rotation.y = Mathf.Clamp(_rotation.y, lowerLimitV, upperLimitV);
            
            camTgt.transform.eulerAngles = new Vector3(_rotation.y, _rotation.x, 0);
        
            // cam.transform.position = Vector3.Slerp(cam.transform.position, camAnchor.transform.position, camT);
            // cam.transform.position = desiredPos;
            // cam.transform.position = camAnchor.position;
            cam.transform.position = camTgt.position;

        
        
            // cam.transform.LookAt(camTgt.position);
            
            Debug.Log(scheme);

        }
    
    }
}
