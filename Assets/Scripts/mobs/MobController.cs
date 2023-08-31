using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackState
{
    NotAttacking,
    Attack,
    Cooldown
}

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
    protected AttackState _attackState;
    
    // Health and dealing with damage
    protected int _currentHealth;
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected int armor = 0;
    
    // Ability to do stuff
    protected bool _locked;
    protected bool _isPlayer;

    protected void SharedAwake()
    {
        _currentHealth = maxHealth;
        _attackState = AttackState.NotAttacking;
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

    protected void FireProjectile()
    {
        // GameObject projectile, Vector3 position, float boxColliderSize, bool firingUp, float attackCooldown, bool isFromPlayer
        // _attackController.ProjectileAttack(projectile, transform.position, _boxCollider.size.y, false, 0f, false);
        var newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        ProjectileController projectileController = newProjectile.GetComponent<ProjectileController>();
        // TODO: Will the player ever fire down, will NPCs ever fire up? Can firingUp be removed and we just use isPlayer instead?
        bool firingUp = _isPlayer;
        projectileController.InstantiateProjectile(transform.position, _boxCollider.size.y, firingUp, _isPlayer);
        if (attackCooldown == 0f) return;
        _attackState = AttackState.Cooldown;
        Invoke(nameof(ResetAttack), attackCooldown);
    }
    
    protected void ResetAttack()
    {
        _attackState = AttackState.NotAttacking;
    }
    
    public bool CanAttack()
    {
        return _attackState == AttackState.NotAttacking;
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
            var projectileController = other.gameObject.GetComponent<ProjectileController>();
            
            // We don't want enemies to be shooting each other
            if (!projectileController.IsFromPlayer() && !_isPlayer)
                return;
            
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
