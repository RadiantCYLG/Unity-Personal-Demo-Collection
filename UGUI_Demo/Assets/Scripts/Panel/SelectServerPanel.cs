using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//选择服务器面板
public class SelectServerPanel : BasePanel
{
    //返回按钮
    [SerializeField] private Button btnGoBack;
    //左右滚动视图
    [SerializeField] private ScrollRect sv_Left;
    [SerializeField] private ScrollRect sv_Right;

    //上次登录的信息
    [SerializeField] private Text txtTitleServer;
    [SerializeField] private Text txtState;

    //服务器范围文字
    [SerializeField] private Text txtRange;

    //存储右侧的服务器按钮
    private List<GameObject> btnServerRightList = new List<GameObject>();

    protected override void Init()
    {
        btnGoBack.onClick.AddListener(() =>
        {
            UI_Manager.Instance.PlayBtnSound(clickAudio_GoBack);

            UI_Manager.Instance.ShowPanel<ServerPanel>();
            UI_Manager.Instance.HidePanel<SelectServerPanel>();
        });

        //动态创建左侧的按钮
        //先得到服务器数据
        List<ServerInfo> infoList = LoginAndRegisterMgr.Instance.ServerData;
        //计算需要的按钮数量,每5个服务器编为一个按钮
        int btnNum = infoList.Count / 5 + 1;
        //创建按钮
        for (int i = 0; i < btnNum; i++)
        {
            GameObject item = GameObject.Instantiate(Resources.Load<GameObject>("UI/BtnSelectServer_Left"));
            item.transform.SetParent(sv_Left.content, false);

            BtnServerLeft btnServerLeft = item.GetComponent<BtnServerLeft>();
            //计算按钮上开始的区，结尾的区
            int beginIndex = 5 * i + 1;
            int endIndex = 5 * (i + 1);
            //判断结尾的区是否超过服务器总数
            if (endIndex > infoList.Count)
            {
                endIndex = infoList.Count;
            }
            //设置开始和结尾的区
            btnServerLeft.InitInfo(beginIndex, endIndex);
        }
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //显示上次选择的服务器
        int frontServerID = LoginAndRegisterMgr.Instance.LoginData.frontServerID;
        if (frontServerID <= 0)
        {
            //上次没有选择服务器
            txtTitleServer.text = "无";
            txtTitleServer.alignment = TextAnchor.MiddleCenter; //让文字垂直居中+水平居中
            txtState.gameObject.SetActive(false);
        }
        else
        {
            //上次选择过服务器，则显示上次选择的服务器
            ServerInfo serverInfo = LoginAndRegisterMgr.Instance.ServerData[frontServerID - 1];
            txtTitleServer.text = serverInfo.id + "区 " + serverInfo.name;
            //确定服务器的状态
            //初始显示状态
            txtState.gameObject.SetActive(true);
            switch (serverInfo.state)
            {
                case 0://不显示状态
                    txtState.gameObject.SetActive(false);
                    break;
                case 1://维护
                    txtState.color = new Color32(157,157, 157, 255);
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

        //服务器数的总数
        int serverDateMax = LoginAndRegisterMgr.Instance.ServerData.Count;
        //根据左侧按钮选择，更新右侧的服务器显示
        UpdateServerTheRightShow(1, 5 > serverDateMax ? serverDateMax : 5);
    }

    /// <summary>
    /// 更新面板右侧服务器按钮的显示
    /// </summary>
    public void UpdateServerTheRightShow(int beginIndex, int endIndex)
    {
        //更新上方文字——服务器范围的显示
        txtRange.text = "服务器 " + beginIndex + " - " + endIndex;
        //删除上一轮显示的服务器按钮
        for (int i = 0; i < btnServerRightList.Count; i++)
        {
            Destroy(btnServerRightList[i]);
        }
        btnServerRightList.Clear();//清空列表
        //跟新服务器按钮
        for (int i = beginIndex; i <= endIndex; i++)
        {
            //获取服务器信息
            ServerInfo serverInfo = LoginAndRegisterMgr.Instance.ServerData[i - 1];
            //创建服务器Button实例
            GameObject item = GameObject.Instantiate(Resources.Load<GameObject>("UI/BtnSelectServer_Right"));
            item.transform.SetParent(sv_Right.content, false);
            //获取Button实例上的脚步，初始化每个Button实例信息
            BtnServerRight btnServerRight = item.GetComponent<BtnServerRight>();
            btnServerRight.InitInfo(serverInfo);

            //记录到列表中
            btnServerRightList.Add(item);
        }
    }
}
