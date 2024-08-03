using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Food : PoolableObject
{
    public Rigidbody2D Rigid;

    // Food id
    public string Fid;

    // Value;
    public float BaseValue;
    public float RandomValue;

    public void InitValue(string id)
    {
        Fid = id;
        BaseValue = TableMgr.GetTableFloat("food", id, "base_value");
        RandomValue = GameStaticValue.BaseFoodRandomValue;
    }

    public bool EatableFood()
    {
        return false;
    }

}
