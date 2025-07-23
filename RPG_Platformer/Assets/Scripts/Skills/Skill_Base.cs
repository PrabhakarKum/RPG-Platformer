using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public DamageScaleData damageScaleData { get; private set; }
    
    [Header("General Details")] 
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    private float _lastTimeUsed;
    public Player player { get; private set; }
    public SkillManager skillManager { get; private set; }
    

    protected virtual void Awake()
    {
        skillManager = GetComponent<SkillManager>();
        _lastTimeUsed -= cooldown;
        damageScaleData = new DamageScaleData();
    }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        damageScaleData = upgrade.damageScaleData;
        ResetCooldown();
    }
    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        _lastTimeUsed -= Time.deltaTime;
    }
    
    public virtual bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None)
            return false;
        
        if (OnCoolDown())
        {
            Debug.Log("On Cooldown");
            return false;
        }

        UseSkill();
        SetSkillOnCooldown();
        return true;
    }

    protected virtual void UseSkill()
    {
        //use skill
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;
    protected bool OnCoolDown() => Time.time < _lastTimeUsed + cooldown;
    protected void SetSkillOnCooldown() => _lastTimeUsed = Time.time;
    public void ReduceCooldownBy(float coolDownReduction) => _lastTimeUsed += coolDownReduction;
    private void ResetCooldown() => _lastTimeUsed = Time.time - cooldown;
}
