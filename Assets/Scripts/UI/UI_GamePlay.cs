using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamePlay : MonoBehaviour
{
    #region inspector
    public Button ButtonBlockCreate;
    public Button ButtonSelectBlock;
    public Button ButtonAttack;
    #endregion

    private void Awake()
    {
        InitButtonCallback();
    }

    private void InitButtonCallback()
    {
        ButtonBlockCreate.onClick.AddListener(() =>
        {

        });

        ButtonAttack.onClick.AddListener(() =>
        {

        });

        ButtonSelectBlock.onClick.AddListener(() =>
        {

        });
    }
}
