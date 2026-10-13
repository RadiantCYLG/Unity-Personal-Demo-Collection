using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GroundState : EnemyState
{
    public Enemy_GroundState(Enemy enemy, FSM stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(enemy.CheckPlayer()==true)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
