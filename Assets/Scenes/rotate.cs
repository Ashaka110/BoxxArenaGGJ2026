using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private float rotation;
    
    void Start()
    {
        
    }

    void Update()
    {
        rotation += Time.deltaTime*speed;
        transform.localRotation = Quaternion.Euler(0, rotation, 0);
    }
}
