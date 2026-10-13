using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FallState : Player_AirState
{
    public Player_FallState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
       
        if (player.isOnGround == true)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (player.isTouchWall == true)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
