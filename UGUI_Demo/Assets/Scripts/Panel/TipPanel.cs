using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class TipPanel : BasePanel
{
    [SerializeField] private Button btnSure;
    [SerializeField] private Text txtTip; //提示文本
    [SerializeField] private Image imgTipBg;//提示背景控件

    [SerializeField] private Sprite tipBgImg_White; //提示背景图

    protected override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UI_Manager.Instance.PlayBtnSound(clickAudio);
            UI_Manager.Instance.HidePanel<TipPanel>();
        });
    }

    public void ChangeTipTxt(string txtInfo)
    {
        txtTip.text = txtInfo;
    }

    /// <summary>
    /// 改变提示背景图为白色版
    /// </summary>
    public void ChangeBgToBeWhite()
    {
        imgTipBg.sprite = tipBgImg_White;
        txtTip.color = Color.black;
    }
}
