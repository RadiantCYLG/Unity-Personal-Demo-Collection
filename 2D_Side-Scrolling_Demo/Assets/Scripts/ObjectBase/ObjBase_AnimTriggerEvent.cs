using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class ObjBase_AnimTriggerEvent : MonoBehaviour
{
    private ObjBase objBase;
    private ObjBase_Combat objectCombat;
    protected virtual void Awake()
    {
        objBase = GetComponentInParent<ObjBase>();
        objectCombat = GetComponentInParent<ObjBase_Combat>();
    }

    private void AnimTriggerComeTrue()
    {
        objBase.CallAnimTrigger();
    }

    private void AttackTrigger()
    {
        objectCombat.ExecuteAttack();
    }
}
