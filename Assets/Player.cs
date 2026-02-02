using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [SerializeField] CharacterController controller;
    [SerializeField] private Projectile _projectile;
    
    [SerializeField] Transform CameraXZRoot;
    [SerializeField] Transform CameraRoot;
    
    [SerializeField] float Speed = 5f; 
    [SerializeField] float MouseSensitivityX = 50f;

    private float _cameraY;
    private float YVelocity;
    [SerializeField] private float JumpSpeed = 5;
    [SerializeField] private float Gravity;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        controller.Move(transform.forward * Input.GetAxis("Vertical") * Time.deltaTime*Speed);
        controller.Move(transform.right * Input.GetAxis("Horizontal") * Time.deltaTime*Speed);
        
        transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * Time.deltaTime * MouseSensitivityX, 0f);
        _cameraY += Input.GetAxis("Mouse Y") * Time.deltaTime * MouseSensitivityX;
        _cameraY = Mathf.Clamp(_cameraY, -90f, 90f);
        
        CameraRoot.localRotation = Quaternion.Euler(-_cameraY, 0f, 0f); 

        bool grounded = Physics.Raycast(transform.position, Vector3.down, .01f);

        if (grounded) YVelocity = 0;
        else
        {
            YVelocity -= Gravity * Time.deltaTime;}
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            YVelocity = JumpSpeed;
        }

        var flags = controller.Move(Vector3.up * YVelocity * Time.deltaTime);

        if ((flags & CollisionFlags.CollidedBelow) != 0)
        {
            YVelocity = 0;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var p = Instantiate(_projectile, CameraRoot.position, CameraRoot.rotation);
            p.transform.position = CameraRoot.position + CameraRoot.forward * 1f;
            AudioManager.Instance.PlaySound(1);


        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
