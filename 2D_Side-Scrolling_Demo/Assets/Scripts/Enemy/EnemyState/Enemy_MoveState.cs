using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveState : Enemy_GroundState
{
    public Enemy_MoveState(Enemy enemy, FSM stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //让敌人持续移动
        enemy.SetVelocity(enemy.moveSpeed * enemy.rayFaceDir, enemy.rb.velocity.y);
        //如果到达地面的边缘，或者碰到墙面时，则停下
        if (enemy.isOnGround == false || enemy.isTouchWall )
        {
            stateMachine.ChangeState(enemy.idleState);
            //转向
            enemy.Flip();
        }
    }
}
