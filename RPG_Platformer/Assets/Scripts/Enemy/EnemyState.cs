using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine enemyStateMachine;
    protected Enemy enemyBase;
    
    private string animBoolName;
    
    protected float stateTimer;
    protected bool triggerCalled;
    
    public EnemyState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName)
    {
        enemyBase = _enemyBase;
        enemyStateMachine = _enemyStateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Enter()
    { 
        triggerCalled = false;  
        enemyBase.animator.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        enemyBase.animator.SetBool(animBoolName, false);
    }

    public void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
    
    
}
