using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _body;
    private Animator _animator;
    private SpriteRenderer _sr;
    private BoxCollider2D _boxCollider;
    private bool _locked;
    [SerializeField] private Sprite deathSprite;

    [SerializeField] private float speed = 8f;
    [SerializeField] private float attackCooldown = 0.75f;

    // TODO: Remove _movementY? I think we are going to be stuck on a rail and move only on x
    private float _movementX, _movementY;
    private AttackController _attackController;

    private int _currentHealth;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int armor = 0;

    [SerializeField] private GameObject projectile;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        if (_locked) return;
        PlayerMoveKeyboard();
        AnimatePlayer();   
    }

    private void PlayerMoveKeyboard()
    {
        _movementX = Input.GetAxisRaw("Horizontal");
        // _movementY = Input.GetAxisRaw("Vertical");
        if (Input.GetKey("space") && _attackController.CanAttack())
            _attackController.ProjectileAttack(projectile, transform.position, _boxCollider.size.y, true, attackCooldown);
        _body.velocity = new Vector2(_movementX, 0) * speed;
    }

    private void AnimatePlayer()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            _animator.Play("Walking");
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            _animator.Play("Standing");
    }

    internal void TakeDamage(float damageToTake)
    {
        if (damageToTake <= 0 || _currentHealth <= 0) return;
        // TODO: Reconcile float damage vs int health
        _currentHealth -= (int)damageToTake;
        Debug.Log(_currentHealth);
        if (_currentHealth <= 0)
        {
            // We don't want the player moving or trying to do anything while their death animation is playing
            _locked = true;
            _body.velocity = new Vector2(0, 0);
            
            // The end of the death animation calls Die
            _animator.Play("Die");
        }
    }

    // Die is called at the end of the death animation
    internal void Die()
    {
        _sr.sprite = deathSprite;
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
}
