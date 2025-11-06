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
    private bool IsStart = false;
    private bool IsArriveBottom = false;
    private float DisableTimer = 0f;

    private void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
    }

    // TODO : 모든 먹이는 똑같으나, 강화도에 의해서 차이가 날 수 있도록 수정이 필요할듯. 
    public void InitValue(string id)
    {
        Fid = id;
        BaseValue = 2f * GameStaticValue.FishGrowMulti(UserDataMgr.Instance.GetGoldUpgrade(GoldUpgrade.food_grow));//TableMgr.GetTableFloat("food", id, "base_value");
        RandomValue = UnityEngine.Random.Range(GameStaticValue.BaseFoodRandomValue * -1, GameStaticValue.BaseFoodRandomValue);
        IsEating = false;
        IsStart = true;
        IsArriveBottom = false;
        DisableTimer = 0f;
    }

    public bool EatableFood()
    {
        return !IsEating;
    }

    public float GetValue()
    {
        return BaseValue + RandomValue;
    }

    public void EatThis()
    {
        IsEating = true;
        AquaMgr.Instance.DisableFood(this);
    }

    private void Update()
    {
        if (IsStart)
        {
            if (!IsArriveBottom)
            {
                Move();
            }
            else
            {
                DisableTimer += Time.deltaTime;

                if (DisableTimer > GameStaticValue.FoodDisableTime)
                {
                    AquaMgr.Instance.DisableFood(this);
                }
            }
        }
    }

    private void Move()
    {
        if (transform.position.y > GameStaticValue.FishMaxYPercent * CameraMgr.CameraSize)
        {
            Rigid.AddForce(Vector2.down * 9.81f);
        }
        else if (transform.position.y > GameStaticValue.FishMinYPercent * CameraMgr.CameraSize)
        {
            Rigid.AddForce(Vector2.up * 0.05f);
        }
        else
        {
            IsArriveBottom = true;
            DisableTimer = 0f;
            Stop();
        }
    }

    private void Stop()
    {
        Rigid.velocity = Vector3.zero;
    }
}
