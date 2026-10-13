using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.DisableInput();
        player.rb.simulated = false; //禁用2D刚体的物理模拟，否则敌人会在进入空闲状态一瞬间又检测到玩家
    }
}
