using UnityEngine;

public class EnemyState
{
    protected readonly EnemyStateMachine enemyStateMachine;
    private readonly Enemy _enemy;
    private readonly Entity_Stats _entityStats;
    
    private string animBoolName;
    
    protected float stateTimer;
    protected bool triggerCalled;

    protected EnemyState(Enemy _enemy, EnemyStateMachine _enemyStateMachine, string _animBoolName)
    {
        this._enemy = _enemy;
        enemyStateMachine = _enemyStateMachine;
        animBoolName = _animBoolName;
        _entityStats = _enemy.entityStats;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Enter()
    { 
        triggerCalled = false;  
        _enemy.animator.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        _enemy.animator.SetBool(animBoolName, false);
    }

    public void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    protected void SyncAttackSpeed()
    {
        var attackSpeed = _entityStats.offence.attackSpeed.GetValue();
        _enemy.animator.SetFloat("AttackSpeedMultiplier", attackSpeed);
    }
    
    
}
