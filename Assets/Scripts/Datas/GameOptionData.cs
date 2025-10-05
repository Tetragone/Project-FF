using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class GameOptionData : SingletonAllSecen<GameOptionData>
{
    public GameLanguage Language { get; private set; } = GameLanguage.ko;

    public void SetLanguage(GameLanguage language)
    {

        Language = language;
        SaveData();
        PopupMgr.MakeCommonPopup(TransMgr.GetText("안내창"), TransMgr.GetText("언어 변경으로 인해 게임을 종료합니다.")
            , false, false, () =>
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
    }

    #region Local Data Save
    public GameOptionLocal SaveData()
    {
        GameOptionLocal data = new GameOptionLocal();
        data.Locale = Language.ToString();
        GameOptionLocal.SaveData(data);
        return data;
    }

    public IEnumerator LoadData()
    {
        GameOptionLocal data = GameOptionLocal.LoadData();
        Language = (GameLanguage)Enum.Parse(typeof(GameLanguage), data.Locale);
        yield return StartCoroutine(SetLocale(Language.ToString()));
    }

    private IEnumerator SetLocale(string code)
    {
        // 로컬 데이터 로딩을 기다림
        yield return LocalizationSettings.InitializationOperation;

        var locales = LocalizationSettings.AvailableLocales.Locales;
        foreach (var locale in locales)
        {
            if (locale.Identifier.Code == code)
            {
                LocalizationSettings.SelectedLocale = locale;
                break;
            }
        }

        yield return null;
    }
    #endregion
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