using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaMgr : Singleton<AquaMgr>
{
    public PoolData<InAquaFish> PoolFish = new PoolData<InAquaFish>(GameStaticValue.FishPath);
    public PoolData<Food> PoolFood = new PoolData<Food>(GameStaticValue.FoodPath);
    
    private float Money = 0f;
    private float EanMoney = 2f;
    private bool IsStart = false;

    
    // 굳이 새로 만들지 말고 있는거 껏다 키는 방식으로 제어하자
    private void OnEnable()
    {
        InitStart();
    }

    public void InitStart()
    {
        Money = 0f;
        EanMoney = 2f;
        IsStart = true;
    }

    private void OnDisable()
    {
        EndGame();
    }

    public void EndGame()
    {
        Money = 0f;
        EanMoney = 2f;
        IsStart = false;
    }

    public void CreateFood()
    {
        Food newFood = PoolFood.GetNew();
        newFood.InitValue("");
        newFood.transform.position = SetFoodPosition();
        PoolFood.Add(newFood);
    }

    private Vector3 SetFoodPosition()
    {
        return new Vector3(1, 2, 0);
    }

    public void CreateFish()
    {
        InAquaFish newFish = PoolFish.GetNew();
        newFish.Init("");
        PoolFish.Add(newFish);
    }

    public Food GetClosestFood(Vector3 position, float radius = 0f)
    {
        List<Food> foodes = PoolFood.GetNow();

        if (foodes.Count > 0)
        {
            float distace = float.MaxValue;
            Food result = null;

            for (int i = 0; i < foodes.Count; i++)
            {
                if (!foodes[i].EatableFood())
                {
                    continue;
                }

                float nowDistance = Vector3.Distance(position, foodes[i].transform.position) - radius;

                if (distace > nowDistance)
                {
                    distace = nowDistance;
                    result = foodes[i];
                }
            }

            return result;
        }
        else
        {
            return null;
        }
    }

    public int GetMoney()
    {
        return Mathf.FloorToInt(Money);
    }

    public bool IsEnoughMoney(int cost)
    {
        return GetMoney() >= cost;
    }

    public void UseMoney(int cost)
    {
        if (IsEnoughMoney(cost))
        {
            Money -= cost;
        }
    }

    private void Update()
    {
        if (IsStart)
        {
            Money += EanMoney * Time.deltaTime;
        }
    }
}