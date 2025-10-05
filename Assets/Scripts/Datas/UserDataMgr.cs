using System;
using System.Collections.Generic;
using UnityEngine;

public class UserDataMgr : SingletonAllSecen<UserDataMgr>
{
    public SecureInt Gold { get; private set; } = 0;
    public SecureInt GachaPoint { get; private set; } = 0;
    public SecureInt RelicPoint { get; private set; } = 0;

    public SecureInt Stage { get; private set; } = 0;

    // 늘어날때만 사용
    public void AddGoods(int value, GoodsType type)
    {
        if (value < 0)
        {
            return;
        }

        switch (type)
        {
            case GoodsType.gold:
                Gold += value;
                break;
            case GoodsType.gacha_point:
                GachaPoint += value;
                break;
            case GoodsType.relic_point:
                RelicPoint += value;
                break;
        }

        this.SaveData();
    }

    public void UseGoods(int value, GoodsType type)
    {
        if (value < 0)
        {
            return;
        }

        if (!IsEnoughGoods(value, type))
        {
            return;
        }

        switch (type)
        {
            case GoodsType.gold:
                Gold -= value;
                break;
            case GoodsType.gacha_point:
                GachaPoint -= value;
                break;
            case GoodsType.relic_point:
                RelicPoint -= value;
                break;
        }
    }

    public bool IsEnoughGoods(int value, GoodsType type)
    {
        int nowValue = 0;

        switch (type)
        {
            case GoodsType.gold:
                nowValue = Gold;
                break;
            case GoodsType.gacha_point:
                nowValue = GachaPoint;
                break;
            case GoodsType.relic_point:
                nowValue = RelicPoint;
                break;
        }

        return nowValue >= value;
    }

    public void StageClear()
    {
        Stage++;
    }

    #region Local Data Save
    public UserLocalData SaveData()
    {
        UserLocalData data = new UserLocalData();
        data.Gold = Gold.GetBase();
        data.GachaPoint = GachaPoint.GetBase();
        data.RelicPoint = RelicPoint.GetBase();
        data.Stage = Stage.GetBase();
        UserLocalData.SaveData(data);
        return data;
    }

    public void LoadData()
    {
        UserLocalData data = UserLocalData.LoadData();
        Gold = new SecureInt(data.Gold);
        GachaPoint = new SecureInt(data.GachaPoint);
        RelicPoint = new SecureInt(data.RelicPoint);
        Stage = new SecureInt(data.Stage);
    }
    #endregion
}

public enum GoodsType
{
    gold, gacha_point, relic_point
}

public class UserLocalData
{
    public string Gold;
    private static string GoldKey = "gk";
    public string GachaPoint;
    private static string GachaKey = "gak";
    public string RelicPoint;
    private static string RelicKey = "rk";
    public string Stage;
    private static string StageKey = "st";

    public static void SaveData(UserLocalData data)
    {
        PlayerPrefs.SetString(GoldKey, data.Gold);
        PlayerPrefs.SetString(GachaKey, data.GachaPoint);
        PlayerPrefs.SetString(RelicKey, data.RelicPoint);
        PlayerPrefs.SetString(StageKey, data.Stage);
        PlayerPrefs.Save();
    }

    public static UserLocalData LoadData()
    {
        UserLocalData data = new UserLocalData();
        data.Gold = PlayerPrefs.GetString(GoldKey, "");
        data.GachaPoint = PlayerPrefs.GetString(GachaKey, "");
        data.RelicPoint = PlayerPrefs.GetString(RelicKey, "");
        data.Stage = PlayerPrefs.GetString(StageKey, "");
        return data;
    }
}