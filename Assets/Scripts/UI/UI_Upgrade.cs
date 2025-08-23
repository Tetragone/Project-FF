using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Upgrade : MonoBehaviour
{
    public List<UI_UpgradeBlock> UpgradeBlockes = new List<UI_UpgradeBlock>();

    private void Awake()
    {
        for (int i = 0; i < UpgradeBlockes.Count; i++)
        {
            UpgradeBlockes[i].ChangeType((GoldUpgrade)i);
            UpgradeBlockes[i].UpgradeButton.onClick.AddListener(() =>
            {
                for (int j = 0; j < UpgradeBlockes.Count; j++)
                {
                    UpgradeBlockes[j].Refresh();
                }
            });
        }
    }
}
