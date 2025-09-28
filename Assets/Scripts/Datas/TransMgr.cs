using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public static class TransMgr
{
    public static string GetLocaleString(GameLanguage locale)
    {
        switch (locale)
        {
            case GameLanguage.none:
                Debug.LogErrorFormat("locale input is Wrong");
                return "";
            default:
                return locale.ToString();
        }
    }

    public static GameLanguage GetLocaleEnum(string locale)
    {
        if (locale == "en")
        {
            return GameLanguage.en;
        } 
        else if (locale == "ko")
        {
            return GameLanguage.ko;
        } else
        {
            Debug.LogErrorFormat("locale input is Wrong. input : {0}", locale);
            return GameLanguage.none;
        }
    }

    public static string GetText(string text)
    {
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(GetLocaleString(GameLanguage.en));
        string comfire = LocalizationSettings.StringDatabase.GetLocalizedString(GameStaticValue.TransTable, text, locale);

        if (comfire == text)
        {
            return string.Format("NoTrans_{0}", text);
        }
        else
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(GameStaticValue.TransTable, text);
        }
    }
}

public enum GameLanguage
{
    ko, en, none,
}
