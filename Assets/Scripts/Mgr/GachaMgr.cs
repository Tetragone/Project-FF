using System.Collections;
using System.Collections.Generic;

public static class GachaMgr
{
    private static List<int> RateGrade = new List<int>() { 10000, 2000, 500, 20 };

    public static int GetRandomIdxFromRate(List<int> rate)
    {
        int rateSum = 0;

        foreach (int singleRate in rate)
        {
            rateSum += singleRate;
        }

        int random = UnityEngine.Random.Range(0, rateSum);

        for (int i = 0; i < rate.Count; i++)
        {
            random -= rate[i];

            if (random < 0)
            {
                return i;
            }
        }

        return rate.Count - 1;
    }

    public static List<string> GachaFish(int count)
    {
        List<string> result = new List<string>();

        var table = TableMgr.GetTable("fish");
        Dictionary<int, List<string>> fishBaseGrade = new Dictionary<int, List<string>>();

        foreach (string key in table.Keys)
        {
            int grade = int.Parse(table[key]["grade"]);

            if (!fishBaseGrade.ContainsKey(grade))
            {
                List<string> temp = new List<string>();
                fishBaseGrade.Add(grade, temp);
            }

            fishBaseGrade[grade].Add(key);
        }

        for (int i = 0; i < count; i++)
        {
            int grade = GetRandomIdxFromRate(RateGrade);
            int idx = UnityEngine.Random.Range(0, fishBaseGrade[grade].Count);
            result.Add(fishBaseGrade[grade][idx]);
        }

        return result;
    }
}
