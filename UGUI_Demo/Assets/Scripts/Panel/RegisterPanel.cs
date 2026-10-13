using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    [SerializeField] private InputField inputUN;
    [SerializeField] private InputField inputPW;
    [SerializeField] private Button btnCancel;
    [SerializeField] private Button btnSure;

    protected override void Init()
    {
        btnCancel.onClick.AddListener(DoSth_OnClickBtnCancel);
        btnSure.onClick.AddListener(DoSth_OnClickBtnSure);
    }

    private void DoSth_OnClickBtnSure()
    {
        UI_Manager.Instance.PlayBtnSound(clickAudio);

        //判断输入的账号和密码是否合理
        if (inputUN.text.Length <= 2 || inputPW.text.Length <= 6)
        {
            //提示
            TipPanel tipPanel = UI_Manager.Instance.ShowPanel<TipPanel>();
            tipPanel.ChangeTipTxt("密码必须大于6位");
            return;
        }

        //正常注册
        if (LoginAndRegisterMgr.Instance.RegisterUserInfo(inputUN.text, inputPW.text))
        {
            /*注册成功*/

            //清理登录数据，用于新注册账号的LoginDate数据重置
            LoginAndRegisterMgr.Instance.ClearLoginData();

            //保存注册数据
            LoginAndRegisterMgr.Instance.SaveRegisterDate();
            //显示登录面板
            LoginPanel loginPanel = UI_Manager.Instance.ShowPanel<LoginPanel>();
            //将注册时的信息快捷赋给登录面板上
            loginPanel.SetInfo(inputUN.text, inputPW.text);
           

            //显示注册成功提示信息
            TipPanel tipPanel = UI_Manager.Instance.ShowPanel<TipPanel>();
            tipPanel.ChangeBgToBeWhite();
            tipPanel.ChangeTipTxt("注册成功");

            //隐藏自己
            UI_Manager.Instance.HidePanel<RegisterPanel>();
        }
        //该用户已注册过
        else
        {
            TipPanel tipPanel = UI_Manager.Instance.ShowPanel<TipPanel>();
            tipPanel.ChangeTipTxt("用户已存在");
            inputUN.text = "";
            inputPW.text = "";
        }
    }

    private void DoSth_OnClickBtnCancel()
    {
        UI_Manager.Instance.PlayBtnSound(clickAudio);

        //隐藏自己
        UI_Manager.Instance.HidePanel<RegisterPanel>(true);
        //显示登录面板
        UI_Manager.Instance.ShowPanel<LoginPanel>();
    }
}
