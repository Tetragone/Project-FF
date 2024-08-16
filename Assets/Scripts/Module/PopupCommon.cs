using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupCommon : MonoBehaviour
{
    public TextMeshProUGUI TextTitle;
    public TextMeshProUGUI TextContent;
    public Button ButtonClose;
    public Button ButtonYes;
    public Button ButtonNo;

    private string Title = "";
    private string Content = "";

    public void InitAfterSetting(bool IsUseNo, bool IsUseClose)
    {
        TextTitle.gameObject.SetActive(Title != "");
        TextTitle.text = Title;

        TextContent.gameObject.SetActive(Content != "");
        TextContent.text = Content;

        ButtonClose.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });

        ButtonYes.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });

        ButtonNo.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });

        ButtonNo.gameObject.SetActive(IsUseNo);
        ButtonClose.gameObject.SetActive(IsUseClose);
    }

    public void AddYesListener(UnityAction action)
    {
        ButtonYes.onClick.AddListener(action);
    }

    public void AddCloseListener(UnityAction action)
    {
        ButtonClose.onClick.AddListener(action);
    }

    public void AddNoListener(UnityAction action)
    {
        ButtonNo.onClick.AddListener(action);
    }

    public void SetText(string title, string content)
    {
        Title = title;
        Content = content;
    }
}
