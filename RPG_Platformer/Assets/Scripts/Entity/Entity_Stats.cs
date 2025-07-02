using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Entity_Stats : MonoBehaviour
{ 
   public Stat maxHealth;

   public Stat_MajorGroup major;
   public Stat_DefenceGroup defence;
   public Stat_OffenceGroup offence;

   public float GetElementalDamage(out ElementType element, float scaleFactor = 1f)
   {
      var fireDamage = offence.fireDamage.GetValue();
      var iceDamage = offence.iceDamage.GetValue();
      var lightningDamage = offence.lightningDamage.GetValue();
      var bonusElementalDamage = major.intelligence.GetValue(); //Bonus elemental Damage from Intelligence +1 per INT

      var highestDamage = fireDamage;
      element = ElementType.Fire;

      if (iceDamage > highestDamage)
      {
         highestDamage = iceDamage;
         element = ElementType.Ice;
      }

      if (lightningDamage > highestDamage)
      {
         highestDamage = lightningDamage;
         element = ElementType.Lightning;
      }

      if (highestDamage <= 0)
      {
         element = ElementType.None;
         return 0f;
      }

      var bonusFireDamage = (Mathf.Approximately(fireDamage, highestDamage)) ? 0 : fireDamage * 0.5f;
      var bonusIceDamage = (Mathf.Approximately(iceDamage, highestDamage)) ? 0 : iceDamage * 0.5f;
      var bonusLightningDamage = (Mathf.Approximately(lightningDamage, highestDamage)) ? 0 : lightningDamage * 0.5f;

      var weakerElementalDamage = bonusFireDamage + bonusIceDamage + bonusLightningDamage;
      
      var finalDamage = highestDamage + weakerElementalDamage + bonusElementalDamage;
      return finalDamage * scaleFactor;
   }


   public float GetElementalResistance(ElementType element)
   {
      var baseResistance = 0f;
      var bonusResistance = major.intelligence.GetValue() * 0.5f; //Bonus resistance from intelligence: 0.5% per INT

      switch (element)
      {
         case ElementType.Fire:
            baseResistance = defence.fireResistant.GetValue();
            break;
         case ElementType.Ice:
            baseResistance = defence.iceResistant.GetValue();
            break;
         case ElementType.Lightning:
            baseResistance = defence.lightningResistant.GetValue();
            break;
         case ElementType.None:
            baseResistance = 0f;
            break;
      }

      var resistance = baseResistance + bonusResistance;
      const float resistanceCap = 75f;

      var finalResistance = Mathf.Clamp(resistance, 0, resistanceCap)/ 100; // convert value into 0 to 1 multiplier
      return finalResistance;
   }

   public float GetPhysicalDamage(out bool isCritical, float scaleFactor = 1f)
   {
      var baseDamage = offence.damage.GetValue();
      var bonusDamage = major.strength.GetValue();
      var totalBaseDamage = baseDamage + bonusDamage;

      var baseCriticalChance = offence.criticalChance.GetValue();
      var bonusCriticalChance = major.agility.GetValue() * 0.3f; // Bonus Critical Chance from agility: +0.3% per AGI
      var criticalChance = baseCriticalChance + bonusCriticalChance;

      var baseCriticalPower = offence.criticalPower.GetValue();
      var bonusCriticalPower = major.strength.GetValue() * 0.5f; // Bonus Critical Power from agility: +0.5% per CritPower
      var criticalPower = (baseCriticalPower + bonusCriticalPower)/ 100; // Total Critical Power as multiplier (e.g 110 / 100 = 1.1f - multiplier)

      isCritical = Random.Range(0, 100) < criticalChance;
      var finalDamage= isCritical ? totalBaseDamage * criticalPower : totalBaseDamage;

      return finalDamage * scaleFactor;
   }

   public float GetArmourMitigation(float armourReduction)
   {
      var baseArmour = defence.armour.GetValue();
      var bonusArmour = major.vitality.GetValue(); // Bonus armour from Vitality : +1 per VIT
      var totalArmour = baseArmour + bonusArmour;

      var reductionMultiplier = Mathf.Clamp(1 - armourReduction, 0, 1);
      var effectiveArmour = totalArmour * reductionMultiplier;
      
      var mitigation = effectiveArmour / (effectiveArmour + 100);
      const float mitigationCap = 0.85f; //Max mitigation will be capped at 85%;

      var finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);
      return finalMitigation;
   }

   public float GetArmourReduction()
   {
      // Total armour reduction as multiplier (e.g 30 / 100 = 0.3f - multiplier)
      var finalReduction = offence.armourReduction.GetValue() / 100;
      return finalReduction;
   }
   public float GetMaxHealth()
   {
      var baseMaxHp = maxHealth.GetValue();
      var bonusMaxHp =  major.vitality.GetValue() * 5;
      var finalMaxHealth = baseMaxHp + bonusMaxHp;
      return finalMaxHealth;
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
