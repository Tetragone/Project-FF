using System;
using System.Collections.Generic;
using UnityEngine;

public class UserDataMgr : SingletonAllTime<UserDataMgr>
{
    public SecureInt Gold = 0;
    public SecureInt GachaPoint = 0;
    public SecureInt RelicPoint = 0;

    #region Local Data Save
    public UserLocalData SaveData()
    {
        UserLocalData data = new UserLocalData();
        data.Gold = Gold.GetBase();
        data.GachaPoint = GachaPoint.GetBase();
        data.RelicPoint = RelicPoint.GetBase();
        UserLocalData.SaveData(data);
        return data;
    }

    public void LoadData()
    {
        UserLocalData data = UserLocalData.LoadData();
        Gold = new SecureInt(data.Gold);
        GachaPoint = new SecureInt(data.GachaPoint);
        RelicPoint = new SecureInt(data.RelicPoint);
    }
    #endregion
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
        PlayerPrefs.SetString(GoldKey, data.Gold);
        PlayerPrefs.SetString(GachaKey, data.GachaPoint);
        PlayerPrefs.SetString(RelicKey, data.RelicPoint);
        PlayerPrefs.Save();
    }

    public static UserLocalData LoadData()
    {
        UserLocalData data = new UserLocalData();
        data.Gold = PlayerPrefs.GetString(GoldKey, "");
        data.GachaPoint = PlayerPrefs.GetString(GachaKey, "");
        data.RelicPoint = PlayerPrefs.GetString(RelicKey, "");
        return data;
    }
}