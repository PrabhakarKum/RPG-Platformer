using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1f;

    protected Entity_Stats playerStats;
    protected DamageScaleData damageScaleData;
    protected ElementType usedElement;

    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }
    protected void OnDrawGizmos()
    {
        if (targetCheck == null)
            targetCheck = transform;
        
        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        var closestDistance = Mathf.Infinity;

        foreach (var enemy in EnemiesAround(transform, 1.5f))
        {
            var distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distanceToEnemy;
            }
        }

        return target;
    }
    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var target in EnemiesAround(t, radius))
        {
            var damageable = target.GetComponent<IDamageable>();
            var statusHandler = target.GetComponent<Entity_StatusHandler>();
            
            if(damageable == null)
                continue;

            var attackData = playerStats.GetAttackData(damageScaleData);
            
            var physicalDamage = attackData.physicalDamage;
            var elementalDamage = attackData.elementalDamage;
            var element = attackData.element;
            
            damageable.TakeDamage(physicalDamage,elementalDamage, element, target.transform, transform, attackData.isCritical);
            
            if (element != ElementType.None)
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);
            
            usedElement = element;
        }
    }
}
