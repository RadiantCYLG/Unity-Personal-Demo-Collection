using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ñ£ÔÎ×´Ì¬
public class Enemy_StunnedState : EnemyState
{
    private Enemy_VFX vfx;
    public Enemy_StunnedState(Enemy enemy, FSM stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        vfx = enemy.GetComponent<Enemy_VFX>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        vfx.EnableCounterOnAttack(false);
        enemy.EnableEnemyCanbeCounter(false);

        stateTimer = enemy.stunnedDuration;
        enemy.rb.velocity = new Vector2(enemy.stunnedVelocity.x * -enemy.rayFaceDir, enemy.stunnedVelocity.y);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }


}
