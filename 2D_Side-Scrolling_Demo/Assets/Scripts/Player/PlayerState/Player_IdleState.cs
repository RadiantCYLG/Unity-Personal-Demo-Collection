using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_IdleState : Player_GroundState
{
    public Player_IdleState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //每次进入闲置状态时，让玩家在水平方向的速度置0，不改变竖直方向的速度
        player.SetVelocity(0, player.rb.velocity.y);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //输入方向和玩家朝向相同，并且碰墙时
        if (player.xInput == player.rayFaceDir && player.isTouchWall)
        {
            return;
        }

        if (player.xInput != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

}
