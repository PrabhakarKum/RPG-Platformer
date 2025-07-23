using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemySkeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemySkeleton.SetVelocity(enemySkeleton.GetMoveSpeed() * enemySkeleton.facingDirection,
            enemySkeleton.rigidBody.velocity.y);

        if (enemySkeleton.IsWallDetected() || !enemySkeleton.IsGroundDetected())
        {
            enemySkeleton.Flip();
            enemyStateMachine.ChangeState(enemySkeleton.idleState);
        }
    }
}