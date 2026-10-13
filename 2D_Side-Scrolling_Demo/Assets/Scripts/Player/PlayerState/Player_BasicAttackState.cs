using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//普通攻击只有在玩家处于地面时才触发
public class Player_BasicAttackState : Player_GroundState
{
    private float basicAttackVelocityTimer; //普通攻击惯性速度计时器
    private float lastTimeAttack; //记录玩家上次攻击的时间

    private bool isComboAttack; //玩家是否继续连击，
    private const int fristAttackIndex = 1; //攻击动画索引初始值
    private int attackIndex = fristAttackIndex; //攻击动画索引
    private int attackIndexLimtMax = 3; //攻击动画索引限制最大值

    private int attackDir; //用于决定玩家的攻击方向

    public Player_BasicAttackState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (attackIndexLimtMax != player.attackVelocity.Length)
        {
            Debug.LogWarning("根据attackVelocity数组，将调整连击动画的索引最大值限制");
            attackIndexLimtMax = player.attackVelocity.Length;
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        isComboAttack = false;
        ResetAttackIndexIfNeed();

        //攻击方向（检测这次攻击前是否有改变攻击方向的需求）
        attackDir = player.xInput != 0 ? ((int)player.xInput) : player.rayFaceDir;
        anim.SetInteger("basicAttackIndex", attackIndex); //设置动画器上的int参数（索引值），播放不同的攻击动画
        CreateAttackVelocity();
    }


    public override void OnUpdate()
    {
        base.OnUpdate(); 
        HandleAttackVelocity();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CheckComboAttack();
        }

        //判断攻击动画是否结束
        if (isTriggerCalled)
        {
            //结束时，检测攻击状态继续还是退出
            CheckAttackStateContinueOrExit();
        }
    }


    public override void OnExit()
    {
        base.OnExit();
        ++attackIndex;
        lastTimeAttack = Time.time;
    }

    /// <summary>
    /// 处理攻击时的移速/移动，玩家触发攻击时那一刻不移动
    /// </summary>
    private void HandleAttackVelocity()
    {
        basicAttackVelocityTimer -= Time.deltaTime;
        if (basicAttackVelocityTimer < 0)
        {
            player.SetVelocity(0, player.rb.velocity.y);
        }
    }

    /// <summary>
    /// 产生一个攻击惯性速度，角色普攻时，向朝向的方向移动一小段距离
    /// </summary>
    private void CreateAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[attackIndex - 1];
        basicAttackVelocityTimer = player.attackVelocityDuration; //初始化计时器的计时
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }

    /// <summary>
    /// 重置攻击动画的索引
    /// </summary>
    private void ResetAttackIndexIfNeed()
    {
        if (attackIndex > attackIndexLimtMax || Time.time > lastTimeAttack + player.resetAttackAfterTime)
        {
            attackIndex = fristAttackIndex;
        }
    }

    /// <summary>
    /// 处理攻击状态是继续还是退出
    /// </summary>
    private void CheckAttackStateContinueOrExit()
    {
        //如果可以连续攻击
        if (isComboAttack)
        {
            anim.SetBool(animBoolName, false); //当前帧为false
                                               //使用协程延迟执行
            player.DelayEnterAttackState();
        }
        else
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    /// <summary>
    /// 检测是否继续连击
    /// </summary>
    private void CheckComboAttack()
    {
        if (attackIndex < attackIndexLimtMax)
        {
            isComboAttack = true;
        }
    }
}
