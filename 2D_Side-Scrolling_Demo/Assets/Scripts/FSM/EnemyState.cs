using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : BaseState
{
    protected Enemy enemy;
    protected EnemyState(Enemy enemy, FSM stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;

        anim = enemy.anim;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void UpdateAnimatorParameters()
    {
        base.UpdateAnimatorParameters();
        float battleAnimSpeedMultiplier = enemy.battleMoveSpeed / enemy.moveSpeed;

        //将敌人的移动速度与动画速率（根据采样率）进行关联
        anim.SetFloat("battleAnimSpeedMultiplier", battleAnimSpeedMultiplier);
        anim.SetFloat("moveAnimSpeedMultiplier", enemy.moveAnimSpeedMultiplier);
        anim.SetFloat("xVelocity",enemy.rb.velocity.x);
    }
}
