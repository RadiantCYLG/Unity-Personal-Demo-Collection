using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//当敌人检测到玩家后，进入战斗状态
public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private float lastTimeInBattle; //上一次处于战斗中的时间

    public Enemy_BattleState(Enemy enemy, FSM stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        UpdateBattleTimer();

        //获取到玩家对象
        if (player == null)
        {
            player = enemy.GetPlayer();
        }

        //判断玩家是否贴脸，需要后撤
        if (IsRetreat())
        {
            enemy.rb.velocity = new Vector2(enemy.retreatVelocity.x * -FaceToPlayer(), enemy.retreatVelocity.y);
            //后撤之后，立马面向玩家
            enemy.CheckFlip(FaceToPlayer());
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //如果检测到玩家
        if (enemy.CheckPlayer())
            UpdateBattleTimer(); //更新战斗计时

        //如果战斗时间结束
        if (BattleTimeIsOver() || enemy.isTouchWall)
            stateMachine.ChangeState(enemy.idleState); //切换为待机状态

        //如果检测玩家在攻击范围内
        if (enemy.CheckPlayer() && InAttackRange())
        {
            //则切换为攻击状态
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        //否则快速朝玩家移动
        {
            enemy.SetVelocity(enemy.battleMoveSpeed * FaceToPlayer(), enemy.rb.velocity.y);
        }
    }

    private void UpdateBattleTimer() => lastTimeInBattle = Time.time;

    private bool BattleTimeIsOver() => Time.time > lastTimeInBattle + enemy.battleTimeDuration;

    private bool IsRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;


    /// <summary>
    /// 判断玩家是否在攻击范围内
    /// </summary>
    private bool InAttackRange()
    {
        return DistanceToPlayer() < enemy.attackDistance;
    }

    /// <summary>
    /// 计算到玩家的距离
    /// </summary>
    private float DistanceToPlayer()
    {
        if (player == null)
        {
            return float.MaxValue; //float.MaxValue 是一个常量，表示 float 类型能存储的最大正数值
        }
        //返回玩家与敌人之间距离差的绝对值
        return Mathf.Abs(player.transform.position.x - enemy.transform.position.x);
    }

    /// <summary>
    /// 敌人朝向玩家
    /// </summary>
    private int FaceToPlayer()
    {
        if (player == null)
        {
            return 0; //敌人会不动
        }
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
