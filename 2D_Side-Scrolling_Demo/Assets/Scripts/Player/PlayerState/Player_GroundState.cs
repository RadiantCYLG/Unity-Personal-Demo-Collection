using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GroundState : PlayerState
{
    public Player_GroundState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //玩家y轴突然变负，下落，则改变玩家状态。如：跳崖
        if (player.rb.velocity.y < 0 && player.isOnGround == false)
        {
            stateMachine.ChangeState(player.fallState);
        }
        //按下空格键，切换为跳跃状态
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
        }
        //按下鼠标左键，玩家攻击（切换为玩家攻击状态）
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.basicAttackState);
        }
        //按下Q进入反击状态
        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.counterAttackState);
        }
    }
}
