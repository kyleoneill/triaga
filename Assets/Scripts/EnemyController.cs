using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    // TODO: There is a lot of overlap between this stuff and the player. Might want some sort of common class they both inherit from
    private Rigidbody2D _body;
    private Animator _animator;
    private SpriteRenderer _sr;
    private BoxCollider2D _boxCollider;

    [SerializeField] private float speed = 8f;
    [SerializeField] private float attackCooldown = 0.75f;
    [SerializeField] private float actionCooldown = 1f;
    
    private AttackController _attackController;
    
    private int _currentHealth;
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int armor = 0;

    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject[] loot;

    private int _actionsTaken;
    private bool _locked;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _currentHealth = maxHealth;
        _attackController = gameObject.AddComponent<AttackController>();
        _locked = false;
        _actionsTaken = 0;
    }

    private float elapsed = 0f;
    // Update it called once per frame
    void Update()
    {
        // If we're locked then an animation is playing and we do not want to interrupt it
        if (_locked) return;
        
        // We only want to do one action per cooldown window
        elapsed += Time.deltaTime;
        if (elapsed >= actionCooldown)
        {
            elapsed %= actionCooldown;
            
            // TODO: It might be better to just tear down this weight system and make it a flat random chance to move or attack
            
            // The more we take actions, the more likely we are to move
            int weight = _actionsTaken * Random.Range(0, 10);
            if(weight > 10)
                Move();
            else
                Attack();
            _actionsTaken += 1;
            
            // Reset the weight after a few actions
            if (_actionsTaken > 10)
                _actionsTaken = 0;
        }
    }

    private void Move()
    {
        // TODO: More complex moving logic then just a 50/50 to move left/right?
        // TODO: How to handle walking into a wall or another enemy?
        _locked = true;
        int direction = Random.Range(0, 2);
        _animator.Play("Moving");
        if (direction == 0)
        {
            // Move left
            _body.velocity = new Vector2(-0.3f, 0) * speed;
        }
        else
        {
            // Move right
            _body.velocity = new Vector2(0.3f, 0) * speed;
        }
    }

    private void Attack()
    {
        // Lock the enemy so the attack animation can play. The Unlock method is called at the end of the Shooting animation
        _locked = true;
        _animator.Play("Shooting");
    }

    // This method is called halfway through the Shooting animation
    private void FireArrow()
    {
        _attackController.ProjectileAttack(projectile, transform.position, _boxCollider.size.y, false, 0f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            var projectileController = other.gameObject.GetComponent<ProjectileController>();
            float damageToTake = projectileController.GetDamageDealt();
            DamageType damageType = projectileController.GetDamageType();
            if (damageType == DamageType.Physical)
            {
                damageToTake -= armor;
            }
            Destroy(other.gameObject);
            TakeDamage(damageToTake);
        }
    }

    internal void TakeDamage(float damageToTake)
    {
        if (damageToTake <= 0) return;
        // TODO: Reconcile float damage vs int health
        _currentHealth -= (int)damageToTake;
        if (_currentHealth <= 0)
        {
            BeginDeath();
        }
    }

    private void BeginDeath()
    {
        // We don't want the enemy moving or trying to do anything while their death animation is playing
        _locked = true;
        SpawnLoot();
        // The end of the explosion animation calls KillEnemy
        _animator.Play("Explosion");
    }

    private void SpawnLoot()
    {
        Instantiate(loot[0], gameObject.transform.position, Quaternion.identity);
    }
    
    // StopMoving is called by the end of the move animation
    internal void StopMoving()
    {
        _body.velocity = new Vector2(0, 0);
        Unlock();
    }
    
    // Unlock is called by the end of the attack animation
    private void Unlock()
    {
        _locked = false;
    }
    
    // KillEnemy is called by the end of the death animation
    private void KillEnemy()
    {
        Destroy(gameObject);
    }
}
