using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMgr : SingletonAllSecen<TableMgr>
{
    public static bool IsReaded = false;

    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> Tables = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
    private Dictionary<string, string> TableName = new Dictionary<string, string>() 
    {
        { "fish", "Tables/ff_fish" },
        { "relic", "Tables/ff_relic" },
    };

    private int CorutineCount = 10;

    protected override void Awake()
    { 
        base.Awake();
        StartCoroutine(ReadTables());
    }

    private IEnumerator ReadTables()
    {
        int cnt = 0;

        foreach (string key in TableName.Keys)
        {
            Tables.Add(key, CSVReader.Read(TableName[key]));
            cnt++;
            
            if (cnt == CorutineCount)
            {
                yield return null;
            }
        }

        IsReaded = true;
    }

    public static Dictionary<string, Dictionary<string, string>> GetTable(string name)
    {
        if (Instance.Tables.ContainsKey(name))
        {
            return Instance.Tables[name];
        }
        else
        {
            Debug.LogErrorFormat("{0}은 없는 테이블입니다.", name);
            return null;
        }
    }

    public static string GetTableString(string name, string id, string colume)
    {
        return Instance.Tables[name][id][colume];
    }

    public static int GetTableInt(string name, string id, string colume)
    {
        return Int32.Parse(Instance.Tables[name][id][colume]);
    }

    public static float GetTableFloat(string name, string id, string colume)
    {
        return float.Parse(Instance.Tables[name][id][colume]);
    }

    public static long GetTableLong(string name, string id, string colume)
    {
        return Int64.Parse(Instance.Tables[name][id][colume]);
    }
}
