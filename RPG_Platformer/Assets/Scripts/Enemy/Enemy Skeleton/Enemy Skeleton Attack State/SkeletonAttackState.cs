using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private EnemySkeleton enemySkeleton;
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemySkeleton, _enemyStateMachine, _animBoolName)
    {
        this.enemySkeleton = _enemySkeleton;
    }

    public override void Update()
    {
        base.Update();
        
        enemySkeleton.SetZeroVelocity();
        
        if(triggerCalled)
            enemyStateMachine.ChangeState(enemySkeleton.battleState);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemySkeleton.lastTimeAttacked = Time.time;
    }
}
