using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private EnemySkeleton enemySkeleton;
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemySkeleton, _enemyStateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }


    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            enemyStateMachine.ChangeState(enemySkeleton.idleState);
        }
    }

    public override void Enter()
    {
        base.Enter();
        
        enemySkeleton.entityVFX.InvokeRepeating("RedColorBlink", 0f, 0.1f);
        stateTimer = enemySkeleton.stunDuration;
        enemySkeleton.rigidBody.velocity = new Vector2(-enemySkeleton.facingDirection * enemySkeleton.stunDirection.x, enemySkeleton.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemySkeleton.entityVFX.Invoke("CancelRedBlink", 0f);
    }
}
