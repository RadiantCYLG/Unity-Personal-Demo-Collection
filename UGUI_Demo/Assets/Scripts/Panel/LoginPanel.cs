using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    //登录注册按钮
    [SerializeField] private Button btnRegister;
    [SerializeField] private Button btnSureLogin;

    //账号、密码输入框
    [SerializeField] private InputField inputUN;
    [SerializeField] private InputField inputPW;

    //记住密码、自动登录开关
    [SerializeField] private Toggle togRemberPW;
    [SerializeField] private Toggle togAutoLogin;

    //勾选自动登录时，在登录界面短暂停留后进入下一界面
    private Coroutine delayAutoLogin;
    
    protected override void Init()
    {
        //注册事件监听
        btnRegister.onClick.AddListener(DoSth_OnClickBtnRegister);
        btnSureLogin.onClick.AddListener(DoSth_OnClickBtnSureLogin);
        togRemberPW.onValueChanged.AddListener(DoSth_OnValueChangedTogRemberPW);
        togAutoLogin.onValueChanged.AddListener(DoSth_OnValueChangedTogAutoLogin);
    }

    /// <summary>
    /// 自动登录开关
    /// </summary>
    private void DoSth_OnValueChangedTogAutoLogin(bool isOn)
    {
        if (isOn)
        {
            //勾选记住密码
            togRemberPW.isOn = true;
        }
    }

    /// <summary>
    /// 记住密码开关
    /// </summary>
    /// <param name="isOn">是否勾选</param>
    private void DoSth_OnValueChangedTogRemberPW(bool isOn)
    {
        if (!isOn)
        {
            //自动登录不勾选
            togAutoLogin.isOn = false;
        }
    }

    /// <summary>
    /// 点击确认登录按钮后要做的事
    /// </summary>
    private void DoSth_OnClickBtnSureLogin()
    {
        UI_Manager.Instance.PlayBtnSound(clickAudio);

        //验证账号和密码是否正确
        // 验证输入是否合规
        if (inputUN.text.Length <= 2 || inputPW.text.Length <= 6)
        {
            //提示
            TipPanel tipPanel = UI_Manager.Instance.ShowPanel<TipPanel>();
            tipPanel.ChangeTipTxt("密码必须大于6位");
            return;
        }

        // 验证登录信息是否正确
        if (LoginAndRegisterMgr.Instance.CheckLoginInfo(inputUN.text, inputPW.text))
        {
            //成功登录，先记录数据 
            LoginAndRegisterMgr.Instance.LoginData.userName = inputUN.text;
            LoginAndRegisterMgr.Instance.LoginData.password = inputPW.text;
            LoginAndRegisterMgr.Instance.LoginData.isRemeberPW = togRemberPW.isOn;
            LoginAndRegisterMgr.Instance.LoginData.isAutoLogin = togAutoLogin.isOn;
            LoginAndRegisterMgr.Instance.SaveLoginData();

            //根据是否选择过服务器，来决定选择显示的面板
            if (LoginAndRegisterMgr.Instance.LoginData.frontServerID <= 0)
            {
                //没有选择过服务器，显示选服面板
                UI_Manager.Instance.ShowPanel<SelectServerPanel>();
            }
            else
            {
                //显示区服面板
                UI_Manager.Instance.ShowPanel<ServerPanel>();
            }

            //隐藏当前面板
            UI_Manager.Instance.HidePanel<LoginPanel>();

        }
        else
        {
            //用户不存在
            if (LoginAndRegisterMgr.Instance.RegisterData.isFindUser == false)
            {
                UI_Manager.Instance.ShowPanel<TipPanel>().ChangeTipTxt("用户未注册");
            }
            //用户存在，但输入信息错误
            else
            {
                UI_Manager.Instance.ShowPanel<TipPanel>().ChangeTipTxt("账号或密码错误");
            }
        }
    }

    /// <summary>
    /// 点击注册按钮后要做的事
    /// </summary>
    private void DoSth_OnClickBtnRegister()
    {
        UI_Manager.Instance.PlayBtnSound(clickAudio);

        //显示注册面板
        UI_Manager.Instance.ShowPanel<RegisterPanel>();
        //隐藏登录面板 (true代表使用淡出效果)
        UI_Manager.Instance.HidePanel<LoginPanel>(true);
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //根据已有数据，更新面板数据
        LoginData loginDate = LoginAndRegisterMgr.Instance.LoginData;
        //更新记住密码、自动登录的显示
        togRemberPW.isOn = loginDate.isRemeberPW;
        togAutoLogin.isOn = loginDate.isAutoLogin;
        //更新账号和密码的显示
        inputUN.text = loginDate.userName;
        //如果要记住密码
        if (togRemberPW.isOn)
        {
            inputPW.text = loginDate.password;
        }

        //如果自动登录
        if (togAutoLogin.isOn)
        {
            DelayAutoLogin();
        }

    }

    /// <summary>
    /// 从外部快捷得到注册时的账号和密码
    /// </summary>
    public void SetInfo(string name, string password)
    {
        inputUN.text = name;
        inputPW.text = password;
    }

    private void DelayAutoLogin()
    {
        if (delayAutoLogin != null)
        {
            StopCoroutine(delayAutoLogin);
        }
        delayAutoLogin = StartCoroutine(DelayAutoLoginCo());
    }
    private IEnumerator DelayAutoLoginCo()
    {
        yield return new WaitForSeconds(1f);
        //1秒后，判断自动登录是否仍然处于勾选状态
        if (togAutoLogin.isOn)
        {
            //根据是否选择过服务器，来决定选择显示的面板
            if (LoginAndRegisterMgr.Instance.LoginData.frontServerID <= 0)
            {
                //-1表示没有选择过服务器，显示选服面板
                UI_Manager.Instance.ShowPanel<SelectServerPanel>();
            }
            else
            {
                //显示区服面板
                UI_Manager.Instance.ShowPanel<ServerPanel>();
            }

            //隐藏当前面板
            UI_Manager.Instance.HidePanel<LoginPanel>(false);
        }
    }
}
