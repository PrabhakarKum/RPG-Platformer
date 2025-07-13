using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage,float elementalDamage, ElementType element, Transform position, Transform damageDealer, bool isCritical = false);
}
