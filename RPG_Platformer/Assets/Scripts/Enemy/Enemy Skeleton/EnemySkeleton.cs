using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public  SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    public SkeletonDeathState deathState { get; private set; }

    #endregion
    
    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }

    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, enemyStateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, enemyStateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, enemyStateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, enemyStateMachine, "Attack", this);
        stunnedState = new SkeletonStunnedState(this, enemyStateMachine, "Stunned", this);
        deathState= new SkeletonDeathState(this, enemyStateMachine, "Death", this );
    }

    protected override void Start()
    {
        base.Start();
        enemyStateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            enemyStateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    protected override void EntityDeath()
    {
        base.EntityDeath();
        enemyStateMachine.ChangeState(deathState);
    }

    private void HandlePlayerDeath()
    {
        isPlayerDead = true;
        enemyStateMachine.ChangeState(idleState);
    }
}
