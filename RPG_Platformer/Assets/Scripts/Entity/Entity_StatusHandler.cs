using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class Entity_StatusHandler : MonoBehaviour
{
   private Entity _entity;
   private Entity_VFX _entityVFX;
   private Entity_Stats _entityStats;
   private ElementType _currentEffect = ElementType.None;

   [Header("Lightning effect Details")]
   [SerializeField] private GameObject lightningStrikeVFX;
   [SerializeField] private float currentCharge;
   [SerializeField] private float maximumCharge = 1f;
   private Coroutine _lightningCoroutine;
   
   private void Awake()
   {
      _entity = GetComponent<Entity>();
      _entityVFX = GetComponent<Entity_VFX>();
      _entityStats = GetComponent<Entity_Stats>();
   }

   public void ApplyStatusEffect(ElementType element, ElementalEffectData effectData)
   {
      switch (element)
      {
         case ElementType.Ice when CanBeApplied(ElementType.Ice):
            ApplyChillEffect(effectData.chillDuration, effectData.chillSlowMultiplier);
            break;
         case ElementType.Fire when CanBeApplied(ElementType.Fire):
            ApplyBurnEffect(effectData.burnDuration, effectData.burnDamage);
            break;
         case ElementType.Lightning when CanBeApplied(ElementType.Lightning):
            ApplyLightningEffect(effectData.shockDuration, effectData.shockDamage, effectData.shockCharge);
            break;
      }
   }

   public void ApplyChillEffect(float duration, float slowMultiplier)
   {
      var iceResistance = _entityStats.GetElementalResistance(ElementType.Ice);
      var finalDuration = duration * (1 - iceResistance);
      
      StartCoroutine(ChillEffectCoroutine(finalDuration, slowMultiplier));
   }
   private IEnumerator ChillEffectCoroutine(float duration, float slowMultiplier)
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
   public void ApplyLightningEffect(float duration, float lightningDamage, float charge)
   {
      var lightningResistance = _entityStats.GetElementalResistance(ElementType.Lightning);
      var finalCharge = charge * (1 - lightningResistance);
      currentCharge += finalCharge;
      
      if (currentCharge >= maximumCharge)
      {
         DoLightningStrike(lightningDamage);
         StopLightningEffect();
         return;
      }
      
      if(_lightningCoroutine != null)
         StopCoroutine(_lightningCoroutine);

      _lightningCoroutine = StartCoroutine(LightningEffectCoroutine(duration));
   }

   private IEnumerator LightningEffectCoroutine(float duration)
   {
      _currentEffect = ElementType.Lightning;
      _entityVFX.PlayStatusVFX(duration, _currentEffect);

      yield return new WaitForSeconds(duration);
      StopLightningEffect();
   }

   private void StopLightningEffect()
   {
      _currentEffect = ElementType.None;
      currentCharge = 0;
      _entityVFX.StopAllVFX();
   }
   private void DoLightningStrike(float lightningDamage)
   {
      Instantiate(lightningStrikeVFX, transform.position, Quaternion.identity);
      _entity.ReduceHp(lightningDamage);
   }

   public bool CanBeApplied(ElementType element)
   {
      if (element == ElementType.Lightning && _currentEffect == ElementType.Lightning)
         return true;
      
      
      return _currentEffect == ElementType.None;
   }
}
