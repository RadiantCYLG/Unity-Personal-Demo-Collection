using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    private Collider2D col;
    public Enemy_DeadState(Enemy enemy, FSM stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        col = enemy.GetComponent<Collider2D>();
    }

    public override void OnEnter()
    {
        anim.enabled = false; //禁用动画
        //让敌人直接掉落
        col.enabled = false; //禁用 Collider2D组件
        enemy.rb.gravityScale = 12;
        enemy.rb.velocity = new Vector2(enemy.rb.velocity.y, 15);
        GameObject.Destroy(enemy.gameObject, 1.5f);
    }
}
