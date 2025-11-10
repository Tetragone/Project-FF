using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Option : MonoBehaviour
{
    [Header("언어")]
    public TextMeshProUGUI TextLangTitle;
    public List<Toggle> ListToggleLang;

    [Header("계정 연동")]
    public TextMeshProUGUI TextLink;
    public Button ButtonEmail;

    private void Awake()
    {
        TextLangTitle.text = TransMgr.GetText("언어 설정");
        TextLink.text = TransMgr.GetText("계정 연동");

        for (int i = 0; i < ListToggleLang.Count; i++)
        {
            ListToggleLang[i].isOn = i == (int)GameOptionData.Instance.Language;
        }

        ButtonEmail.onClick.AddListener(() =>
        {
            PopupMgr.MakeLoginPopup();
        });
    }

    // Start에서 하는 이유는 Awake()에서 하면 object가 켜지지 않는다.
    private void Start()
    {
        for (int i = 0; i < ListToggleLang.Count; i++)
        {
            int temp = i;

            ListToggleLang[i].onValueChanged.AddListener((isOn) =>
            {
                GameOptionData.Instance.SetLanguage((GameLanguage)temp);
            });
        }
    }
}
