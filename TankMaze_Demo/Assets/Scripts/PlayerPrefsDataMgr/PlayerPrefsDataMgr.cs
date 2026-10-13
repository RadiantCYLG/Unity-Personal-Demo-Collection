using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// PlayerPrefs数据管理类
/// </summary>
public class PlayerPrefsDataMgr
{
    private static PlayerPrefsDataMgr instance = new PlayerPrefsDataMgr();
    public static PlayerPrefsDataMgr Instance
    {
        get
        {
            return instance;
        }
    }

    public void SaveData(string keyName, object data)
    {
        //得到类型
        Type dataType = data.GetType();
        //得到该类型的所有成员
        FieldInfo[] infos = dataType.GetFields();
        string saveKeyName = "";
        //得到单个成员
        FieldInfo info;
        for (int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            //键名规则：keyName+类型名+字段类型+字段名
            saveKeyName = keyName + "_" + dataType.Name + "_" + info.FieldType.Name + "_" + info.Name;

            //info.GetValue(date)，是得到info的值，并存入date对象中
            SaveValue(saveKeyName, info.GetValue(data)); 
        }
        PlayerPrefs.Save();
    }

    private void SaveValue(string keyName, object value)
    {
        Type fieldType = value.GetType();
        //PlayerPrefs只会存储3种类型int、string、float
        //判断传入的value的具体类型并转换、

        //value是int型
        if (fieldType == typeof(int))
        {
            PlayerPrefs.SetInt(keyName, (int)value);
        }
        //value是float型
        else if (fieldType == typeof(float))
        {
            PlayerPrefs.SetFloat(keyName, (float)value);
        }
        //value是string型
        else if (fieldType == typeof(string))
        {
            PlayerPrefs.SetString(keyName, value.ToString());
        }
        //value是bool型
        else if (fieldType == typeof(bool))
        {
            PlayerPrefs.SetInt(keyName, (bool)value ? 1 : 0);
        }
        //value是List<T>类型
        //反射的方法IsAssignableFrom(Type c)
        //传入的参数可以为函数调用方分配空间，相当于判断传入参数是否继承了IList
        else if (typeof(IList).IsAssignableFrom(fieldType))
        {
            //IList型的list作为基类容器
            IList list = value as IList;
            PlayerPrefs.SetInt(keyName, list.Count); 
            int index = 0;
            foreach (object listValue in list)
            {
                //设置泛型List列表中每个元素，对列表中的元素加上序号确保唯一键值，再做一次上述判断
                SaveValue(keyName + index, listValue);
                ++index;
            }
        }
        //value是Dictionary类型
        else if (typeof(IDictionary).IsAssignableFrom(fieldType))
        {
            IDictionary dic = value as IDictionary;
            PlayerPrefs.SetInt(keyName, dic.Count); //设置字典的键数
            int index = 0;
            foreach (object key in dic.Keys)
            {
                SaveValue(keyName + "_key_" + index, key); //设置键
                SaveValue(keyName + "_value_" + index, dic[key]); //设置值
                ++index;
            }
        }
        //value是自定义类型
        else
        {
            //自定义类型对象相当于多了一层到多层的类型包裹起来
            //用递归函数，重复几轮进行处理
            SaveData(keyName, value);
        }
    }

    public object LoadData(string keyName, Type type)
    {
        //快速实例
        object data = Activator.CreateInstance(type);

        FieldInfo[] infos = type.GetFields();
        string loadKeyName = "";
        FieldInfo info;
        for (int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            //与存储时对应的建名
            loadKeyName = keyName + "_" + type.Name + "_" + info.FieldType.Name + "_" + info.Name;
            //反射中的方法SetValue————设置对象的date的值
            //info.FieldType得到类型中成员的类型，加载对应成员的值，载入date
            info.SetValue(data, LoadValue(loadKeyName, info.FieldType));
        }
        return data;
    }

    private object LoadValue(string keyName, Type fieldType)
    {
        if (fieldType == typeof(int))
        {
            return PlayerPrefs.GetInt(keyName, 0); //默认值为0，有值就不使用默认值
        }
        else if (fieldType == typeof(float))
        {
            return PlayerPrefs.GetFloat(keyName, 0);
        }
        else if (fieldType == typeof(string))
        {
            return PlayerPrefs.GetString(keyName, "");
        }
        else if (fieldType == typeof(bool))
        {
            return PlayerPrefs.GetInt(keyName, 0) == 1 ? true : false;
        }

        else if (typeof(IList).IsAssignableFrom(fieldType))
        {
            //得到泛型List长度
            int count = PlayerPrefs.GetInt(keyName, 0);
            IList list = Activator.CreateInstance(fieldType) as IList;
            for (int i = 0; i < count; i++)
            {
                //通过加载存储的列表数据，往列表中添加
                //GetGenericArguments() 得到泛型List<T>中的参数T的类型Type
                //对列表中的元素再做一次上述判断
                list.Add(LoadValue(keyName + i, fieldType.GetGenericArguments()[0]));//得到第0个参数的Type
            }
            return list;
        }
        else if (typeof(IDictionary).IsAssignableFrom(fieldType))
        {
            int count = PlayerPrefs.GetInt(keyName, 0);
            IDictionary dic = Activator.CreateInstance(fieldType) as IDictionary;
            //得到字典的键和值类型
            Type[] kyType = fieldType.GetGenericArguments();
            for (int i = 0; i < count; i++)
            {
                dic.Add(LoadValue(keyName + "_key_" + i, kyType[0]),
                        LoadValue(keyName + "_value_" + i, kyType[1]));
            }
            return dic;
        }
        //自定义类型
        else
        {
            return LoadData(keyName, fieldType);
        }
        //return null;
    }
}
