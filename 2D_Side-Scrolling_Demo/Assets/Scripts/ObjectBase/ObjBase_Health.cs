using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjBase_Health : MonoBehaviour, IDamage
{
    [SerializeField]private Slider hpUI;
    private ObjBase_VFX objVfx;
    private ObjBase objBase;

    [SerializeField] protected float currentHp;
    [SerializeField] protected float maxHp = 100;
    [SerializeField] protected bool isDead;

    [Header("受伤被击退")]
    [SerializeField] protected Vector2 knockbackPower = new Vector2(1.5f, 2.5f); //受伤被击退向量
    [SerializeField] protected float knockbackDuration = 0.2f; //受伤被击退效果时间
    [SerializeField] protected Vector2 heavyKnockbackPower = new Vector2(7, 5); //受重击被击退向量
    [SerializeField] protected float heavyknockbackDuration = 0.5f; //受重击被击退效果时间


    [Header("受到重击")]
    [SerializeField] protected float heavyDamageThreshold = 0.3f; //伤害超过maxHp的百分值，则视为受到重击

    protected virtual void Awake()
    {
        //hpUI = GetComponentInChildren<Slider>();
        objVfx = GetComponent<ObjBase_VFX>();
        objBase = GetComponent<ObjBase>();

        currentHp = maxHp;
    }

    /// <summary>
    /// 更新UI血量显示
    /// </summary>
    private void UpdateHpUI()
    {
        if (hpUI == null)
        {
            return;
        }
        hpUI.value = currentHp / maxHp;
    }

    /// <summary>
    /// 受到伤害的方法
    /// </summary>
    /// <param name="damage">伤害值</param>
    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
        {
            return;
        }
        Vector2 knockback = CalculateKnockBack(damage, damageDealer);
        float duration = CalculateKnockBackDuration(damage);

        objBase?.ExecuteKnockBack(knockback, duration);
        objVfx?.PlayOnDamageVfx();
        ReduceHp(damage);
    }

    /// <summary>
    /// 计算受击力
    /// </summary>
    /// <param name="damageDealer">伤害造成者</param>
    /// <returns></returns>
    private Vector2 CalculateKnockBack(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;

        knockback.x *= direction;//只改变水平方向
        return knockback;
    }

    /// <summary>
    /// 计算需要的时间
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    private float CalculateKnockBackDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyknockbackDuration : knockbackDuration;
    }

    private bool IsHeavyDamage(float damage)
    {
        return damage / maxHp > heavyDamageThreshold;
    }

    /// <summary>
    /// 血量减少
    /// </summary>
    /// <param name="damage">减少的值</param>
    private void ReduceHp(float damage)
    {
        currentHp -= damage;
        UpdateHpUI();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 角色死亡
    /// </summary>
    private void Die()
    {
        isDead = true;
        objBase.ObjectBaseDeath();
        Debug.Log(gameObject.name + "已死亡");
    }

}
