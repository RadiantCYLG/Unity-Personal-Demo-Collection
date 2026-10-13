using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Íæ¼Ò×´Ì¬»ùÀà
public abstract class PlayerState : BaseState
{
    protected Player player;

    public PlayerState(Player player, FSM stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        anim = player.anim;
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetKeyDown(KeyCode.LeftShift) && player.CanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
    }

    protected override void UpdateAnimatorParameters()
    {
        base.UpdateAnimatorParameters();
        anim.SetFloat("yVelocity", player.rb.velocity.y);
    }

}