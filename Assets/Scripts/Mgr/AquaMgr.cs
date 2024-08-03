using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaMgr : Singleton<AquaMgr>
{
    [HideInInspector] public List<InAquaFish> ListFishes = new List<InAquaFish>();
    [HideInInspector] public List<Food> ListFoods = new List<Food>();

    public void CreateFood(string id, Vector3 position)
    {

    }

    public Food GetClosestFood(Vector3 position, float radius = 0f)
    {
        if (ListFoods.Count > 0)
        {
            float distace = float.MaxValue;
            Food result = null;

            for (int i = 0; i < ListFoods.Count; i++)
            {
                float nowDistance = Vector3.Distance(position, ListFoods[i].transform.position) - radius;

                if (distace > nowDistance)
                {
                    distace = nowDistance;
                    result = ListFoods[i];
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