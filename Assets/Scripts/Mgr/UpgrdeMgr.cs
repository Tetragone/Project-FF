using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgrdeMgr : Singleton<UpgrdeMgr>
{
    private Dictionary<string, SecureInt> FishesCount = new Dictionary<string, SecureInt>();
    private Dictionary<string, SecureInt> FishedLv = new Dictionary<string, SecureInt>();
    private Dictionary<string, SecureInt> RelicCount = new Dictionary<string, SecureInt>();
    private Dictionary<string, SecureInt> RelicLv = new Dictionary<string, SecureInt>();


}

public class UpgradeLocalData
{
    // TODO : 로컬 저장 
    public string Gold;
    private static string GoldKey = "gk";
    public string GachaPoint;
    private static string GachaKey = "gak";
    public string RelicPoint;
    private static string RelicKey = "rk";

    public static void SaveData(UpgradeLocalData data)
    {
        PlayerPrefs.SetString(GoldKey, data.Gold);
        PlayerPrefs.SetString(GachaKey, data.GachaPoint);
        PlayerPrefs.SetString(RelicKey, data.RelicPoint);
        PlayerPrefs.Save();
    }

    public static UpgradeLocalData LoadData()
    {
        UpgradeLocalData data = new UpgradeLocalData();
        data.Gold = PlayerPrefs.GetString(GoldKey, "");
        data.GachaPoint = PlayerPrefs.GetString(GachaKey, "");
        data.RelicPoint = PlayerPrefs.GetString(RelicKey, "");
        return data;
    }
}