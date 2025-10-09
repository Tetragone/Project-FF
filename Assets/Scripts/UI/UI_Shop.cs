using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    public UI_ShopCategoy CategoyRes;
    public UI_ShopBlock BlockRes;

    public Transform Content;

    private Dictionary<string, UI_ShopCategoy> Categories = new Dictionary<string, UI_ShopCategoy>();

    private void Awake()
    {
        var table = TableMgr.GetTable("shop");

        foreach (string key in table.Keys)
        {
            string category = table[key]["category"];

            if (!Categories.ContainsKey(category))
            {
                UI_ShopCategoy newCate = Instantiate(CategoyRes, Content);
                newCate.TextTitle.text = TransMgr.GetText(table[key]["category_name"]);
                Categories.Add(category, newCate);
            }

            UI_ShopBlock newBlock = Instantiate(BlockRes, Categories[category].Content);
            Categories[category].ListBlocks.Add(newBlock);
            newBlock.Parent = this;
            newBlock.SetIcon(key);
        }
    }

    public void RefreshAllIcon()
    {
        foreach (UI_ShopCategoy value in Categories.Values)
        {
            foreach (UI_ShopBlock block in value.ListBlocks)
            {
                block.RefreshInteract();
            }
        }
    }

    private void OnEnable()
    {
        RefreshAllIcon();
    }
}
