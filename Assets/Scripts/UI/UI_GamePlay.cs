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
    public Transform TransformCreate;
    #endregion

    private void Awake()
    {
        InitButtonCallback();
    }

    private void InitButtonCallback()
    {
        ButtonBlockCreate.onClick.AddListener(() =>
        {
            if (!GameMgr.Instance.IsSelectBlock)
            {
                GameMgr.Instance.CreateBlock();
                GameMgr.Instance.SetBlockParent(TransformCreate);
            }
        });

        ButtonAttack.onClick.AddListener(() =>
        {
            if (!GameMgr.Instance.IsSelectBlock)
            {
                GameMgr.Instance.Attack();
            }
        });

        ButtonSelectBlock.onClick.AddListener(() =>
        {
            if (GameMgr.Instance.IsSelectBlock)
            {
                GameMgr.Instance.Board.FilledSelectedBlock();
            }
        });
    }
}
