using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MobController
{
    [SerializeField] private float actionCooldown = 1f;
    
    [SerializeField] private GameObject[] loot;

    private int _actionsTaken;
    [SerializeField] private int score = 10;

    private void Awake()
    {
        // Run the Awake logic for the parent class
        SharedAwake();
        _isPlayer = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Run the Start logic for the parent class
        SharedStart();
        
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
        // The Shooting animation, when halfway done, calls the parent FireProjectile method
        _animator.Play("Shooting");
    }

    public override void BeginDeath()
    {
        _locked = true;
        // Delete our collider, this stops the explosion animation from eating arrows and BeginDeath from being called
        // while the enemy is already dying (and spawning multiple loot instances)
        Destroy(_boxCollider);
        
        SpawnLoot();
        _animator.StopPlayback();
        _animator.Play("Explosion");
    }

    protected override void UpdateHealthUI()
    {
        return;
    }

    private void SpawnLoot()
    {
        // TODO: Need logic for non-rupee loot to disappear
        // Might need to refactor RupeeController into a generic LootController and roll rupee
        // logic into it. Need overlapping logic, like flashing when about to de-spawn and then de-spawning
        // TODO: An enemy should not always drop loot, there should be a chance roll here
        // TODO: Hearts spawn a little too often, it's a 50/50 between a heart and one of the 3 rupee types
        int i = Random.Range(0, loot.Length);
        var item = Instantiate(loot[i], gameObject.transform.position, Quaternion.identity);
        item.name = loot[i].name;
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
        SceneController sceneController = GameObject.FindWithTag("SceneController").GetComponent<SceneController>();
        sceneController.KillEnemy(score);
        Destroy(gameObject);
    }
}
