using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Entity_Stats : MonoBehaviour
{
   public Stat_SetupSO defaultStatSetup;
   
   public Stat_ResourceGroup resources;
   public Stat_DefenceGroup defence;
   public Stat_OffenceGroup offence;
   public Stat_MajorGroup major;

   protected virtual void Awake()
   {
      
   }

   public AttackData GetAttackData(DamageScaleData scaleData)
   {
      return new AttackData(this, scaleData);
   }
   
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
      var baseDamage = GetBaseDamage();
      var criticalChance = GetCriticalChance();
      var criticalPower = GetCriticalPower()/ 100; // Total Critical Power as multiplier (e.g 110 / 100 = 1.1f - multiplier)

      isCritical = Random.Range(0, 100) < criticalChance;
      var finalDamage= isCritical ? baseDamage * criticalPower : baseDamage;

      return finalDamage * scaleFactor;
   }

   public float GetBaseDamage() => offence.damage.GetValue() + major.strength.GetValue(); // Bonus Damage from strength: +1 per STR
   public float GetCriticalChance() => offence.criticalChance.GetValue() + (major.agility.GetValue() * 0.3f); // Bonus Critical Chance from agility: +0.3% per AGI
   public float GetCriticalPower() => offence.criticalPower.GetValue() + (major.strength.GetValue() * 0.5f); // Bonus Critical Power from strength: +0.5% per STR


   public float GetArmourMitigation(float armourReduction)
   {
      var totalArmour = GetBaseArmour();

      var reductionMultiplier = Mathf.Clamp(1 - armourReduction, 0, 1);
      var effectiveArmour = totalArmour * reductionMultiplier;
      
      var mitigation = effectiveArmour / (effectiveArmour + 100);
      const float mitigationCap = 0.85f; //Max mitigation will be capped at 85%;

      var finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);
      return finalMitigation;
   }

   public float GetBaseArmour() => defence.armour.GetValue() + major.vitality.GetValue();  // Bonus armour from Vitality : +1 per VIT
   public float GetArmourReduction()
   {
      // Total armour reduction as multiplier (e.g 30 / 100 = 0.3f - multiplier)
      var finalReduction = offence.armourReduction.GetValue() / 100;
      return finalReduction;
   }
   public float GetMaxHealth()
   {
      var baseMaxHp = resources.maxHealth.GetValue();
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

   public Stat GetStatByType(StatType type)
   {
      switch (type)
      {
         case StatType.MaxHealth:
            return resources.maxHealth;
         case StatType.HealthRegeneration:
            return resources.healthRegeneration;
         
         case StatType.Strength:
            return major.strength;
         case StatType.Agility:
            return major.agility;
         case StatType.Intelligence:
            return major.intelligence;
         case StatType.Vitality:
            return major.vitality;
         
         case StatType.AttackSpeed:
            return offence.attackSpeed;
         case StatType.Damage:
            return offence.damage;
         case StatType.CriticalPower:
            return offence.criticalPower;
         case StatType.CriticalChance:
            return offence.criticalChance;
         case StatType.ArmourReduction:
            return offence.armourReduction;
         
         case StatType.FireDamage:
            return offence.fireDamage;
         case StatType.IceDamage:
            return offence.iceDamage;
         case StatType.LightningDamage:
            return offence.lightningDamage;
         
         
         case StatType.Armour:
            return defence.armour;
         case StatType.Evasion:
            return defence.evasion;
         case StatType.FireResistance:
            return defence.fireResistant;
         case StatType.IceResistance:
            return defence.iceResistant;
         case StatType.LightningResistance:
            return defence.lightningResistant;

         default:
            Debug.LogWarning($"Stat Type: {type} not implemented yet");
            return null;
      }
   }

   [ContextMenu(("Update Default Setup"))]
   public void ApplyDefaultStatSetup()
   {
      if (defaultStatSetup == null)
      {
         Debug.LogWarning("No default setup assigned");
         return;
      }
      
      resources.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
      resources.healthRegeneration.SetBaseValue(defaultStatSetup.healthRegeneration);
      
      major.strength.SetBaseValue(defaultStatSetup.strength);
      major.agility.SetBaseValue(defaultStatSetup.agility);
      major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
      major.vitality.SetBaseValue(defaultStatSetup.vitality);
      
      offence.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
      offence.damage.SetBaseValue(defaultStatSetup.damage);
      offence.criticalPower.SetBaseValue(defaultStatSetup.criticalPower);
      offence.criticalChance.SetBaseValue(defaultStatSetup.criticalChance);
      offence.armourReduction.SetBaseValue(defaultStatSetup.armourReduction);
      
      offence.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
      offence.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
      offence.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);
      
      defence.armour.SetBaseValue(defaultStatSetup.armour);
      defence.evasion.SetBaseValue(defaultStatSetup.evasion);
      
      defence.fireResistant.SetBaseValue(defaultStatSetup.fireResistant);
      defence.iceResistant.SetBaseValue(defaultStatSetup.iceResistant);
      defence.lightningResistant.SetBaseValue(defaultStatSetup.lightningResistant);
      
   }
   
}
