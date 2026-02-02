using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]private float speed;
    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        var en = other.gameObject.GetComponent<Enemy>();
        if (en != null)
        {
            en.OnHit(1);
        } 
        var bo = other.gameObject.GetComponent<Boss>();
        if (bo != null)
        {
            bo.OnHit(1);
        }
        
        Destroy(this.gameObject);
    }
}
