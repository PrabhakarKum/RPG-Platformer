using System;
using UnityEngine;

public class PlayerState
{
    protected readonly Player player;
    protected readonly PlayerStateMachine playerStateMachine;
    private readonly Entity_Stats _entityStats;

    protected float xInput;
    protected float yInput;
    private readonly string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    protected PlayerState(Player player, PlayerStateMachine playerStateMachine, string animBoolName)
    {
        this.playerStateMachine = playerStateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
        _entityStats = player.entityStats;
    }

    public virtual void Enter()
    {
        player.animator.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        player.animator.SetFloat("yVelocity", player.rigidBody.velocity.y);
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animBoolName, false);
    }

    public void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    protected void SyncAttackSpeed()
    {
        var attackSpeed = _entityStats.offence.attackSpeed.GetValue();
        player.animator.SetFloat("AttackSpeedMultiplier", attackSpeed);
    }
}
