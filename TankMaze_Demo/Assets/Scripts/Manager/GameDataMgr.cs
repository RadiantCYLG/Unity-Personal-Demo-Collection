using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    //音乐音效数据
    public MusicData musicData { get; private set; }
    //排行榜数据
    public RankList rankInfo { get; private set; }

    private GameDataMgr()
    {
        musicData = PlayerPrefsDataMgr.Instance.LoadData("MusicAndSound", typeof(MusicData)) as MusicData;
        //如果第一次进入游戏，是没有音乐音效数据的，需要设置
        if (!musicData.notFirstLoad)
        {
            musicData.notFirstLoad = true;//！！！改变标识

            musicData.isOpenMusic = true;
            musicData.isOpenSound = true;
            musicData.musicValue = 1;
            musicData.soundValue = 1;
            PlayerPrefsDataMgr.Instance.SaveData("MusicAndSound", musicData);
        }
        //加载排行榜数据
        rankInfo = PlayerPrefsDataMgr.Instance.LoadData("RankDate", typeof(RankList)) as RankList;
        CheckRankInfo();


    }

    /// <summary>
    ///--- 稳健性修复： ---检测历史保存的数据 确保 rankDataInfo 与 dataList 不为空，并且裁剪超过 10 条的数据
    /// </summary>
    private void CheckRankInfo()
    {
        if (rankInfo == null)
        {
            rankInfo = new RankList { dataList = new List<RankData>() };
            PlayerPrefsDataMgr.Instance.SaveData("RankDate", rankInfo);
        }
        else if (rankInfo.dataList == null)
        {
            rankInfo.dataList = new List<RankData>();
            PlayerPrefsDataMgr.Instance.SaveData("RankDate", rankInfo);
        }
        else if (rankInfo.dataList.Count > 10)
        {
            // 保证保存的数据不超过 10 条
            while (rankInfo.dataList.Count > 10)
            {
                rankInfo.dataList.RemoveAt(rankInfo.dataList.Count - 1);
            }
            PlayerPrefsDataMgr.Instance.SaveData("RankDate", rankInfo);
        }
    }

    #region 提供给外部改变排行榜数据的方法
    /// <summary>
    /// 在排行榜中添加数据
    /// </summary>
    public void AddRankData(string name, float score, float time)
    {
        rankInfo.dataList.Add(new RankData(name, score, time));
        //排序，用时少的排在前
        rankInfo.dataList.Sort((a, b) => a.time < b.time ? -1 : 1);
        //排完序后，超出10条的移除
        for (int i = rankInfo.dataList.Count - 1; i >= 10; i--)
        {
            rankInfo.dataList.RemoveAt(i);
        }
        PlayerPrefsDataMgr.Instance.SaveData("RankDate", rankInfo);
    }
    #endregion

    #region 提供给外部控制音乐音效的方法
    /// <summary>
    /// 开关音乐
    /// </summary>
    public void SwitchMusic(bool isOpen)
    {
        musicData.isOpenMusic = isOpen;
        BGM.Instance.SwitchAudio(isOpen);
        PlayerPrefsDataMgr.Instance.SaveData("MusicAndSound", musicData);
    }

    /// <summary>
    /// 开关音效
    /// </summary>
    public void SwitchSound(bool isOpen)
    {
        musicData.isOpenSound = isOpen;
        PlayerPrefsDataMgr.Instance.SaveData("MusicAndSound", musicData);
    }

    /// <summary>
    /// 改变音乐大小
    /// </summary>
    public void ChangeMusicValue(float value)
    {
        musicData.musicValue = value;
        BGM.Instance.ChangeValue(value);
        PlayerPrefsDataMgr.Instance.SaveData("MusicAndSound", musicData);
    }

    /// <summary>
    /// 改变音效大小
    /// </summary>
    public void ChangeSoundValue(float value)
    {
        musicData.soundValue = value;
        PlayerPrefsDataMgr.Instance.SaveData("MusicAndSound", musicData);
    }
    #endregion
}
