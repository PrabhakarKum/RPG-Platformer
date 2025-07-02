using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Entity _entity;
    
    [Header("On Taking Damage VFX")]
    [SerializeField] private Material hitMaterial;
    [SerializeField] private float flashDuration;
    private Material _originalMaterial;

    [Header("On Doing Damage VFX")] 
    [SerializeField] private Color hitVFXColor = Color.white;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject criticalHitVFX;
    
    
    [Header("Elemental Colors")] 
    [SerializeField] private Color chillVFX = Color.cyan;
    [SerializeField] private Color burnVFX = Color.red;
    private Color _originalColorHitVFX;
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _entity = GetComponent<Entity>();
        _originalMaterial = _spriteRenderer.material;
        _originalColorHitVFX = hitVFXColor;
    }

    public IEnumerator FlashFX()
    {
        _spriteRenderer.material = hitMaterial;
        yield return new WaitForSeconds(flashDuration);
        _spriteRenderer.material = _originalMaterial;
    }

    public void PlayStatusVFX(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
            StartCoroutine(PlayStatusVFXCoroutine(duration, chillVFX));
        
        if (element == ElementType.Fire)
            StartCoroutine(PlayStatusVFXCoroutine(duration, burnVFX));
    }
    private IEnumerator PlayStatusVFXCoroutine(float duration, Color effectColor)
    {
        const float tickInterval = 0.2f;
        var timeHasPassed = 0f;

        var lightColor = effectColor * 0.2f;
        var darkColor = effectColor * 1.5f;

        var toggle = false;
        
        while(timeHasPassed < duration)
        {
            _spriteRenderer.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timeHasPassed += tickInterval;
        }

        _spriteRenderer.color = Color.white;
    }

    private void RedColorBlink()
    {
        _spriteRenderer.color = _spriteRenderer.color != Color.white ? Color.white : Color.red;
    }
    
    private void CancelRedBlink()
    {
        CancelInvoke();
        _spriteRenderer.color = Color.white;
    }

    public void CreateOnHitVFX(Transform target, bool isCritical)
    {
        var hitPrefab = isCritical ? criticalHitVFX : hitVFX;
        var vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
        
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitVFXColor;
        
        if (_entity.facingDirection == 1 && isCritical)
        {
            vfx.transform.Rotate(0,180,0);
        }
    }

    public void UpdateOnHitColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.Fire:
                hitVFXColor = burnVFX;
                break;
            case ElementType.Ice:
                hitVFXColor = chillVFX;
                break;
            case ElementType.Lightning:
                hitVFXColor = Color.yellow;
                break;
            case ElementType.None:
                hitVFXColor = _originalColorHitVFX;
                break;
        }
    }
}
