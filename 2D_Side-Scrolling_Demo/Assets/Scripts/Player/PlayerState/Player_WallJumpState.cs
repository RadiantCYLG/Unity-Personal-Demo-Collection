using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//贴墙跳跃状态
public class Player_WallJumpState : PlayerState
{
    public Player_WallJumpState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        //给贴墙滑行的玩家一个斜上方的跳跃力
        player.SetVelocity(player.wallJumpForce.x * -player.rayFaceDir, player.wallJumpForce.y);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (player.rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
        }
        if (player.isTouchWall)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }

}
