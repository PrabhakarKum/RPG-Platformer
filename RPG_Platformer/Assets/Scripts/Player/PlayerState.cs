using System;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine playerStateMachine;
    protected Player player;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    protected PlayerState(Player player, PlayerStateMachine playerStateMachine, string animBoolName)
    {
        this.playerStateMachine = playerStateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
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

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
