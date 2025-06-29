using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathState : EnemyState
{
    public SkeletonDeathState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Enter()
    {
        base.Enter();
        enemyStateMachine.SwitchOffStateMachine();
    }
}
