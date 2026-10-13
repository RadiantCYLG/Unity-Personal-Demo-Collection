using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MinHpUI : MonoBehaviour
{
    private ObjBase objBase;

    private void Awake()
    {
        objBase = GetComponentInParent<ObjBase>();
    }

    private void OnEnable()
    {
        objBase.onFlipUI += FlipUI;
    }

    private void OnDisable()
    {
        objBase.onFlipUI -= FlipUI;
    }

    private void FlipUI()
    {
        transform.rotation = Quaternion.identity;
    }
}
