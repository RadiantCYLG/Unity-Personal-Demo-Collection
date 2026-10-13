using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginData 
{
    //账号名
    public string userName;
    //密码
    public string password;

    //是否记住密码
    public bool isRemeberPW;
    //是否自动登录
    public bool isAutoLogin;

    //服务器选择相关
    public int frontServerID = -1; //上一次选择的服务器ID，-1代表没有选择过服务器
}
