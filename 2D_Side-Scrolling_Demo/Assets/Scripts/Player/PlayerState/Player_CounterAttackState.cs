using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CounterAttackState : PlayerState
{//玩家反击状态

    private Player_Combat combat;
    private bool hasCounteredEnemy;

    public Player_CounterAttackState(Player player, FSM stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        //重置是否反击到敌人
        hasCounteredEnemy = combat.IsCounterAttackExecute();
        //重置反击动画
        anim.SetBool("counterAttackExecute", hasCounteredEnemy);
        //设定反击时间
        stateTimer = combat.GetCounterRecoveryTime();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        player.SetVelocity(0, player.rb.velocity.y);

        //通过反击目标完成，判断是否退出反击状态
        //反击动画结束
        if (isTriggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

        //通过计时器退出反击状态
        if (stateTimer < 0 && hasCounteredEnemy == false)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
