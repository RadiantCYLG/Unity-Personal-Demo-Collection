using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy, FSM stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //触发器为真，则从攻击状态转为闲置状态
        if (isTriggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
