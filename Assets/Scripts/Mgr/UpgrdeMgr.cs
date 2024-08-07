using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgrdeMgr : Singleton<UpgrdeMgr>
{
    private Dictionary<string, SecureInt> FishesCount = new Dictionary<string, SecureInt>();
    private Dictionary<string, SecureInt> FishesLv = new Dictionary<string, SecureInt>();
    private Dictionary<string, SecureInt> RelicCount = new Dictionary<string, SecureInt>();
    private Dictionary<string, SecureInt> RelicLv = new Dictionary<string, SecureInt>();


    #region Local Data Save
    public UpgradeLocalData SaveData()
    {
        UpgradeLocalData data = new UpgradeLocalData();

        var fishTable = TableMgr.GetTable("fish");
        
        foreach (string key in fishTable.Keys)
        {
            string countBase = FishesCount.ContainsKey(key) ? FishesCount[key].GetBase() : "";
            string lvBase = FishesLv.ContainsKey(key) ? FishesLv[key].GetBase() : "";
            data.FishCount.Add(key, countBase);
            data.FishLv.Add(key, lvBase);
        }

        var relicTable = TableMgr.GetTable("relic");

        foreach (string key in relicTable.Keys)
        {
            string countBase = RelicCount.ContainsKey(key) ? RelicCount[key].GetBase() : "";
            string lvBase = RelicLv.ContainsKey(key) ? RelicLv[key].GetBase() : "";
            data.RelicCount.Add(key, countBase);
            data.RelicLv.Add(key, lvBase);
        }

        UpgradeLocalData.SaveData(data);

        return data;
    }

    public void LoadData()
    {
        UpgradeLocalData data = UpgradeLocalData.LoadData();

        FishesCount.Clear();
        FishesLv.Clear();

        foreach (string key in data.FishCount.Keys)
        {
            FishesCount.Add(key, new SecureInt(data.FishCount[key]));
            FishesLv.Add(key, new SecureInt(data.FishLv[key]));
        }

        RelicCount.Clear();
        RelicLv.Clear();

        foreach (string key in data.RelicCount.Keys)
        {
            RelicCount.Add(key, new SecureInt(data.RelicCount[key]));
            RelicLv.Add(key, new SecureInt(data.RelicLv[key]));
        }
    }
    #endregion
}

public class UpgradeLocalData
{
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
        
        var fishTable = TableMgr.GetTable("fish");

        Dictionary<string, string> fishCount = new Dictionary<string, string>();
        Dictionary<string, string> fishLv = new Dictionary<string, string>();

        foreach (string key in fishTable.Keys)
        {
            fishCount.Add(key, PlayerPrefs.GetString(string.Format(FishCountKey, key), ""));
            fishLv.Add(key, PlayerPrefs.GetString(string.Format(FishLvKey, key), ""));
        }

        var relicTable = TableMgr.GetTable("relic");

        Dictionary<string, string> relicCount = new Dictionary<string, string>();
        Dictionary<string, string> relicLv = new Dictionary<string, string>();

        foreach (string key in relicTable.Keys)
        {
            relicCount.Add(key, PlayerPrefs.GetString(string.Format(RelicCountKey, key), ""));
            relicLv.Add(key, PlayerPrefs.GetString(string.Format(RelicLvKey, key), ""));
        }

        return data;
    }
}