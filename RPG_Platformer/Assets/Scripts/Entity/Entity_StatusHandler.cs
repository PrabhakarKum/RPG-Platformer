using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
   private Entity _entity;
   private Entity_VFX _entityVFX;
   private Entity_Stats _entityStats;
   private ElementType _currentEffect = ElementType.None;

   private void Awake()
   {
      _entity = GetComponent<Entity>();
      _entityVFX = GetComponent<Entity_VFX>();
      _entityStats = GetComponent<Entity_Stats>();
   }

   public void ApplyChilledEffect(float duration, float slowMultiplier)
   {
      var iceResistance = _entityStats.GetElementalResistance(ElementType.Ice);
      var finalDuration = duration * (1 - iceResistance);
      
      StartCoroutine(ChilledEffectCoroutine(finalDuration, slowMultiplier));
   }
   private IEnumerator ChilledEffectCoroutine(float duration, float slowMultiplier)
   {
      _entity.SlowDownEntity(duration, slowMultiplier);
      _currentEffect = ElementType.Ice;
      _entityVFX.PlayStatusVFX(duration, _currentEffect);
      
      yield return new WaitForSeconds(duration);
      _currentEffect = ElementType.None;

   }
   
   public void ApplyBurnEffect(float duration, float fireDamage)
   {
      var fireResistance = _entityStats.GetElementalResistance(ElementType.Fire);
      var finalDamage = fireDamage * (1 - fireResistance);
      
      StartCoroutine(BurnedEffectCoroutine(duration, finalDamage));
   }
   private IEnumerator BurnedEffectCoroutine(float duration, float totalDamage)
   {
      _currentEffect = ElementType.Fire;
      _entityVFX.PlayStatusVFX(duration, _currentEffect);

      const int ticksPerSecond = 2;
      var tickCount = Mathf.RoundToInt(ticksPerSecond * duration);

      var damagePerTick = totalDamage / tickCount;
      var tickInterval = 1f / ticksPerSecond;

      for (var i = 0; i < tickCount; i++)
      {
         _entity.ReduceHp(damagePerTick);
         yield return new WaitForSeconds(tickInterval);
      }

      _currentEffect = ElementType.None;

   }

   public bool CanBeApplied(ElementType element)
   {
      return _currentEffect == ElementType.None;
   }
}
