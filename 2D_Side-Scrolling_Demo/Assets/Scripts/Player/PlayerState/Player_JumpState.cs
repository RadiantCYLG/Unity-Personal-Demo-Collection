using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpState : Player_AirState
{
    public Player_JumpState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //进入跳跃状态时，让玩家向上朓一次
        player.SetVelocity(player.rb.velocity.x, player.jumpForce);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //如果玩家开始下落，则切换为下落状态
        if (player.rb.velocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
        {
            stateMachine.ChangeState(player.fallState);
        }
        //在跳跃时，改变玩家朝向
        else if (player.rb.velocity.y > 0 && (Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.D))))
        {
            player.SetVelocity(player.xInput * player.moveSpeed, player.rb.velocity.y);
        }
    }
}
