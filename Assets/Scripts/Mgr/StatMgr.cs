using System.Collections.Generic;

public static class StatMgr
{
    // stat type - key - value로 사용.
    // key의 경우는 값이 갱신되는 경우를 생각해서 추가.
    private static Dictionary<StatType, Dictionary<string, SecureInt>> StatLv = new Dictionary<StatType, Dictionary<string, SecureInt>>();

    // key는 테이블 + id로
    public static void SetStat(StatType type, string key, int lv)
    {
        if (!StatLv.ContainsKey(type))
        {
            StatLv.Add(type, new Dictionary<string, SecureInt>());
        }

        if (StatLv[type].ContainsKey(key))
        {
            StatLv[type].Remove(key);
        }

        StatLv[type].Add(key, lv);
    }

    public static int GetStatLv(StatType type)
    {
        if (!StatLv.ContainsKey(type))
        {
            return 0;
        }

        int sum = 0;

        foreach(SecureInt value in StatLv[type].Values)
        {
            sum += value;
        }

        return sum;
    }

    public static int GetStatLv(StatType type, string key)
    {
        if (!StatLv.ContainsKey(type) || !StatLv[type].ContainsKey(key))
        {
            return 0;
        }

        return StatLv[type][key];
    }
}

public enum StatType
{
    gold_up, 
    start_fish_up, 
    hungry_fast, 
    tiem_exp, 
    fish_speed_up, 
    double_fish_rate, 
    double_food_rate, 
    eat_big_fish
}