using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkillObject_Health : MonoBehaviour, ICloneDamageable
{
    [SerializeField] private float currentHealth;
    public float lastDamageTaken { get; private set;}
    private void Die()
    {
        SkillObject_Clone clone = GetComponent<SkillObject_Clone>();
        clone.HandleDeath();
    }


    public void CloneTakeDamage(float amount)
    {
        lastDamageTaken = amount;
        currentHealth -= amount;
        if(currentHealth <= 0)
            Die();
    }
}
