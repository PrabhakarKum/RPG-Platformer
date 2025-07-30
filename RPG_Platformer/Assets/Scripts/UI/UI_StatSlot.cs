using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   private RectTransform rectTransform;
   private UI_Manager uiManager;
   private Player_Stats playerStats;
   
   [SerializeField] private StatType statSlotType;
   [SerializeField] private TextMeshProUGUI statName;
   [SerializeField] private TextMeshProUGUI statValue;

   private void OnValidate()
   {
      gameObject.name = "UI Stat - " + GetStatNameByType(statSlotType);
      statName.text = GetStatNameByType(statSlotType);
   }

   private void Awake()
   {
      uiManager = GetComponentInParent<UI_Manager>();
      rectTransform = GetComponent<RectTransform>();
      playerStats = FindFirstObjectByType<Player_Stats>();
   }
   
   public void OnPointerEnter(PointerEventData eventData)
   {
      uiManager.statTooltip.ShowToolTip(true, rectTransform, statSlotType);
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      uiManager.statTooltip.ShowToolTip(false, null);
   }
   public void UpdateStatValue()
   {
      Stat statToUpdate = playerStats.GetStatByType(statSlotType);

      if (statToUpdate == null && statSlotType != StatType.ElementalDamage)
      {
         Debug.Log("Stat not found for type: " + statSlotType);
         return;
      }
      
      float value = 0;
      
      switch (statSlotType)
      {
         // Major Stats
         case StatType.Strength:
            value = playerStats.major.strength.GetValue();
            break;
         case StatType.Agility:
            value = playerStats.major.agility.GetValue();
            break;
         case StatType.Intelligence:
            value = playerStats.major.intelligence.GetValue();
            break;
         case StatType.Vitality:
            value = playerStats.major.vitality.GetValue();
            break;
         
         // Offence Stats
         case StatType.Damage:
            value = playerStats.GetBaseDamage();
            break;
         case StatType.CriticalPower:
            value = playerStats.GetCriticalPower();
            break;
         case StatType.CriticalChance:
            value = playerStats.GetCriticalChance();
            break;
         case StatType.ArmourReduction:
            value = playerStats.GetArmourReduction() * 100; // Convert to percentage
            break;
         case StatType.AttackSpeed:
            value = playerStats.offence.attackSpeed.GetValue() * 100; // Convert to percentage
            break;
         
         // Defence Stats
         case StatType.MaxHealth:
            value = playerStats.GetMaxHealth();
            break;
         case StatType.HealthRegeneration:
            value = playerStats.resources.healthRegeneration.GetValue();
            break;
         case StatType.Armour:
            value = playerStats.GetBaseArmour();
            break;
         case StatType.Evasion:
            value = playerStats.GetEvasion();
            break;
         
         // Elemental Damages Stats
         case StatType.FireDamage:
            value = playerStats.offence.fireDamage.GetValue();
            break;
         case StatType.IceDamage:
            value = playerStats.offence.iceDamage.GetValue();
            break;
         case StatType.LightningDamage:
            value = playerStats.offence.lightningDamage.GetValue();
            break;
         case StatType.ElementalDamage:
            value = playerStats.GetElementalDamage(out ElementType elementType, 1f);
            break;
         
         // Elemental Resistances Stats
         case StatType.FireResistance:
            value = playerStats.GetElementalResistance(ElementType.Fire) * 100; // Convert to percentage
            break;
         case StatType.IceResistance:
            value = playerStats.GetElementalResistance(ElementType.Ice) * 100; // Convert to percentage
            break;
         case StatType.LightningResistance:
            value = playerStats.GetElementalResistance(ElementType.Lightning) * 100; // Convert to percentage
            break;
         
         default: 
            Debug.LogWarning("Stat type not handled: " + statSlotType);
            return;
      }
      statValue.text = IsPercentageStat(statSlotType) ? value + "%" : value.ToString();
   }

   private string GetStatNameByType(StatType statType)
   {
      switch (statType)
      {
         case StatType.MaxHealth: return "Max Health";
         case StatType.HealthRegeneration: return "HealthRegeneration";
         
         case StatType.Strength: return "Strength";
         case StatType.Agility: return "Agility";
         case StatType.Intelligence: return "Intelligence";
         case StatType.Vitality: return "Vitality";
         
         case StatType.AttackSpeed: return "Attack Speed";
         case StatType.Damage: return "Damage";
         case StatType.CriticalPower: return "Critical Power";
         case StatType.CriticalChance: return "Critical Chance";
         case StatType.ArmourReduction: return "Armour Reduction";
         
         case StatType.FireDamage: return "Fire Damage";
         case StatType.IceDamage: return "Ice Damage";
         case StatType.LightningDamage: return "Lightning Damage";
         case StatType.ElementalDamage: return "Elemental Damage";
         
         case StatType.Armour: return "Armour";
         case StatType.Evasion: return "Evasion";
         case StatType.FireResistance: return "Fire Resistant";
         case StatType.IceResistance: return "Ice Resistant";
         case StatType.LightningResistance: return "Lightning Resistant";
         default:
            return "Unknown Stat";
      }
   }
   
   private bool IsPercentageStat(StatType statType)
   {
      switch (statType)
      {
         case StatType.CriticalChance:
         case StatType.CriticalPower:
         case StatType.ArmourReduction:
         case StatType.FireResistance:
         case StatType.IceResistance:
         case StatType.LightningResistance:
         case StatType.AttackSpeed:
         case StatType.Evasion:
            return true;
         default:
            return false;
      }
   }
}
