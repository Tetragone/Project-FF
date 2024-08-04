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

    public static readonly float FoodEatRange = 0.5f;

    public static readonly string FishPath = "";
    public static readonly string FoodPath = "";

    public static readonly int CostFood = 1;
    public static readonly int CostFish = 20;
    #endregion

    public static readonly List<Color> LIST_COLOR_BLOCK = new List<Color>() { Color.blue, Color.yellow, Color.magenta, Color.cyan, Color.red, Color.green, Color.black };
    public static readonly Color BASE_COLOR = Color.white;
    public static readonly int BOARD_WIDTH = 5;
    public static readonly int BOARD_HEIGTH = 4;
    public static readonly float ENEMY_INTERVAL = 0.5f;
    public static readonly Dictionary<string, string> DIC_EMENY = new Dictionary<string, string>()
    {
        { "normal", "Enemy/NormalEnemy" },
        { "power", "Enemy/PowerEnemy" },
        { "speed", "Enemy/SpeedEnemy" }
    };

    public static readonly int SPECIAL_ENEMY_INTERVAL = 10;
    public static readonly string[] SPECIAL_ENEMY_ID = new string[] { "power", "speed" };
    public static readonly int MAX_LIFE = 3;

}

public enum BlockType
{
    I, O, L, J, Z, S, T, none
}

public enum BlockRotataion
{
    up, right, down, left
}