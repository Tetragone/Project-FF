using System;
using System.Collections.Generic;
using UnityEngine;

public class UserDataMgr : SingletonAllTime<UserDataMgr>
{
    // 걍 ToString하고 특정 각 배열에 특정 숫자만큼 더한 값을 저장하면 되겠는디 숩게 합시다.
    public float Gold;
    public float GachaPoint;
    public float RelicPoint;
}

public class UserLocalData
{
    public string Gold;
    private static string GoldKey = "gk";
    public string GachaPoint;
    private static string GachaKey = "gak";
    public string RelicPoint;
    private static string RelicKey = "rk";

    public static void SaveData(UserLocalData data)
    {

    }

    public static UserLocalData LoadData()
    {
        UserLocalData data = new UserLocalData();
        return data;
    }
}