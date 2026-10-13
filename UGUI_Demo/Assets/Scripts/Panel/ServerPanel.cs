using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//区服面板（已经选择好了服器）
public class ServerPanel : BasePanel
{
    [SerializeField] private Image imgBg; //显示服务游戏区服的背景
    [SerializeField] private Text txtName; //区服的名字
    [SerializeField] private Button btnChange; //换区按钮
    [SerializeField] private Button btnEnterGame; //进入游戏按钮
    [SerializeField] private Button btnGoBack; //返回按钮

    protected override void Init()
    {
        btnChange.onClick.AddListener(DoSth_OnClickBtnChange);
        btnEnterGame.onClick.AddListener(DoSth_OnClickBtnEnterGame);
        btnGoBack.onClick.AddListener(DoSth_OnClickBtnGoBack);
    }

    private void DoSth_OnClickBtnGoBack()
    {
        UI_Manager.Instance.PlayBtnSound(clickAudio_GoBack);

        //回到登录页
        //显示登录面板
        UI_Manager.Instance.ShowPanel<LoginPanel>();
        //隐藏自己
        UI_Manager.Instance.HidePanel<ServerPanel>();
    }

    private void DoSth_OnClickBtnEnterGame()
    {
        UI_Manager.Instance.PlayBtnSound(clickAudio);

        /*进入游戏*/

        //在进入游戏前，保存登录的数据（选择的服务器）
        LoginAndRegisterMgr.Instance.SaveLoginData();

        //隐藏背景图
        UI_Manager.Instance.HidePanel<LoginBackgroundPanel>(false);
        //隐藏自己
        UI_Manager.Instance.HidePanel<ServerPanel>(false);

        //切换至游戏场景
        SceneManager.LoadScene("GameScene");
    }

    private void DoSth_OnClickBtnChange()
    {
        UI_Manager.Instance.PlayBtnSound(clickAudio);

        //显示换服面板
        UI_Manager.Instance.ShowPanel<SelectServerPanel>();
        //隐藏自己
        UI_Manager.Instance.HidePanel<ServerPanel>();
    }

    public override void ShowMe()
    {
        base.ShowMe();

        //显示自己时，更新面板上当前选择的区服的名字
        int id = LoginAndRegisterMgr.Instance.LoginData.frontServerID;
        if(id<=0)
        {
            txtName.text = "无选择";
        }
        else
        {
            ServerInfo serverInfo = LoginAndRegisterMgr.Instance.ServerData[id-1];
            txtName.text = serverInfo.id + "区 " + serverInfo.name;
        }
    }
}
