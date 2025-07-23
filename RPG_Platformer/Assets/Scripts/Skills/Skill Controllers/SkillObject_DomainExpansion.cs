using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_DomainExpansion : SkillObject_Base
{
    private DomainExpansion domainExpansion;
    private float duration;
    private float slowDownPercent;
    private float expandSpeed;
    private Vector3 targetScale;
    private bool isShrinking;

    private void Update()
    {
        HandleScaling();
    }


    public void SetupDomain(DomainExpansion domainManager)
    { 
        domainExpansion = domainManager;
        
        duration = domainManager.GetDomainDuration();
        var maxSize = domainManager.maxDomainSize;
        expandSpeed = domainManager.expandSpeed;
        slowDownPercent = domainManager.GetSlowPercentage();
        
        targetScale = Vector2.one * maxSize;
        Invoke(nameof(ShrinkDomain), duration);
        
    }

    private void HandleScaling()
    {
        var sizeDifference = Mathf.Abs(transform.localScale.x - targetScale.x);
        var shouldChangeScale = sizeDifference > 0.1f;

        if (shouldChangeScale)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expandSpeed * Time.deltaTime);

        if (isShrinking && sizeDifference < 0.1f)
            TerminateDomain();
        
    }

    private void TerminateDomain()
    {
        domainExpansion.ClearTargets();
        Destroy(gameObject);
    }

    private void ShrinkDomain()
    {
        targetScale = Vector3.zero;
        isShrinking = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null)
            return;
        
        domainExpansion.AddTarget(enemy);
        enemy.SlowDownEntity(duration, slowDownPercent, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null)
            return;
        
        enemy.StopSlowDown();
    }

    
}
