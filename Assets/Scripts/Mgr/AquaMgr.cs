using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaMgr : Singleton<AquaMgr>
{
    public PoolData<InAquaFish> PoolFish = new PoolData<InAquaFish>(GameStaticValue.FishPath);
    public PoolData<Food> PoolFood = new PoolData<Food>(GameStaticValue.FoodPath);
    
    public void CreateFood()
    {
        Food newFood = PoolFood.GetNew();
        newFood.InitValue("");
        PoolFood.Add(newFood);
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
}