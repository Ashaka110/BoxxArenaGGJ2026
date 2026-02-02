using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Animator _animator;

    [SerializeField] private float health;

    [SerializeField] private NavMeshAgent Agent;

    private bool _isDead;
    
    
    private float damageAnim;
    void Start()
    {
        
        OnHit(1);
    }

    void Update()
    {
        if(_isDead) return;
        if (damageAnim > 0)
        {
            damageAnim -= Time.deltaTime;
            _renderer.material.SetFloat("_DamageAnim", damageAnim);
        }
        else
        {
            _animator.ResetTrigger("Hit");

            if (Physics.Raycast(transform.position, Player.Instance.transform.position - transform.position, out var hit))
            {
                if (hit.transform.GetComponent<Player>())
                {
                    Agent.SetDestination(Player.Instance.transform.position);
                }
            }
        }
        
    }

    public void OnHit(float damage)
    {
        AudioManager.Instance.PlaySound(0);
        health -= damage;
        damageAnim = 1;

         if (health < 0)
         {
             Agent.enabled = false;
             _animator.SetBool("Dead",true);
             _isDead = true;
         }
         else
         {
             _animator.SetTrigger("Hit");
             Agent.SetDestination(transform.position);
         }
    }

    public void SetNavEnabled()
    {
        Agent.enabled = true;
    }
    public void SetNavDisabled()
    {
        Agent.enabled = false;
    }

    public bool IsAlive()
    {
        return health > 0;
    }
}
