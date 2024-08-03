using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Food : MonoBehaviour
{
    public Rigidbody2D Rigid;

    // Food id
    public string Fid;

    // Value;
    public float BaseValue;
    public float RandomValue;

    private bool IsEating = false;

    // TODO : 모든 먹이는 똑같으나, 강화도에 의해서 차이가 날 수 있도록 수정이 필요할듯. 
    public void InitValue(string id)
    {
        Fid = id;
        BaseValue = TableMgr.GetTableFloat("food", id, "base_value");
        RandomValue = GameStaticValue.BaseFoodRandomValue;
        IsEating = false;
    }

    public bool EatableFood()
    {
        return IsEating;
    }


    public float GetValue()
    {
        return BaseValue + RandomValue;
    }

    public void EatThis()
    {
        IsEating = true;
    }
}
