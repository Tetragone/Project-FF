using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStaticValue
{
    #region InAquaFish
    public static readonly float BaseFoodRandomValue = 0.5f;
    public static readonly float FishRandomMoveMaxTime = 5f;
    public static readonly float FishRandomMoveMinTime = 1f;
    public static readonly float FishRandomMoveMaxSpeed = 3f;
    public static readonly float FishRandomMoveMinSpeed = 1f;

    public static readonly float ChildStarveTime = 2f;
    public static readonly float AdultStarveTime = 10f;

    public static readonly float FishMaxGrowSize = 3f;
    public static readonly float FishChildSize = 1f;

    public static readonly float FoodEatRange = 0.5f;

    public static readonly string FishPath = "Prefabs/aqua_fish";
    public static readonly string FoodPath = "Prefabs/food";

    public static readonly int AquaInitFishCount = 2;
    public static readonly int CostFood = 1;
    public static readonly int CostFish = 20;

    // Camera size에 따라서 다르게 하기 위해서 
    public static readonly float NonWhiteSpaceOnX = 0.8f;
    public static readonly float FishMaxYPercent = 0.7f;
    public static readonly float FishMinYPercent = -1f;
    public static readonly float FoodCreateYPercent = 0.8f;

    public static readonly float FoodDisableTime = 3f;

    public static readonly float AquaTime = 10f;
    #endregion

    #region Race Fish
    public static readonly string RaceFishPath = "Prefabs/race_fish";

    public static readonly float RaceFishMaxSpeed = 30f;
    public static readonly float RaceFishMinSpeed = 5f;
    public static readonly float RaceFishSizeAdder = 0.5f;
    public static readonly int RaceInitFishCount = 5;
    public static readonly float RaceFishCreateTime = 1.5f;

    public static readonly int MyFishLayer = 8;
    public static readonly int EnemyFishLayer = 9;

    public static readonly float FishRaceYPercent = 3f;
    public static readonly float BaseFishSize = 3f;

    public static readonly float WinMulti = 1.5f;
    public static readonly int StageClearGachaPoint = 150;
    #endregion

    #region UI Constant
    public static readonly int FishPrice = 300;
    public static readonly int RelicPrice = 300;

    public static string GetGradePath(int grade)
    {
        switch (grade)
        {
            case 1:
                return "";
            case 2:
                return "";
            default:
                return "";

        }
    }
    #endregion

    #region Upgrade Constant
    public static int MaxFishLv = 15;
    public static int GetNeedFishLvUpCount(int nowLv)
    {
        return (int)(Mathf.Pow(nowLv, 2) + 10);
    }

    public static int NeedGold(int lv)
    {
        // 버림으로 하자.
        return Mathf.RoundToInt(Mathf.Pow(1.2f, lv)) * 20;
    }
    #endregion
}
