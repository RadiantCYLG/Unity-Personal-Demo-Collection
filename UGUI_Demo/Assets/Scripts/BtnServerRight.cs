using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnServerRight : MonoBehaviour
{
    [SerializeField] private Button btnSelf;
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtState; //服务器状态

    private ServerInfo currentServerInfo; //当前按钮代表的服务器

    private AudioClip clickAudio;

    // Start is called before the first frame update
    void Start()
    {
        clickAudio = Resources.Load<AudioClip>("Sounds/Arknights_ButtonClickAudio1");

        btnSelf.onClick.AddListener(() =>
        {
            UI_Manager.Instance.PlayBtnSound(clickAudio);

            //记录选择的服务器数据
            LoginAndRegisterMgr.Instance.LoginData.frontServerID = currentServerInfo.id;
            //显示服务器面板
            UI_Manager.Instance.ShowPanel<ServerPanel>();
            //隐藏选服面板
            UI_Manager.Instance.HidePanel<SelectServerPanel>();
        });
    }

    /// <summary>
    /// 通过外部传值，确定当前按钮代表的服务器
    /// </summary>
    public void InitInfo(ServerInfo serverInfo)
    {
        //记下服务器
        currentServerInfo = serverInfo;
        //更新显示信息
        txtName.text = "   " + currentServerInfo.id + "区   " + currentServerInfo.name;
        //确定服务器的状态
        //初始显示状态
        txtState.gameObject.SetActive(true);
        switch (currentServerInfo.state)
        {
            case 0://不显示状态
                txtState.gameObject.SetActive(false);
                break;
            case 1://维护
                txtState.color = new Color32(157, 157, 157, 255);
                txtState.text = "维护";
                break;
            case 2://流畅
                txtState.color = new Color32(139, 207, 0, 255);
                txtState.text = "流畅";
                break;
            case 3://拥挤
                txtState.color = new Color32(221, 152, 76, 255);
                txtState.text = "拥挤";
                break;
        }
    }
}
