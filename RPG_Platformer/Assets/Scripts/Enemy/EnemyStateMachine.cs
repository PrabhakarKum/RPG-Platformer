using UnityEngine;

public class EnemyStateMachine
{ 
    public EnemyState currentState { get; private set; }
    public bool canChangeState;
    public void Initialize(EnemyState startState)
    {
        canChangeState = true;
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        if(canChangeState == false)
            return;
        
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void SwitchOffStateMachine() => canChangeState = false;
}
