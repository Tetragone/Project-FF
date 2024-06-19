using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr
{
    private Dictionary<string, string> EnemyNameToEid = new Dictionary<string, string>();

    private string GetEid(string name)
    {
        if (EnemyNameToEid.Count == 0)
        {
            var table = TableMgr.GetTable("enemy");

            foreach (var key in table.Keys)
            {
                EnemyNameToEid.Add(table[key]["name"], key);
            }
        }

        return EnemyNameToEid.ContainsKey(name) ? EnemyNameToEid[name] : "";
    }

    public BaseCharacters GetChar(string name)
    {
        if (!GameStaticValue.DIC_EMENY.ContainsKey(name))
        {
            return null;
        }

        BaseCharacters character = ObjectPoolMgr.Instance.Pop(name) as BaseCharacters;
        character.gameObject.SetActive(true);
        character.Init(GetEid(name));
        character.SetPosition(InGameStaticValue.StartPosition);
        return character;
    }
}
