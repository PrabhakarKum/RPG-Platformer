using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class SkeletonGroundState : EnemyState
{
    protected EnemySkeleton enemySkeleton;
    private Transform _player;

    protected SkeletonGroundState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemySkeleton, _enemyStateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }

    public override void Update()
    {
        base.Update();
        
        if (enemySkeleton.EnemyDead())
            return;
        
        if(enemySkeleton.isPlayerDead)
            return;
        
        
        if (enemySkeleton.IsPlayerDetected() || Vector3.Distance(enemySkeleton.transform.position, _player.position) < 1f)
        {
            enemyStateMachine.ChangeState(enemySkeleton.battleState);
        }
    }

    public override void Enter()
    {
        base.Enter();
        _player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
