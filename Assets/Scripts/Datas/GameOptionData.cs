using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptionData : SingletonAllSecen<GameOptionData>
{
    public GameLanguage Language = GameLanguage.ko;

    public void SetLanguage(GameLanguage language)
    {
        Language = language;
    }
}

public class GameOptionLocal
{
    public string Locale = "ko";
    private static string LocaleKey = "la";

    public static void SaveData(GameOptionLocal data)
    {
        PlayerPrefs.SetString(LocaleKey, data.Locale);
        PlayerPrefs.Save();
    }

    public static GameOptionLocal LoadData()
    {
        GameOptionLocal data = new GameOptionLocal();
        data.Locale = PlayerPrefs.GetString(LocaleKey, TransMgr.GetLocaleString(GameLanguage.ko));
        return data;
    }
}