using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player_MoveState : Player_GroundState
{
    public Player_MoveState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (player.xInput == 0 || player.isTouchWall)
        {
            stateMachine.ChangeState(player.idleState);
        }
        //设置二维向量，赋给Rigidbody2D
        player.SetVelocity(player.xInput * player.moveSpeed, player.rb.velocity.y);
    }

}
