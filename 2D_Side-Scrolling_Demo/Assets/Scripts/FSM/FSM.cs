using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//有限状态机
//管理状态，记录当前处于哪个状态，并负责状态的切换
public class FSM
{
    public BaseState currentState { get; private set; }

    /// <summary>
    /// 游戏运行时，初始化状态
    /// </summary>
    public void InitialState(BaseState startState)
    {
        currentState = startState;
        currentState.OnEnter();
    }

    /// <summary>
    /// 改变状态
    /// </summary>
    public void ChangeState(BaseState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    /// <summary>
    /// 持续更新当前状态
    /// </summary>
    public void UpdateCurState()
    {
        currentState.OnUpdate();
    }
}
