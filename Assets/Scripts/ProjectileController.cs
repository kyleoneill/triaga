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

    public void InstantiateProjectile(Vector3 parentPosition, float parentColliderSize, bool firingUp)
    {
        _body = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();

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

    internal float GetDamageDealt()
    {
        return damageDealt;
    }

    internal DamageType GetDamageType()
    {
        return dmgType;
    }
}
