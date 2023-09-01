using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MobController
{
    // Player specific component stuff
    [SerializeField] private Sprite deathSprite;

    // TODO: Remove _movementY? I think we are going to be stuck on a rail and move only on x
    // Control player velocity
    private float _movementX, _movementY;
    
    // Player inventory
    private int _rupeeCount;
    
    // Other game controller info
    private HUDController _hudController;

    private void Awake()
    {
        // Run the Awake logic for the parent class
        SharedAwake();
        _isPlayer = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Run the Start logic for the parent class
        SharedStart();
        
        _hudController = GameObject.FindWithTag("hud").GetComponent<HUDController>();
        _rupeeCount = 0;
        _hudController.SetHealthText(_currentHealth);
        _hudController.SetRupeeText(_rupeeCount);
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
        if (Input.GetKey("space") && CanAttack())
            FireProjectile();
        _body.velocity = new Vector2(_movementX, 0) * speed;
    }

    private void AnimatePlayer()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            _animator.Play("Walking");
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            _animator.Play("Standing");
    }

    protected override void BeginDeath()
    {
        // The end of the death animation calls Die
        _animator.Play("Die");
    }

    protected override void UpdateHealthUI()
    {
        _hudController.SetHealthText(_currentHealth);
    }

    internal void UpdateRupees(int amountToAdd)
    {
        _rupeeCount += amountToAdd;
        _hudController.SetRupeeText(_rupeeCount);
    }

    // Die is called at the end of the death animation
    internal void Die()
    {
        _sr.sprite = deathSprite;
    }
}
