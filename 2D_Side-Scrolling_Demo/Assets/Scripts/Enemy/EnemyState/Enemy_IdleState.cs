using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IdleState : Enemy_GroundState
{
    public Enemy_IdleState(Enemy enemy, FSM stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateTimer = enemy.idleTime;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
