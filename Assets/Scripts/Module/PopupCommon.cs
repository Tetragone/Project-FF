using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupCommon : MonoBehaviour
{
    public TextMeshProUGUI TextTitle;
    public TextMeshProUGUI TextContent;
    public Button ButtonClose;
    public Button ButtonYes;
    public Button ButtonNo;

    public GameObject ObjButtons;

    [Space(7)]
    public List<GameObject> CustomObj;
    private string Title = "";
    private string Content = "";
    // 자기자신을 가능성이 높지만 확인용으로 사용.
    private GameObject ReleaseObj = null;

    public void InitAfterSetting(bool IsUseButtons, bool IsUseNo, bool IsUseClose)
    {
        if (TextTitle != null)
        {
            TextTitle.gameObject.SetActive(Title != "");
            TextTitle.text = Title;
        }

        if (TextContent != null)
        {
            TextContent.gameObject.SetActive(Content != "");
            TextContent.text = Content;
        }

        if (ButtonClose != null)
        {
            ButtonClose.onClick.AddListener(() =>
            {
                DestroyThis();
            });
            ButtonClose.gameObject.SetActive(IsUseClose);
        } 

        if (ButtonYes != null)
        {
            ButtonYes.onClick.AddListener(() =>
            {
                DestroyThis();
            });
        }

        if (ButtonNo != null)
        {
            ButtonNo.onClick.AddListener(() =>
            {
                DestroyThis();
            });
            ButtonNo.gameObject.SetActive(IsUseNo);
        }

        if (ObjButtons != null)
        {
            ObjButtons.SetActive(IsUseButtons);
        }
    }

    private void DestroyThis()
    {
        if (ReleaseObj != null)
        {
            AddressableMgr.ReleaseAfterMS(ReleaseObj);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetReleaseObj(GameObject obj)
    {
        ReleaseObj = obj;
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
