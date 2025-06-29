using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    [Header("On Taking Damage VFX")]
    [SerializeField] private Material hitMaterial;
    [SerializeField] private float flashDuration;
    private Material originalMaterial;

    [Header("On Doing Damage VFX")] [SerializeField]
    private Color hitVFXColor = Color.white;
    [SerializeField] private GameObject hitVFX;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMaterial;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = originalMaterial;
    }

    private void RedColorBlink()
    {
        if (spriteRenderer.color != Color.white)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
            
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }

    public void CreateOnHitVFX(Transform target)
    {
        GameObject vfx = Instantiate(hitVFX, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitVFXColor;
    }
}
