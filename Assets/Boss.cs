using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private Animator _animator;
    private float damageAnim;
    [SerializeField]private float MaxHealth;
    [SerializeField]private float health;

    [SerializeField]float ActivationDistance;

    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private Enemy EnemyPrefab;
    private List<Enemy> spawnedEnemies;

    [SerializeField] private float SpawnCooldownTime = 10;
    private float _spawnCooldownTimer;

    [SerializeField] private int maxEnemies = 9;

    private Collider col;
    
    void Start()
    {
        spawnedEnemies = new List<Enemy>();
        col = GetComponent<Collider>();

    }

    private bool DestoryFlag;
    
    void Update()
    {
        if (damageAnim > 0)
        {
            damageAnim -= Time.deltaTime;
            foreach (var renderer in _renderers)
            {
                renderer.material.SetFloat("_DamageAnim", damageAnim);
            }
        }

        if (health < 0)
        {
            col.enabled = false;
            transform.position += Vector3.down * Time.deltaTime *3;
            damageAnim = 1;
            if (!DestoryFlag)
            {
                DestoryFlag = true;
                AudioManager.Instance.PlaySound(2);
                AudioManager.Instance.StopMusic();
                foreach (var enemy in spawnedEnemies)
                {
                    Destroy(enemy.gameObject);
                }
            }
        }

        if (PlayerInRange())
        {
            if (_spawnCooldownTimer < 0 && health > 0)
            {
                foreach (var enemy in spawnedEnemies.ToList())
                {
                    if (!enemy.IsAlive())
                    {
                        spawnedEnemies.Remove(enemy);
                    }
               
                }
                _spawnCooldownTimer = SpawnCooldownTime;
                if (spawnedEnemies.Count < maxEnemies)
                {
                    foreach (var spawnPoint in spawnPoints)
                    {
                        spawnedEnemies.Add(Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation));
                    }
                }
            }
            else
            {
                _spawnCooldownTimer -= Time.deltaTime;
            }
        }

        
    }

    public void OnHit(float damage)
    {
        AudioManager.Instance.PlaySound(0);
        health -= damage;
        damageAnim = 1;
    }

    public bool PlayerInRange()
    {
        return (Vector3.Distance(Player.Instance.transform.position, transform.position) < ActivationDistance);
    }

    public float GetMaxHealth() { return MaxHealth; }
    public float GetHealth() { return health; }
    
}
