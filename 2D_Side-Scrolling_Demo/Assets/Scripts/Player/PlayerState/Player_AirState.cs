using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AirState : PlayerState
{
    public Player_AirState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //玩家跳跃至空中时，控制左右移动
        if (player.xInput != 0)
        {
            player.SetVelocity(player.xInput * (player.moveSpeed * player.inAirMoveMultiplier), player.rb.velocity.y);
        }

        //玩家跳跃至空中时，按下鼠标左键触发跳跃攻击
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.jumpAttackState);
        }
    }
}
