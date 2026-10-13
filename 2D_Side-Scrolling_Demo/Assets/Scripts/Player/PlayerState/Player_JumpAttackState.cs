using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpAttackState : PlayerState
{
    private bool isTouchGround;
    public Player_JumpAttackState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //每次进入当前状态就重置 接地
        isTouchGround = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //如果玩家从空中跳跃至地面
        if (player.isOnGround && isTouchGround == false)
        {
            isTouchGround = true;
            //触发Player_JumpAttack_Execute动画
            player.anim.SetTrigger("jumpAttackTrigger");
            //落地时，跳跃攻击状态未结束，玩家不能移动
            player.SetVelocity(0, player.rb.velocity.y);
        }

        //如果玩家从空中跳跃至地面，同时激活了 攻击动画结束触发器
        if (player.isOnGround && isTriggerCalled)
        {
            //玩家状态变为待机
            stateMachine.ChangeState(player.idleState);
        }

    }
}
