using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作为所有敌人的基类
public class Enemy : ObjBase
{
    public Enemy_IdleState idleState; //敌人闲置状态
    public Enemy_MoveState moveState; //敌人移动状态
    public Enemy_AttackState attackState; //敌人攻击状态
    public Enemy_BattleState battleState; //敌人检测到玩家后进入的战斗状态
    public Enemy_DeadState deadState; //敌人死亡状态
    public Enemy_StunnedState stunnedState; //敌人眩晕状态

    [Header("移动相关")]
    public float idleTime = 2f; //敌人闲置时间
    public float moveSpeed = 1.6f; //敌人移动速度
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1; //移速与动画播放速度的关联

    [Header("战斗状态相关")]
    public float battleMoveSpeed = 3f; //战斗状态下的速度
    public float attackDistance = 2f;//攻击距离（玩家在距离内则立刻攻击玩家）
    public float battleTimeDuration = 5f; //玩家脱离范围后，战斗持续时间
    public float minRetreatDistance = 1; //玩家和敌人小于该距离时，敌人后撤
    public Vector2 retreatVelocity; //后撤的速度

    [Header("检测玩家")]
    [SerializeField] private LayerMask whatIsPlayer; //检测图层为玩家
    [SerializeField] private Transform checkPlayerPoint; //射线起点
    [SerializeField] private float checkPlayerDistance; //射线距离
    public Transform player { get; private set; }

    [Header("眩晕相关")]
    public float stunnedDuration = 0.3f; //眩晕持续时间
    public Vector2 stunnedVelocity = new Vector2(3f, 4f);//眩晕作用速度（矢量）
    public bool canBeCounter { get; private set; } //是否能被反击眩晕

    private void OnEnable()
    {
        Player.onPlayerDeath += ChangeStateOnPlayerDeath;
    }

    private void OnDisable()
    {
        Player.onPlayerDeath -= ChangeStateOnPlayerDeath;
    }

    public override void ObjectBaseDeath()
    {
        base.ObjectBaseDeath();
        fSM.ChangeState(deadState);
    }

    private void ChangeStateOnPlayerDeath()
    {
        fSM.ChangeState(idleState);
    }

    public void EnableEnemyCanbeCounter(bool enable)
    {
        canBeCounter = enable;
    }

    /// <summary>
    /// 尝试进入攻击状态
    /// </summary>
    public void EnterBattleState(Transform player)
    {
        if (fSM.currentState == battleState || fSM.currentState == attackState)
        {
            return;
        }

        if (player != null)
        {
            this.player = player;
        }
        fSM.ChangeState(battleState);
    }

    /// <summary>
    /// 获取玩家
    /// </summary>
    /// <returns>player对象</returns>
    public Transform GetPlayer()
    {
        if (player == null)
        {
            player = CheckPlayer().transform;
        }
        return player;
    }

    /// <summary>
    /// 检测玩家
    /// </summary>
    /// <returns>若射线撞到了东西，物理引擎就会把撞击的所有细节打包进RaycastHit2D这个结构体里返回</returns>
    public RaycastHit2D CheckPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPlayerPoint.position, Vector2.right * rayFaceDir, 
            checkPlayerDistance, whatIsPlayer | whatIsGroundOrWall);
        //检测到的不是玩家
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return default; //返回默认值
        }
        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        //玩家检测射线
        Gizmos.color = Color.yellow; //改变射线的颜色
        Gizmos.DrawLine(checkPlayerPoint.position, new Vector3(checkPlayerPoint.position.x + (checkPlayerDistance * rayFaceDir), checkPlayerPoint.position.y));
        //攻击范围检测射线
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(checkPlayerPoint.position, new Vector3(checkPlayerPoint.position.x + (attackDistance * rayFaceDir), checkPlayerPoint.position.y));
        //后撤检测射线
        Gizmos.color = Color.red;
        Gizmos.DrawLine(checkPlayerPoint.position, new Vector3(checkPlayerPoint.position.x + (minRetreatDistance * rayFaceDir), checkPlayerPoint.position.y));
    }

}
