using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DashState : PlayerState
{

    public Player_DashState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateTimer = player.dashDuration;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        player.SetVelocity(player.dashSpeed * player.rayFaceDir, 0); //注：让冲刺时不受重力影响
        CancelDashIfNeed();
        //冲刺计时结束就退出冲刺状态
        if (stateTimer < 0)
        {
            //在地面时
            if (player.isOnGround)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        player.lastDashTimer=Time.time;
        //退出冲刺状态时，不将冲刺速度带到其他状态
        player.SetVelocity(0, 0);
    }

    /// <summary>
    /// 在一些特别情况下退出冲刺状态
    /// </summary>
    private void CancelDashIfNeed()
    {
        //没冲刺完就碰到墙壁
        if (player.isTouchWall)
        {
            //处于地面时碰墙
            if (player.isOnGround)
                stateMachine.ChangeState(player.idleState);
            //处于半空中时碰墙
            else
                stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
