using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Entity_Stats
{
   private List<string> activeBuffs = new List<string>();
   private Inventory_Player inventory_Player;

   protected override void Awake()
   {
      base.Awake();
      inventory_Player = GetComponent<Inventory_Player>();
   }

   public bool CanApplyBuffOf(string source)
   {
      return activeBuffs.Contains(source) == false;
   }

   public void ApplyBuffOf(BuffEffectData[] buffToApply, float duration, string source)
   {
      StartCoroutine(BuffCoroutine(buffToApply, duration, source));
   }
   
   private IEnumerator BuffCoroutine(BuffEffectData[] buffToApply, float duration, string source)
   {
      // if (CanApplyBuffOf(source) == false)
      //    yield break;

      activeBuffs.Add(source);
      foreach (var buff in buffToApply)
      {
         GetStatByType(buff.type).AddModifier(buff.value, source);
      }

      yield return new WaitForSeconds(duration);

      foreach (var buff in buffToApply)
      {
         GetStatByType(buff.type).RemoveModifier(source);
      }
      
      inventory_Player.TriggerUpdateUI();
      activeBuffs.Remove(source);
   }
}
