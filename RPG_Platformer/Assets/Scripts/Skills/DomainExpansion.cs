using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainPrefab;

    
    [Header("Slowing Down Upgrade")]
    [SerializeField]private float slowDownDomainPercent = 0.8f;
    [SerializeField] private float slowDownDomainDuration = 5f;
    
    [Header("Crystal Casting Upgrade")] 
    [SerializeField] private int crystalToCast = 10;
    [SerializeField] private float slowdownCrystalCastingDomain = 5f;
    [SerializeField] private float crystalCastingDomainDuration = 5f;
    private float spellCastTimer;
    private float spellPerSecond;
    
    [Header("Clone Casting Upgrade")] 
    [SerializeField] private int cloneToCast = 10;
    [SerializeField] private float slowdownCloneCastingDomain = 5f;
    [SerializeField] private float cloneCastingDomainDuration = 5f;
    [SerializeField] private float healthToRestoreWithClone = 0.05f;
    
    [Header("Domain Details")]
    public float maxDomainSize = 5f;
    public float expandSpeed = 2f;

    private List<Enemy> trappedTargets = new List<Enemy>();
    private Transform currentTargets;
    
    public void CreateDomain()
    {
        spellPerSecond = GetSpellsToCast() / GetDomainDuration();
        GameObject domain = Instantiate(domainPrefab, player.transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }

    private void CastSpell(Transform target)
    {
        if (upgradeType == SkillUpgradeType.Domain_CloneSpam)
        {
            Vector3 offset = Random.value < 0.5f ? new Vector2(0.2f, 0) : new Vector2(-0.2f, 0);
            skillManager.cloneSkill.CreateClone(target, offset);
        }

        if (upgradeType == SkillUpgradeType.Domain_CrystalSpam)
        {
            skillManager.crystalSkill.CreateRawCrystal(target, true);
        }
        
    }

    public void DoSpellCasting()
    {
        spellCastTimer -= Time.deltaTime;

        if (currentTargets == null)
            currentTargets = FindTargetInDomain();

        if (currentTargets != null && spellCastTimer < 0)
        {
            CastSpell(currentTargets);
            spellCastTimer = 1 / spellPerSecond;
            currentTargets = null;
        }
    }
    
    private Transform FindTargetInDomain()
    {
        trappedTargets.RemoveAll(target => target == null || target.isDead);

        if (trappedTargets.Count == 0)
            return null;

        var randomIndex = Random.Range(0, trappedTargets.Count);
        return trappedTargets[randomIndex].transform;
    }
    
    public bool InstantDomain()
    {
        return upgradeType != SkillUpgradeType.Domain_CloneSpam
               && upgradeType != SkillUpgradeType.Domain_CrystalSpam;
    }
    public float GetDomainDuration()
    {
        switch (upgradeType)
        {
            case SkillUpgradeType.Domain_SlowingDown:
                return slowDownDomainDuration;
            case SkillUpgradeType.Domain_CrystalSpam:
                return crystalCastingDomainDuration;
            case SkillUpgradeType.Domain_CloneSpam:
                return cloneCastingDomainDuration;
        }

        return 0;
    }
    
    public float GetSlowPercentage()
    {
        switch (upgradeType)
        {
            case SkillUpgradeType.Domain_SlowingDown:
                return slowDownDomainPercent;
            case SkillUpgradeType.Domain_CrystalSpam:
                return slowdownCrystalCastingDomain;
            case SkillUpgradeType.Domain_CloneSpam:
                return slowdownCloneCastingDomain;
        }

        return 0;
    }

    private int GetSpellsToCast()
    {
        switch (upgradeType)
        {
            case SkillUpgradeType.Domain_CrystalSpam:
                return crystalToCast;
            case SkillUpgradeType.Domain_CloneSpam:
                return cloneToCast;
        }

        return 0;
    }
    
    public void AddTarget(Enemy targetToAdd)
    {
        trappedTargets.Add(targetToAdd);
    }

    public void ClearTargets()
    {
        foreach (var enemy in trappedTargets)
            enemy.StopSlowDown();
        
        trappedTargets = new List<Enemy>();
    }

}
