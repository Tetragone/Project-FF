using System;
using System.Collections.Generic;
using UnityEngine;

public class UserData : SingletonAllTime<UserData>
{
    public bool IsLoadingAll = false;
    protected bool _is_save_server = false;
    public bool IsSaveToServer
    {
        get
        {
            return _is_save_server;
        }
        set
        {
            _is_save_server = value;
            PlayerPrefs.GetInt("SaveServer", value ? 1 : 0);
        }
    }

    private Dictionary<string, RaceData> _have_races = new Dictionary<string, RaceData>();
    public Dictionary<string, RaceData> HaveRaces
    {
        get
        {
            return _have_races;
        }
    }

    private int _cash = 0;
    public int Cash
    {
        get
        {
            return _cash;
        }
    }

    private int _gold = 0;
    public int Gold
    {
        get
        {
            return _gold;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        IsSaveToServer = PlayerPrefs.GetInt("SaveServer", 0) == 0 ? true : false;
    }

    public void LoadData()
    {
        LoadLocalData();
        if (IsSaveToServer) LoadServerData();
        IsLoadingAll = true;
    }

    private void LoadLocalData()
    {
        _have_races = RaceData.GetLocal();
        _cash = PlayerPrefs.GetInt(GoodsType.cash.ToString(), 0);
        _gold = PlayerPrefs.GetInt(GoodsType.gold.ToString(), 0);
    }

    private void LoadServerData()
    {

    }

    // 재화 증감은 모두 여기서만 (보안 관련)
    public void AddGoods(GoodsType type, int inc)
    {
        switch (type)
        {
            case GoodsType.cash:
                _cash += inc;
                PlayerPrefs.SetInt(type.ToString(), _cash);
                break;
            case GoodsType.gold:
                _gold += inc;
                PlayerPrefs.SetInt(type.ToString(), _gold);
                break;
        }

        PlayerPrefs.Save();
    }
}

public class RaceData
{
    public string ID = "";
    public int Count = 0;
    public int Lv = 0;

    public RaceData()
    {
        ID = "";
        Count = 0;
        Lv = 0;
    }

    public RaceData(Dictionary<string, string> dic)
    {
        if (dic.ContainsKey("id"))
        {
            ID = dic["id"];
        }

        if (dic.ContainsKey("cnt"))
        {
            Count = Int32.Parse(dic["cnt"]);
        }

        if (dic.ContainsKey("lv"))
        {
            Lv = Int32.Parse(dic["lv"]);
        }
    }

    public Dictionary<string, string> ToDic(RaceData data)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        result.Add("id", data.ID);
        result.Add("cnt", Count.ToString());
        result.Add("lv", Lv.ToString());

        return result;
    }

    public static void SaveLocal(RaceData race)
    {
        PlayerPrefs.SetInt(string.Format("race_count_{0}", race.ID), race.Count);
        PlayerPrefs.SetInt(string.Format("race_lv_{0}", race.ID), race.Lv);
    }

    public static Dictionary<string, RaceData> GetLocal()
    {
        Dictionary<string, RaceData> result = new Dictionary<string, RaceData>();

        foreach (string key in TableMgr.GetTable("race").Keys) {
            RaceData race = new RaceData();
            race.ID = key;
            race.Count = PlayerPrefs.GetInt(string.Format("race_count_{0}", key), 0);
            race.Lv = PlayerPrefs.GetInt(string.Format("race_lv_{0}", key), 0);
            result.Add(key, race);
        }

        return result;
    }
}

public enum GoodsType
{
    cash, gold,
}
