using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Loading : MonoBehaviour
{
    public TextMeshProUGUI TextPercent;
    public Image ImageLoadingBar;

    public void SetPercent(float now, float max)
    {
        ImageLoadingBar.fillAmount = now / max;
        TextPercent.text = string.Format("{0}%", Mathf.RoundToInt(now / max) * 100);
    }

    // max 100
    public void SetPercent(int now)
    {
        ImageLoadingBar.fillAmount = now / 100f;
        TextPercent.text = string.Format("{0}%", Mathf.RoundToInt(now / 100f) * 100);
    }

    // 계산되어서 들어오는 값
    public void SetPercent(float div)
    {
        ImageLoadingBar.fillAmount = div;
        TextPercent.text = string.Format("{0}%", Mathf.RoundToInt(div) * 100);
    }
}
