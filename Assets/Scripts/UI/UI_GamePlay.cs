using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamePlay : MonoBehaviour
{
    #region inspector
    public Button ButtonCreateFood;
    public Button ButtonCreateRandomFish;
    public Button ButtonPause;
    #endregion

    private void Awake()
    {
        InitButtonCallback();
    }

    private void InitButtonCallback()
    {
        ButtonCreateFood.onClick.AddListener(() =>
        {
            AquaMgr.Instance.CreateFood();
        });

        ButtonPause.onClick.AddListener(() =>
        {

        });

        ButtonCreateRandomFish.onClick.AddListener(() =>
        {

        });
    }
}
