using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamePlay : MonoBehaviour
{
    #region inspector
    public Button ButtonCreateFood;
    public Button ButtonCreateRandomFish;
    public Button ButtonPause;

    public TextMeshProUGUI TextMoney;

    public TextMeshProUGUI TextTime;
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
        TextMoney.text = AquaMgr.Instance.GetMoney().ToString();
        AquaMgr.Instance.IsEnoughMoney(GameStaticValue.CostFish);
        AquaMgr.Instance.IsEnoughMoney(GameStaticValue.CostFood);
        TextTime.text = AquaMgr.Instance.GetLeftGameTime();
    }
}
