using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableChar : MonoBehaviour
{
    [Header("-- Camera --")]
    [LabelOverride("Camera")] public Camera cam;
    [LabelOverride("Mouse sensitivity V")] public float sensitivityH = 1;
    [LabelOverride("Mouse sensitivity H")] public float sensitivityV = 1;
    [LabelOverride("Camera smoothness")] public float t;
    [LabelOverride("Camera offset")] public Vector3 offset;
    private float _rotV;
    private float _rotH;
    
    [Header("-- Character --")]
    [LabelOverride("Char speed")] public float speed;
    [LabelOverride("Character rotation speed")] public float charRotSpeed;

    [Header("-- Health --")] 
    public float minHealth;
    public float maxHealth;
    private int _health;
    public int Health { get { return _health; } set { _health = value; } }

    [Header("-- Misc --")]
    [LabelOverride("Got hurt?")] public bool gotDmg;
    [LabelOverride("Camera position target")] public GameObject tgt;
    [LabelOverride("Character axis target")] public GameObject axisTgt;

    public void AlterHealth(float healthVal)
    {
        Health += (int)healthVal;

    }

    public void Movement()
    {
        Vector3 fwdCam = cam.transform.forward;
        fwdCam.y = 0;
        transform.forward = Vector3.Slerp(transform.forward, fwdCam, charRotSpeed * Time.deltaTime);
        
        Vector3 fwd = transform.forward * Input.GetAxis("Vertical");
        Vector3 rgt = transform.right * Input.GetAxis("Horizontal");
        
        transform.position += (fwd + rgt) * speed * Time.deltaTime;
        
        // transform.position += transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        // transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        
    }

    public void CamControl()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, tgt.transform.position, 
            Mathf.SmoothStep(0, 1, t));
        
        _rotV += Input.GetAxis("Mouse Y") * sensitivityV;
        _rotH += Input.GetAxis("Mouse X") * sensitivityH;

        axisTgt.transform.eulerAngles = new Vector3(/*/Mathf.Clamp(_rotV, -35, 45)/**/0, _rotH, 0);
        
        cam.transform.LookAt(transform.position + offset);

    }
    
    #region LIFECYCLE_FUNCTIONS
    void Start()
    {
        minHealth = 0;
        maxHealth = 100;
        
        Health = (int)maxHealth;
        
        cam = Camera.main;

    }
    
    void Update()
    {
        Movement();
        CamControl();
        
        if (gotDmg)
        {
            AlterHealth(-10);
            gotDmg = false;

        }
        
    }
    
    #endregion
    
}
