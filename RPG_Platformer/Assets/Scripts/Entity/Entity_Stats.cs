using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Entity_Stats : MonoBehaviour
{ 
   public Stat maxHealth;

   public Stat_MajorGroup major;
   public Stat_DefenceGroup defence;
   public Stat_OffenceGroup offence;

   public float GetPhysicalDamage()
   {
      var baseDamage = offence.damage.GetValue();
      var bonusDamage = major.strength.GetValue();
      var totalBaseDamage = baseDamage + bonusDamage;

      var baseCriticalChance = offence.criticalChance.GetValue();
      var bonusCriticalChance = major.agility.GetValue() * 0.3f; // Bonus Critical Chance from agility: +0.3% per AGI
      var criticalChance = baseCriticalChance + bonusCriticalChance;

      float baseCriticalPower = offence.criticalPower.GetValue();
      float bonusCriticalPower = major.strength.GetValue() * 0.5f; // Bonus Critical Power from agility: +0.5% per CritPower
      var criticalPower = (baseCriticalPower + bonusCriticalPower)/ 100; // Total Critical Power as multiplier (e.g 110 / 100 = 1.1f - multiplier)

      bool isCritical = Random.Range(0, 100) < criticalChance;
      float finalDamage= isCritical ? totalBaseDamage * criticalPower : totalBaseDamage;

      return finalDamage;
   }
   public float GetMaxHealth()
   {
      var baseHp = maxHealth.GetValue();
      var bonusHp =  major.vitality.GetValue() * 5;
      return baseHp + bonusHp;
   }

   public float GetEvasion()
   {
      var baseEvasion = defence.evasion.GetValue();
      var bonusEvasion = major.agility.GetValue() * 0.5f;

      var totalEvasion = baseEvasion + bonusEvasion;
      var evasionCap = 85f; // Evasion will be capped at 85%

      var finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);
      
      return finalEvasion;
   }
   
}
