using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Entity _entity;
    
    [Header("On Taking Damage VFX")]
    [SerializeField] private Material hitMaterial;
    [SerializeField] private float flashDuration;
    private Material _originalMaterial;
    
    [Header("On Doing Damage VFX")] 
    [SerializeField] private Color hitVFXColor;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject criticalHitVFX;
    
    [Header("Elemental Colors")] 
    [SerializeField] private Color chillVFX = Color.cyan;
    [SerializeField] private Color burnVFX = Color.red;
    [SerializeField] private Color lightningVFX = Color.yellow;
    private Color _originalColorHitVFX;
    
    [Header("Elemental UI Image")]
    [SerializeField] private SpriteRenderer burnIcon;
    [SerializeField] private SpriteRenderer chillIcon;
    [SerializeField] private SpriteRenderer lightningIcon;
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _entity = GetComponent<Entity>();
        _originalMaterial = _spriteRenderer.material;
        _originalColorHitVFX = hitVFXColor;
    }
    
    void Start()
    {
        if(burnIcon ==null|| chillIcon == null || lightningIcon == null)
            return;
        
        burnIcon.gameObject.SetActive(false);
        chillIcon.gameObject.SetActive(false);
        lightningIcon.gameObject.SetActive(false);
    }

    public IEnumerator FlashFX()
    {
        _spriteRenderer.material = hitMaterial;
        yield return new WaitForSeconds(flashDuration);
        _spriteRenderer.material = _originalMaterial;
    }

    public void PlayStatusVFX(float duration, ElementType element)
    {
        switch (element)
        {
            case ElementType.Ice:
                StartCoroutine(PlayStatusVFXCoroutine(duration, chillVFX, chillIcon));
                break;
            case ElementType.Fire:
                StartCoroutine(PlayStatusVFXCoroutine(duration, burnVFX, burnIcon));
                break;
            case ElementType.Lightning:
                StartCoroutine(PlayStatusVFXCoroutine(duration, lightningVFX, lightningIcon));
                break;
        }
    }
    
    private IEnumerator PlayStatusVFXCoroutine(float duration, Color effectColor, SpriteRenderer sprite)
    {

        if (sprite == null)
            yield break;
        
        const float tickInterval = 0.2f;
        var timeHasPassed = 0f;

        var lightColor = effectColor * 0.2f;
        var darkColor = effectColor * 1.5f;
        var toggle = false;

        
        sprite.gameObject.SetActive(true);
        while(timeHasPassed < duration)
        {
            _spriteRenderer.color = toggle ? lightColor : darkColor;
            toggle = !toggle;
            yield return new WaitForSeconds(tickInterval);
            timeHasPassed += tickInterval;
        }

        _spriteRenderer.color = Color.white;
        sprite.gameObject.SetActive(false);
    }

    public void StopAllVFX()
    {
        StopAllCoroutines();
        _spriteRenderer.color = Color.white;
        _spriteRenderer.material = _originalMaterial;
        burnIcon.gameObject.SetActive(false);
        chillIcon.gameObject.SetActive(false);
        lightningIcon.gameObject.SetActive(false);
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
                hitVFXColor = lightningVFX;
                break;
            case ElementType.None:
                hitVFXColor = _originalColorHitVFX;
                break;
        }
    }
}
