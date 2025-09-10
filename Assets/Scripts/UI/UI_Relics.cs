using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Relics : MonoBehaviour
{
    public UI_RelicBlock ResBlock;
    public Transform Content;
    private List<UI_RelicBlock> FishBlocks = new List<UI_RelicBlock>();

    private void Awake()
    {
        var table = TableMgr.GetTable("relic");

        foreach (var key in table.Keys)
        {
            UI_RelicBlock block = Instantiate(ResBlock, Content);
            block.SetRelicId(key);
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
