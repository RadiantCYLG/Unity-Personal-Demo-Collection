using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        FlipSprite();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        HandleWallSlide();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
        }

        if (player.isTouchWall == false)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (player.isOnGround)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        FlipSprite();
    }

    /// <summary>
    /// 处理墙壁滑行时的速度
    /// </summary>
    private void HandleWallSlide()
    {
        //按下S或向下键，下落速度
        if (player.yInput < 0)
        {
            player.SetVelocity(player.xInput, player.rb.velocity.y);
        }
        //不按下S或向下键，缓慢
        else
        {
            player.SetVelocity(player.xInput, player.rb.velocity.y * player.inAirFallMultiplier);
        }
    }

    /// <summary>
    /// 翻转WallSlide对应的Spite
    /// </summary>
    private void FlipSprite()
    {
        player.sr.flipX = !player.sr.flipX;
    }
}
