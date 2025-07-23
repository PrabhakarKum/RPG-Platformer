using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CrystalSkill : Skill_Base
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float detonateTime = 2f;
    private SkillObject_Crystal _currentCrystal;

    [Header("Moving Shard upgrade")] 
    [SerializeField] private float crystalMoveSpeed = 5f;

    [Header("Multicast Crystal Upgrade")] 
    [SerializeField] private int maxCharges;

    [SerializeField] private int currentCharges;
    [SerializeField] private bool isRecharging;

    [Header("Teleport Crystal Upgrade")]
    [SerializeField] private float crystalExistDuration = 10f;

    [Header("Health Rewind Crystal Upgrade")] 
    [SerializeField] private float savedHealthPercent;

    protected override void Start()
    {
        base.Start();
        currentCharges = maxCharges;
    }
    
    protected override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.B)) return;
        if (Unlocked(SkillUpgradeType.Crystal_Teleport) && !OnCoolDown() )
        {
            HandleCrystalTeleport();
        }
            
        if (Unlocked(SkillUpgradeType.Crystal_TeleportHpRewind) && !OnCoolDown() )
        {
            HandleCrystalRewind();
        }
            
        else
        {
            CanUseSkill();
            
            if(Unlocked(SkillUpgradeType.Crystal_MultiCast))
                HandleCrystalMulticast();
        }
    }
    
    protected override void UseSkill()
    {
        //use skill
        base.UseSkill();
        
        if(upgradeType == SkillUpgradeType.None) 
            return;
        
        if(Unlocked(SkillUpgradeType.Crystal))
            HandleCrystalRegular();
        
        if(Unlocked(SkillUpgradeType.Crystal_MoveToEnemy))
            HandleCrystalMoving();
        
    }
    
    private void CreateCrystal()
    {
        var crystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        _currentCrystal = crystal.GetComponent<SkillObject_Crystal>();
        _currentCrystal.SetupCrystal(this);
    }

    public void CreateRawCrystal(Transform target = null, bool crystalCanMove = false)
    {
        var canMove = crystalCanMove == false ? 
            Unlocked(SkillUpgradeType.Crystal_MoveToEnemy) || Unlocked(SkillUpgradeType.Crystal_MultiCast) : crystalCanMove;
        
        var crystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        crystal.GetComponent<SkillObject_Crystal>().SetupCrystal(this, detonateTime, canMove, crystalMoveSpeed, target);
    }

    private void HandleCrystalRewind()
    {
        if (_currentCrystal == null)
        {
            CreateCrystal();
            savedHealthPercent = player.GetHealthPercent();
        }
        else
        {
            SwapPlayerAndCrystal();
            player.SetHealthToPercent(savedHealthPercent);
            SetSkillOnCooldown();
            _currentCrystal = null;
        }
    }
    private void HandleCrystalTeleport()
    {
        if (_currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            SwapPlayerAndCrystal();
            SetSkillOnCooldown();
            _currentCrystal = null;
        }
    }

    private void SwapPlayerAndCrystal()
    {
        var crystalPosition = _currentCrystal.transform.position;
        var playerPosition = player.transform.position;
        
        _currentCrystal.transform.position = playerPosition;
        _currentCrystal.Explode();
        player.TeleportPlayer(crystalPosition);
    }

    private void HandleCrystalRegular()
    {
        CreateCrystal();
    }
    private void HandleCrystalMulticast()
    {
        if(currentCharges <= 0)
            return;
        
        CreateCrystal();
        _currentCrystal.MoveTowardsClosestTarget(crystalMoveSpeed);
        currentCharges--;

        if (isRecharging == false)
            StartCoroutine(CrystalRechargeCoroutine());
    }

    private IEnumerator CrystalRechargeCoroutine()
    {
        isRecharging = true;

        while(currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currentCharges++;
        }

        isRecharging = false;
    }
    private void HandleCrystalMoving()
    {
        CreateCrystal();
        _currentCrystal.MoveTowardsClosestTarget(crystalMoveSpeed);
    }
    

    public float GetDetonateTime()
    {
        if (Unlocked(SkillUpgradeType.Crystal_Teleport) || Unlocked(SkillUpgradeType.Crystal_TeleportHpRewind))
            return crystalExistDuration;

        return detonateTime;
    }

}
