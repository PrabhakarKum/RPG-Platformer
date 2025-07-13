using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Rpg Setup/ Default Stat Setup", fileName = "Default Stat Setup")]
public class Stat_SetupSO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth;
    public float healthRegeneration;
    
    [Header("Offense - Physical Damage")]
    public float damage;
    public float attackSpeed;
    public float criticalPower;
    public float criticalChance;
    public float armourReduction;
    
    [Header("Offense - Elemental Damage")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;
    
    [Header("Defence - Physical Damage")]
    public float armour;
    public float evasion;
    
    [Header("Defence - Elemental Damage")]
    public float fireResistant;
    public float iceResistant;
    public float lightningResistant;

    [Header("Major Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;
    
}
