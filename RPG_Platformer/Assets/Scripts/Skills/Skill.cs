using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] private protected float cooldown;
    private float _cooldownTimer;
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        _cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (_cooldownTimer < 0)
        {
            UseSkill();
            _cooldownTimer = cooldown;
            return true;
        }
        return false;
    }

    public virtual void UseSkill()
    {
        //use skill
    }
}
