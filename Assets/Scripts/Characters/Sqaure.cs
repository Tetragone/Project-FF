using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sqaure : PoolableObject
{
    public Image ImageBlock;
    public Button ButtonBlockSelet;

    private Color color;
    public Color ColorBlock {
        get
        {
            return color;
        }
        set
        {
            color = value;
            ImageBlock.color = value;
        }
    }

    public bool IsFilled = false;

    public void InitBlock()
    {
        IsFilled = false;
        ColorBlock = GameStaticValue.BASE_COLOR;
    }
}
