using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum AttackState
{
    NotAttacking,
    Attack,
    Cooldown
}

public class AttackController : MonoBehaviour
{
    private AttackState _attackState;

    public bool CanAttack()
    {
        return _attackState == AttackState.NotAttacking;
    }
    
    public void ProjectileAttack(GameObject projectile, Vector3 position, float boxColliderSize, bool firingUp, float attackCooldown)
    {
        var newProjectile = Instantiate(projectile, position, Quaternion.identity);
        ProjectileController projectileController = newProjectile.GetComponent<ProjectileController>();
        projectileController.InstantiateProjectile(position, boxColliderSize, firingUp);
        if (attackCooldown == 0f) return;
        _attackState = AttackState.Cooldown;
        Invoke(nameof(ResetAttack), attackCooldown);
    }
    
    private void ResetAttack()
    {
        _attackState = AttackState.NotAttacking;
    }
}
