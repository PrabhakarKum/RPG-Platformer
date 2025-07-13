using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathState : EnemyState
{
    private Enemy _enemy;
    public SkeletonDeathState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        _enemy = _enemyBase;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.counterImage.SetActive(false);
        enemyStateMachine.SwitchOffStateMachine();
    }
}
