using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{
    // Component stuff
    protected Rigidbody2D _body;
    protected Animator _animator;
    protected SpriteRenderer _sr;
    protected BoxCollider2D _boxCollider;
    
    // Movement
    [SerializeField] protected float speed = 8f;
    
    // Attacking
    [SerializeField] protected float attackCooldown = 0.75f;
    [SerializeField] protected GameObject projectile;
    
    // Health and dealing with damage
    protected int _currentHealth;
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected int armor = 0;
    
    // Ability to do stuff
    protected bool _locked;

    protected void SharedAwake()
    {
        _currentHealth = maxHealth;
    }
    
    protected void SharedStart()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _locked = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void BeginDeath()
    {
        throw new NotImplementedException();
    }

    protected virtual void OverrideTakeDamage()
    {
        // TODO: Is this a bad way to handle this type of problem?
        // Issue here is that when TakeDamage is called I need the player to update the UI with their new health
        // but enemies don't need to do anything here
        throw new NotImplementedException();
    }
    
    internal void TakeDamage(float damageToTake)
    {
        if (damageToTake <= 0 || _currentHealth <= 0) return;
        // TODO: Reconcile float damage vs int health
        _currentHealth -= (int)damageToTake;
        OverrideTakeDamage();
        
        // TODO: There should be an invulnerability window here where the entity cannot be damaged again
        // TODO: Play a blink animation or something until the invulnerability window ends
        
        if (_currentHealth <= 0)
        {
            // We don't want our mob moving or trying to do anything while their death animation is playing
            _locked = true;
            _body.velocity = new Vector2(0, 0);
            BeginDeath();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            // TODO: On the projectile controller, need to store some sort of IFF info
            //   which I guess is set when creating the projectile?
            //   So players/enemies don't get hurt by a projectile from their "team"
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

    internal int GetHealth()
    {
        return _currentHealth;
    }
}
