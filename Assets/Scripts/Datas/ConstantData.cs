using System;
using System.Collections.Generic;
using UnityEngine;

public static class ConstantData
{
    public static int NeedGold(int lv)
    {
        // 버림으로 하자.
        return Mathf.RoundToInt(Mathf.Pow(1.2f, lv)) * 20;
    }
}
