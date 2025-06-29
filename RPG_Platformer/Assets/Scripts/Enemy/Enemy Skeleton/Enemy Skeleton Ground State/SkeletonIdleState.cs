using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemySkeleton, _enemyStateMachine, _animBoolName, _enemySkeleton)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemySkeleton.idleTime;
        enemySkeleton.SetVelocity(0f, enemySkeleton.rigidBody.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        if (stateTimer < 0)
        {
            enemyStateMachine.ChangeState(enemySkeleton.moveState);
        }
    }
}