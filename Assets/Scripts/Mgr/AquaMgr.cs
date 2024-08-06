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
    private float GameTimer = 0f;
    
    public void InitStart()
    {
        Money = 0f;
        EanMoney = 2f;
        GameTimer = 0f;
        IsStart = true;
    }

    public void EndGame()
    {
        Money = 0f;
        EanMoney = 2f;
        IsStart = false;
        GameTimer = 0f;
    }

    public void CreateFood()
    {
        Food newFood = PoolFood.GetNew();
        newFood.InitValue("");
        newFood.transform.position = SetFoodPosition();
        PoolFood.Add(newFood);
    }

    public void DisableFood(Food food)
    {
        PoolFood.Remove(food);
    }

    private Vector3 SetFoodPosition()
    {
        float x = UnityEngine.Random.Range(CameraMgr.CameraSize * GameStaticValue.NonWhiteSpaceOnX * -1, CameraMgr.CameraSize * GameStaticValue.NonWhiteSpaceOnX);
        return new Vector3(x, CameraMgr.CameraSize * GameStaticValue.FoodCreateYPercent, 0);
    }

    public void CreateFish()
    {
        InAquaFish newFish = PoolFish.GetNew();
        newFish.Init("");
        PoolFish.Add(newFish);
    }

    public Food GetClosestFood(Vector3 position, float radius = 0f)
    {
        List<Food> foodes = PoolFood.GetNowList();

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
            GameTimer += Time.deltaTime;
            Money += EanMoney * Time.deltaTime;

            if (GameTimer > GameStaticValue.AquaTime)
            {
                GameEndEffect();
            }
        }
    }

    private void GameEndEffect()
    {
        // 물고기가 각 종류 별로 합쳐지면서 어떤 녀석을 고를건지 선택지가 나오도록 하자
        UI_Lobby.Instance.SetActiveMenu(UI_Lobby.MenuType.game_race_menu);
        RaceMgr.Instance.InitRace(PoolFish.GetNowList()[0].GetNowData());
        EndGame();
    }
}