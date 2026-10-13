using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//登录和注册管理器
public class LoginAndRegisterMgr
{
    //设置自身为单例模式
    private static LoginAndRegisterMgr instance = new LoginAndRegisterMgr();
    public static LoginAndRegisterMgr Instance => instance; //相当于{get{return instance;}}

    //登录数据
    private LoginData loginData;
    public LoginData LoginData => loginData;
    
    //注册数据
    private RegisterData registerData;
    public RegisterData RegisterData => registerData;

    //所有服务器数据
    private List<ServerInfo> serverData;
    public List<ServerInfo> ServerData => serverData;

    //构造函数，初始化(或读取)数据
    public LoginAndRegisterMgr()
    {
        //通过Json管理器，来读取对应数据
        //读取登录数据
        loginData = JsonMgr.Instance.LoadData<LoginData>("LoginData");
        //读取注册数据
        registerData = JsonMgr.Instance.LoadData<RegisterData>("RegisterData");
        //读取服务器数据
        serverData = JsonMgr.Instance.LoadData<List<ServerInfo>>("ServerInfo");
    }


    /// <summary>
    /// 保存登录数据
    /// </summary>
    public void SaveLoginData()
    {
        JsonMgr.Instance.SaveData(loginData, "LoginData");
    }

    /// <summary>
    /// 主要用于注册成功后，清除之前账号登陆时在LoginDate中存的数据，
    /// 让新注册的账号不受旧账号登录数据的影响
    /// </summary>
    public void ClearLoginData()
    {
        loginData.frontServerID = -1;
        loginData.isRemeberPW = false;
        loginData.isAutoLogin = false;
    }

    /// <summary>
    /// 保存注册数据
    /// </summary>
    public void SaveRegisterDate()
    {
        JsonMgr.Instance.SaveData(registerData, "RegisterData");
    }

    /// <summary>
    /// 注册方法
    /// </summary>
    /// <returns>是否注册成功</returns>
    public bool RegisterUserInfo(string name, string password)
    {
        //已存在当前用户数据
        if (registerData.registerInfo.ContainsKey(name))
        {
            return false;
        }

        registerData.registerInfo.Add(name, password);
        SaveLoginData();
        return true;
    }

    /// <summary>
    /// 验证登录信息是否正确
    /// </summary>
    /// <returns></returns>
    public bool CheckLoginInfo(string name, string password)
    {
        //存在当前用户
        if (registerData.registerInfo.ContainsKey(name))
        {
            RegisterData.isFindUser = true;
            //判断密码是否相同
            if (registerData.registerInfo[name] == password)
            {
                return true;
            }
        }
        else
        {
            //用户未注册
            RegisterData.isFindUser= false;
            return false;
        }

        RegisterData.isFindUser = true;
        //用户名或密码错误，请重新登录
        return false;
    }
}
