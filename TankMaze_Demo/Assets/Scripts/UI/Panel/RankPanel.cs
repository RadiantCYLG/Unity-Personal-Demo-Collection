using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : BasePanel
{
    [SerializeField] private Button btnExit;
    private List<Text> txtRankList = new List<Text>();
    private List<Text> txtPlayerNameList = new List<Text>();
    private List<Text> txtScoreList = new List<Text>();
    private List<Text> txtTimeList = new List<Text>(); //通关时间

    private void Start()
    {
        btnExit.onClick.AddListener(() =>
        {
            UI_Manager.Instance.ShowPanel<BeginPanel>();
            UI_Manager.Instance.HideAndRemovePanel<RankPanel>();
        });

        for (int i = 1; i <= 10; i++)
        {
            //！！transform.Find只能找到直接的子对象，不能找到子对象的子对象，需要通过加 斜杠/ 来找到
            txtRankList.Add(transform.Find("Rank/TxtRank" + i).GetComponent<Text>());
            txtPlayerNameList.Add(transform.Find("PlayerName/TxtPlayerName" + i).GetComponent<Text>());
            txtScoreList.Add(transform.Find("Score/TxtScore" + i).GetComponent<Text>());
            txtTimeList.Add(transform.Find("Time/TxtTime" + i).GetComponent<Text>());
        }

        //PlayerPrefs.DeleteAll(); //清空所有已经保存的数据
        //GameDataMgr.Instance.AddRankData("测试数据", 99, 5832);
        UpdatePanelInfo();
    }

    private void UpdatePanelInfo()
    {
        List<RankData> list = GameDataMgr.Instance.rankInfo.dataList;
        if (list == null) return;
        //！！！只按照 UI 行数或数据条数中的较小者循环，避免越界
        int displayCount = Mathf.Min(list.Count, txtPlayerNameList.Count);

        for (int i = 0; i < displayCount; i++)
        {
            txtPlayerNameList[i].text = list[i].name;
            txtScoreList[i].text = list[i].score.ToString();
            //时间存储单位是 秒
            //把秒数转换为 时 分 秒
            int time = (int)list[i].time;
            txtTimeList[i].text = ""; //先清空通关时间上的文字
            if (time / 3600 > 0)
            {
                txtTimeList[i].text += time / 3600 + "时";
            }
            if (time % 3600 / 60 > 0 || txtTimeList[i].text != "")//不足一小时的除以分钟, 0分钟也记录上去
            {
                txtTimeList[i].text += time % 3600 / 60 + "分";
            }
            txtTimeList[i].text += time % 60 + "秒";
        }

        // 可选：清空多余 UI 行（若你希望显示固定 10 行且未填满时显示空白）
        //for (int i = displayCount; i < txtPlayerNameList.Count; i++)
        //{
        //    txtRankList[i].text = "";
        //    txtPlayerNameList[i].text = "";
        //    txtScoreList[i].text = "";
        //    txtTimeList[i].text = "";
        //}
    }
}
