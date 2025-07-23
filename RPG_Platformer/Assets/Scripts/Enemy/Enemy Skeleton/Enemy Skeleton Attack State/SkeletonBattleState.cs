using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Transform lastTarget;
    private EnemySkeleton enemySkeleton;
    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemySkeleton, _enemyStateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }

    public override void Update()
    {
        base.Update();

        if (enemySkeleton.IsPlayerDetected())
        {
            UpdateTargetIfNeeded();
            stateTimer = enemySkeleton.battleTime;
            if (enemySkeleton.IsPlayerDetected().distance < enemySkeleton.attackDistance)
            {
                if(CanAttack())
                    enemyStateMachine.ChangeState(enemySkeleton.attackState);
                
            }
        }
        else
        {
            if (stateTimer < 0 || Vector3.Distance(player.transform.position, enemySkeleton.transform.position) > 7)
                enemyStateMachine.ChangeState(enemySkeleton.idleState);
        }

        if (Mathf.Abs(player.position.x - enemySkeleton.transform.position.x) > enemySkeleton.flipThreshHold)
        {
            if (player.position.x > enemySkeleton.transform.position.x)
            {
                moveDir = 1;
            }
            else if(player.position.x < enemySkeleton.transform.position.x)
            {
                moveDir = -1;
            }
        }
        
        
        enemySkeleton.SetVelocity(enemySkeleton.GetBattleMoveSpeed() * moveDir, enemySkeleton.rigidBody.velocity.y);
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time > enemySkeleton.lastTimeAttacked + enemySkeleton.attackCooldown)
        {
            enemySkeleton.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

    private void UpdateTargetIfNeeded()
    {
        if(enemySkeleton.IsPlayerDetected() == false)
            return;
        
        Transform newTarget = enemySkeleton.IsPlayerDetected().transform;
        if (newTarget != lastTarget)
        {
            lastTarget = newTarget;
            player = newTarget;
        }
    }
}
