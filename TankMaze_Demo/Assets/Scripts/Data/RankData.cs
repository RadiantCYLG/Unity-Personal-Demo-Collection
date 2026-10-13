using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 排行榜单条数据
/// </summary>
public class RankData 
{
    public string name;
    public float score;
    public float time; //通关时间

    public RankData()
    {

    }

    public RankData(string name, float score, float time)
    {
        this.name = name;
        this.score = score;
        this.time = time;
    }
}

/// <summary>
/// 排行榜列表
/// </summary>
public class RankList
{
    public List<RankData> dataList;
}
