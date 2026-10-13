using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy, ICounter
{
    protected override void Awake()
    {
        base.Awake();
        idleState = new Enemy_IdleState(this, fSM, "idle");
        moveState = new Enemy_MoveState(this, fSM, "move");
        attackState = new Enemy_AttackState(this, fSM, "attack");
        battleState = new Enemy_BattleState(this, fSM, "battle");
        deadState = new Enemy_DeadState(this, fSM, "idle"); //敌人死亡时要冻结动画，所以这个 "" 参数是什么不重要
        stunnedState = new Enemy_StunnedState(this, fSM, "stunned");
    }

    protected override void Start()
    {
        base.Start();
        fSM.InitialState(idleState);
    }

    /// <summary>
    /// 在敌人发动攻击的某一时刻可以反击
    /// </summary>
    public void CheckBeCounter()
    {
        if (canBeCounter == false && fSM.currentState != stunnedState)
        {
            return;
        }
        fSM.ChangeState(stunnedState);
    }
}
