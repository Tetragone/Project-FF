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
            if (AquaMgr.Instance.IsEnoughMoney(GameStaticValue.CostFood))
            {
                AquaMgr.Instance.UseMoney(GameStaticValue.CostFood);
                AquaMgr.Instance.CreateFood();
            }
        });

        ButtonPause.onClick.AddListener(() =>
        {

        });

        ButtonCreateRandomFish.onClick.AddListener(() =>
        {
            if (AquaMgr.Instance.IsEnoughMoney(GameStaticValue.CostFish))
            {
                AquaMgr.Instance.UseMoney(GameStaticValue.CostFish);
                AquaMgr.Instance.CreateFish();
            }
        });
    }

    private void Update()
    {
        AquaMgr.Instance.IsEnoughMoney(GameStaticValue.CostFish);
        AquaMgr.Instance.IsEnoughMoney(GameStaticValue.CostFood);
    }
}
