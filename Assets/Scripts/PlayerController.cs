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

    [SerializeField] private float speed = 8f;
    [SerializeField] private float attackCooldown = 0.75f;

    // TODO: Remove _movementY? I think we are going to be stuck on a rail and move only on x
    private float _movementX, _movementY;
    private AttackController _attackController;

    private int _currentHealth;
    [SerializeField] private int maxHealth = 3;

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
    }

    // Update is called once per frame
    void Update()
    {
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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Player was hit with a projectile");
            Destroy(col.collider.gameObject);
        }
    }
}
