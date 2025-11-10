using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UserDataMgr : SingletonAllSecen<UserDataMgr>
{
    public SecureInt Gold { get; private set; } = 0;
    public SecureInt GachaPoint { get; private set; } = 0;
    public SecureInt RelicPoint { get; private set; } = 0;

    public SecureInt Stage { get; private set; } = 0;

    private Dictionary<GoldUpgrade, SecureInt> GoldUpgradeLv = new Dictionary<GoldUpgrade, SecureInt>();
    private Dictionary<string, SecureInt> FishesCount = new Dictionary<string, SecureInt>();
    private Dictionary<string, SecureInt> FishesLv = new Dictionary<string, SecureInt>();
    private Dictionary<string, SecureInt> RelicCount = new Dictionary<string, SecureInt>();
    private Dictionary<string, SecureInt> RelicLv = new Dictionary<string, SecureInt>();

    public void RefreshRelicStat()
    {
        foreach (string key in RelicLv.Keys)
        {
            if (RelicLv[key] <= 0)
            {
                continue;
            }

            string sid = TableMgr.GetTableString("relic", key, "sid");
            string stype = TableMgr.GetTableString("stat", sid, "type");
            StatType type = (StatType)Enum.Parse(typeof(StatType), stype);
            StatMgr.SetStat(type, string.Format("relic_{0}", sid), RelicLv[key]);
        }
    }

    #region Get Dataes
    public int GetGoldUpgrade(GoldUpgrade type)
    {
        if (GoldUpgradeLv.ContainsKey(type))
        {
            return GoldUpgradeLv[type];
        }
        else
        {
            return 0;
        }
    }

    public int GetFishesCount(string id)
    {
        if (FishesCount.ContainsKey(id))
        {
            return FishesCount[id];
        }
        else
        {
            return -1;
        }
    }

    public int GetFishesLv(string id)
    {
        if (FishesLv.ContainsKey(id))
        {
            return FishesLv[id];
        }
        else
        {
            return -1;
        }
    }

    public List<string> GetHasFishes()
    {
        List<string> result = new List<string>();

        foreach (string key in FishesLv.Keys)
        {
            int lv = FishesLv[key];

            if (lv > 0)
            {
                result.Add(key);
            }
        }

        return result;
    }

    public int GetRelicCount(string id)
    {
        if (RelicCount.ContainsKey(id))
        {
            return RelicCount[id];
        }
        else
        {
            return -1;
        }
    }

    public int GetRelicLv(string id)
    {
        if (RelicLv.ContainsKey(id))
        {
            return RelicLv[id];
        }
        else
        {
            return -1;
        }
    }
    #endregion

    #region Set Dataes

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

    public bool AddGoldUpgrade(GoldUpgrade type, int addedLv = 1)
    {
        int beforeLv = 0;

        if (GoldUpgradeLv.ContainsKey(type))
        {
            beforeLv = GoldUpgradeLv[type];
            GoldUpgradeLv.Remove(type);
        }

        GoldUpgradeLv.Add(type, beforeLv + addedLv);
        return true;
    }

    public bool AddFishesCount(string id, int addedLv = 1)
    {
        if (!TableMgr.IsValidKey("fish", id))
        {
            return false;
        }

        int beforeLv = 0;

        if (FishesCount.ContainsKey(id))
        {
            beforeLv = FishesCount[id];
            FishesCount.Remove(id);
        }

        FishesCount.Add(id, beforeLv + addedLv);
        return true;
    }

    public bool AddFishesLv(string id, int addedLv = 1)
    {
        if (!TableMgr.IsValidKey("fish", id))
        {
            return false;
        }

        int beforeLv = 0;

        if (FishesLv.ContainsKey(id))
        {
            beforeLv = FishesLv[id];
            FishesLv.Remove(id);
        }

        FishesLv.Add(id, beforeLv + addedLv);
        return true;
    }

    public bool AddRelicCount(string id, int addedLv = 1)
    {
        if (!TableMgr.IsValidKey("relic", id))
        {
            return false;
        }

        int beforeLv = 0;

        if (RelicCount.ContainsKey(id))
        {
            beforeLv = RelicCount[id];
            RelicCount.Remove(id);
        }

        RelicCount.Add(id, beforeLv + addedLv);
        return true;
    }

    public bool AddRelicLv(string id, int addedLv = 1)
    {
        if (!TableMgr.IsValidKey("relic", id))
        {
            return false;
        }

        int beforeLv = 0;

        if (RelicLv.ContainsKey(id))
        {
            beforeLv = RelicLv[id];
            RelicLv.Remove(id);
        }

        RelicLv.Add(id, beforeLv + addedLv);
        RefreshRelicStat();
        return true;
    }
    #endregion

    #region Local Data Save
    public void SaveData()
    {
        UserLocalData userData = new UserLocalData();
        userData.Gold = Gold.GetBase();
        userData.GachaPoint = GachaPoint.GetBase();
        userData.RelicPoint = RelicPoint.GetBase();
        userData.Stage = Stage.GetBase();
        UserLocalData.SaveData(userData);

        UpgradeLocalData upgradeData = new UpgradeLocalData();

        foreach (var key in GoldUpgradeLv.Keys)
        {
            string lvBase = GoldUpgradeLv[key].GetBase();
            upgradeData.GoldUpgrade.Add(((int)key).ToString(), lvBase);
        }

        var fishTable = TableMgr.GetTable("fish");

        foreach (string key in fishTable.Keys)
        {
            string countBase = FishesCount.ContainsKey(key) ? FishesCount[key].GetBase() : "";
            string lvBase = FishesLv.ContainsKey(key) ? FishesLv[key].GetBase() : "";
            upgradeData.FishCount.Add(key, countBase);
            upgradeData.FishLv.Add(key, lvBase);
        }

        var relicTable = TableMgr.GetTable("relic");

        foreach (string key in relicTable.Keys)
        {
            string countBase = RelicCount.ContainsKey(key) ? RelicCount[key].GetBase() : "";
            string lvBase = RelicLv.ContainsKey(key) ? RelicLv[key].GetBase() : "";
            upgradeData.RelicCount.Add(key, countBase);
            upgradeData.RelicLv.Add(key, lvBase);
        }

        UpgradeLocalData.SaveData(upgradeData);
    }

    public void LoadData()
    {
        UserLocalData userData = UserLocalData.LoadData();
        Gold = new SecureInt(userData.Gold);
        GachaPoint = new SecureInt(userData.GachaPoint);
        RelicPoint = new SecureInt(userData.RelicPoint);
        Stage = new SecureInt(userData.Stage);

        UpgradeLocalData upgradeData = UpgradeLocalData.LoadData();
        GoldUpgradeLv.Clear();

        foreach (string key in upgradeData.GoldUpgrade.Keys)
        {
            GoldUpgradeLv.Add((GoldUpgrade)int.Parse(key), new SecureInt(upgradeData.GoldUpgrade[key]));
        }

        FishesCount.Clear();
        FishesLv.Clear();

        foreach (string key in upgradeData.FishCount.Keys)
        {
            FishesCount.Add(key, new SecureInt(upgradeData.FishCount[key]));
            FishesLv.Add(key, new SecureInt(upgradeData.FishLv[key]));
        }

        if (FishesLv[GameStaticValue.BaseFishFid] == 0)
        {
            FishesLv[GameStaticValue.BaseFishFid] = 1;
        }

        if (FishesCount[GameStaticValue.BaseFishFid] <= GameStaticValue.GetNeedFishLvUpCount(1))
        {
            FishesCount[GameStaticValue.BaseFishFid] = GameStaticValue.GetNeedFishLvUpCount(1);
        }

        RelicCount.Clear();
        RelicLv.Clear();

        foreach (string key in upgradeData.RelicCount.Keys)
        {
            RelicCount.Add(key, new SecureInt(upgradeData.RelicCount[key]));
            RelicLv.Add(key, new SecureInt(upgradeData.RelicLv[key]));
        }

        RefreshRelicStat();
    }
    #endregion

    #region Server Data Save 
    public void SaveDataOnServer()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("gold", Gold.ToString());
        data.Add("gacha_point", GachaPoint.ToString());
        data.Add("relic_point", RelicPoint.ToString());
        data.Add("stage", Stage.ToString());

        InsertDataFromDic(data, "gold_lv", GoldUpgradeLv);
        InsertDataFromDic(data, "fish_lv", FishesLv);
        InsertDataFromDic(data, "fish_count", FishesCount);
        InsertDataFromDic(data, "relic_lv", RelicLv);
        InsertDataFromDic(data, "relic_count", RelicCount);

        FireCloudFunction.Instance.CallHttps("user_save", data, (result =>
        {
            SaveData();
        }), () => SaveData());
    }

    private void InsertDataFromDic<TKey, TValue>(Dictionary<string, object> data, string key, Dictionary<TKey, TValue> input)
    {
        Dictionary<string, object> changeToInput = new Dictionary<string, object>();
        foreach (TKey iKey in input.Keys)
        {
            string siKey = iKey.ToString();
            changeToInput.Add(siKey, input[iKey]);
        }
        data.Add(key, changeToInput);
    }

    public void LoadDataOnServer(UnityAction action = null)
    {
        FireCloudFunction.Instance.CallHttps("user_load", new Dictionary<string, object>(), (data =>
        {
            if (data == null)
            {
                Debug.LogError("data loading is error");
                return;
            }

            Dictionary<object, object> user = data["user"] as Dictionary<object, object>;
            foreach (string key in user.Keys)
            {
                switch (key)
                {
                    case "gold":
                        Gold = new SecureInt(int.Parse(user["gold"].ToString()));
                        break;
                    case "gacha_point":
                        GachaPoint = new SecureInt(int.Parse(user["gacha_point"].ToString()));
                        break;
                    case "relic_point":
                        RelicPoint = new SecureInt(int.Parse(user["relic_point"].ToString()));
                        break;
                    case "stage":
                        Stage = new SecureInt(int.Parse(user["stage"].ToString()));
                        break;
                    case "gold_lv":
                        {
                            var goldLv = user["gold_lv"] as Dictionary<object, object>;
                            GoldUpgradeLv.Clear();
                            foreach (string gkey in goldLv.Keys)
                            {
                                int value = int.Parse(goldLv[gkey].ToString());
                                GoldUpgrade upgrade = (GoldUpgrade)Enum.Parse(typeof(GoldUpgrade), gkey);
                                GoldUpgradeLv.Add(upgrade, value);
                            }
                        }
                        break;
                    case "fish_lv":
                        SetDicFromData(FishesLv, user["fish_lv"] as Dictionary<object, object>);
                        break;
                    case "fish_count":
                        SetDicFromData(FishesCount, user["fish_count"] as Dictionary<object, object>);
                        break;
                    case "relic_lv":
                        SetDicFromData(RelicLv, user["relic_lv"] as Dictionary<object, object>);
                        break;
                    case "relic_count":
                        SetDicFromData(RelicCount, user["relic_count"] as Dictionary<object, object>);
                        break;
                }
            }

            if (action != null)
            {
                action.Invoke();
            }
        }));
    }

    private void SetDicFromData(Dictionary<string, SecureInt> dic, Dictionary<object, object> data)
    {
        if (data == null)
        {
            return;
        }

        dic.Clear(); 
        foreach (string key in data.Keys)
        {
            int value = int.Parse(data[key].ToString());
            dic.Add(key, value);
        }
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


public enum GoldUpgrade
{
    food_grow, grow_time, engery_effect,
}

public class UpgradeLocalData
{
    public Dictionary<string, string> GoldUpgrade = new Dictionary<string, string>();
    private static string GoldUpgradeKey = "guk_{0}";
    public Dictionary<string, string> FishCount = new Dictionary<string, string>();
    private static string FishCountKey = "fck_{0}";
    public Dictionary<string, string> FishLv = new Dictionary<string, string>();
    private static string FishLvKey = "flk_{0}";
    public Dictionary<string, string> RelicCount = new Dictionary<string, string>();
    private static string RelicCountKey = "rck_{0}";
    public Dictionary<string, string> RelicLv = new Dictionary<string, string>();
    private static string RelicLvKey = "rlk_{0}";

    public static void SaveData(UpgradeLocalData data)
    {
        foreach (string key in data.GoldUpgrade.Keys)
        {
            PlayerPrefs.SetString(string.Format(GoldUpgradeKey, key), data.GoldUpgrade[key]);
        }

        foreach (string key in data.FishCount.Keys)
        {
            PlayerPrefs.SetString(string.Format(FishCountKey, key), data.FishCount[key]);
            PlayerPrefs.SetString(string.Format(FishLvKey, key), data.FishLv[key]);
        }

        foreach (string key in data.RelicCount.Keys)
        {
            PlayerPrefs.SetString(string.Format(RelicCountKey, key), data.RelicCount[key]);
            PlayerPrefs.SetString(string.Format(RelicLvKey, key), data.RelicLv[key]);
        }

        PlayerPrefs.Save();
    }

    public static UpgradeLocalData LoadData()
    {
        UpgradeLocalData data = new UpgradeLocalData();

        Dictionary<string, string> goldUp = new Dictionary<string, string>();

        foreach (GoodsType type in Enum.GetValues(typeof(GoodsType)))
        {
            goldUp.Add(((int)type).ToString(), PlayerPrefs.GetString(string.Format(GoldUpgradeKey, (int)type), ""));
        }

        data.GoldUpgrade = goldUp;

        var fishTable = TableMgr.GetTable("fish");

        Dictionary<string, string> fishCount = new Dictionary<string, string>();
        Dictionary<string, string> fishLv = new Dictionary<string, string>();

        foreach (string key in fishTable.Keys)
        {
            fishCount.Add(key, PlayerPrefs.GetString(string.Format(FishCountKey, key), ""));
            fishLv.Add(key, PlayerPrefs.GetString(string.Format(FishLvKey, key), ""));
        }

        data.FishCount = fishCount;
        data.FishLv = fishLv;

        var relicTable = TableMgr.GetTable("relic");

        Dictionary<string, string> relicCount = new Dictionary<string, string>();
        Dictionary<string, string> relicLv = new Dictionary<string, string>();

        foreach (string key in relicTable.Keys)
        {
            relicCount.Add(key, PlayerPrefs.GetString(string.Format(RelicCountKey, key), ""));
            relicLv.Add(key, PlayerPrefs.GetString(string.Format(RelicLvKey, key), ""));
        }

        data.RelicCount = relicCount;
        data.RelicLv = relicLv;

        return data;
    }
}