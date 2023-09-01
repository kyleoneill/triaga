using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum DamageType
{
    Physical,
    Magic
}

public class ProjectileController : MonoBehaviour
{
    // Delete me if I am outside of the camera
    // Damage an enemy if there is a collision
    // Needs info on me like "Damage" and maybe more interesting dmg modifiers, like armor pen or a damage type
    //  like physical or magical

    private Rigidbody2D _body;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _sr;
    [SerializeField] private float speed = 15f;
    [SerializeField] private DamageType dmgType;
    [SerializeField] private float damageDealt;
    private bool _isFromPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void InstantiateProjectile(Vector3 parentPosition, float parentColliderSize, bool firingUp, bool isFromPlayer)
    {
        _body = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _isFromPlayer = isFromPlayer;

        // Spawn the projectile either just above or below the GameObject firing it with accompanying velocity
        float colliderDistance = (_boxCollider.size.y / 2f + parentColliderSize / 2f) * 1.05f;
        if (firingUp)
        {
            _body.velocity = new Vector2(0, 1f) * speed;
            _sr.flipY = true;
        }
        else
        {
            colliderDistance *= -1f;
            _body.velocity = new Vector2(0, -1f) * speed;
            _sr.flipY = false;
        }
        Vector3 spawnPosition = new Vector3(parentPosition.x, parentPosition.y + colliderDistance, parentPosition.z);
        transform.position = spawnPosition;
    }

    internal bool IsFromPlayer()
    {
        return _isFromPlayer;
    }
    
    internal float GetDamageDealt()
    {
        return damageDealt;
    }

    internal DamageType GetDamageType()
    {
        return dmgType;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If a player hits a piece of loot with their arrow, de-spawn the loot and give it to the player
        if (other.gameObject.CompareTag("loot") && _isFromPlayer)
        {
            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            switch (other.gameObject.name)
            {
                case "Rupee":
                    RupeeController rupeeController = other.gameObject.GetComponent<RupeeController>();
                    int rupeeValue = rupeeController.GetRupeeValue();
                    rupeeController.Despawn();
                    player.UpdateRupees(rupeeValue);
                    break;
                case "Heart":
                    player.Heal(1);
                    Destroy(other.gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
