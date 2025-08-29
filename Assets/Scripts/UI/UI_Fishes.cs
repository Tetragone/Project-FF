using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Fishes : MonoBehaviour
{
    public UI_FishBlock ResBlock;
    public Transform Content;
    private List<UI_FishBlock> FishBlocks = new List<UI_FishBlock>();

    private void Awake()
    {
        var table = TableMgr.GetTable("fish");

        foreach (var key in table.Keys)
        {
            UI_FishBlock block = Instantiate(ResBlock, Content);
            block.SetFishId(key);
            FishBlocks.Add(block);
        }
    }

    private void OEnable()
    {
        foreach (var block in FishBlocks)
        {
            block.Refresh();
        }
    }
}
