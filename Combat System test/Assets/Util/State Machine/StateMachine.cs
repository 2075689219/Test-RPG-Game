using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public State<T> CurrentState { get; private set; }
    private T _owner;

    public StateMachine(T owner)
    {
        _owner = owner;
    }

    public void ChangeState(State<T> newState)
    {
        CurrentState?.Exit();//如果当前状态不为空，就退出当前状态
        CurrentState = newState;
        CurrentState.Enter(_owner);

    }
    public void Execute()
    {
        CurrentState?.Execute();
    }
}
