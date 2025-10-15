using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AquaMgr : Singleton<AquaMgr>
{
    // addressable를 이용해 풀을 생성
    // 선언과 동시에 풀 생성 불가능.
    private PoolData<InAquaFish> PoolFish;
    private PoolData<Food> PoolFood;
    
    private float Money = 0f;
    private float EanMoney = 2f;
    private bool IsStart = false;
    private float GameTimer = 0f;
    private float EndTimeMulti = 1f;

    private FishData Selected;
    private List<string> HasFishes = new List<string>();

    protected override void SetDataInAwake()
    {
        PoolFish = new PoolData<InAquaFish>(GameStaticValue.FishPath);
        PoolFood = new PoolData<Food>(GameStaticValue.FoodPath);
    }

    public void InitStart()
    {
        Money = 0f;
        EanMoney = 2f;
        GameTimer = 0f;
        IsStart = true;
        EndTimeMulti = GameStaticValue.EndingTimeMulti(UpgradeMgr.Instance.GetGoldUpgrade(GoldUpgrade.grow_time));

        for (int i = 0; i < GameStaticValue.AquaInitFishCount; i++)
        {
            CreateFish();
        }
    }

    public void EndGame()
    {
        RaceMgr.Instance.InitRace(Selected);
        PoolFish.RemoveAll();
        PoolFood.RemoveAll();
        Money = 0f;
        EanMoney = 2f;
        GameTimer = 0f;
        UI_Lobby.Instance.StartFadeIn(0.5f);
        StartCoroutine(OpenRaceUI());
    }

    private IEnumerator OpenRaceUI()
    {
        yield return new WaitForSeconds(0.5f);
        UI_Lobby.Instance.SetActiveMenu(UI_Lobby.MenuType.game_race_menu);
        UI_Lobby.Instance.StartFadeOut(0.5f);
    }


    public async void CreateFood()
    {
        Food newFood = await PoolFood.GetNew();
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
        float range = CameraMgr.CameraSize * GameStaticValue.NonWhiteSpaceOnX;
        float x = UnityEngine.Random.Range(range* -1, range);
        return new Vector3(x, CameraMgr.CameraSize * GameStaticValue.FoodCreateYPercent, 0);
    }

    public async void CreateFish()
    {
        InAquaFish newFish = await PoolFish.GetNew();

        if (HasFishes.Count == 0)
        {
            HasFishes = UpgradeMgr.Instance.GetHasFishes();
        }

        int idx = Random.Range(0, HasFishes.Count);
        newFish.Init(HasFishes[idx], true);
        newFish.transform.position = SetFishPosition();
        PoolFish.Add(newFish);
    }

    private Vector3 SetFishPosition()
    {
        float rangeX = CameraMgr.CameraSize * GameStaticValue.NonWhiteSpaceOnX;
        float x = UnityEngine.Random.Range(rangeX * -1, rangeX);
        float y = UnityEngine.Random.Range(CameraMgr.CameraSize * GameStaticValue.FishMinYPercent, CameraMgr.CameraSize * GameStaticValue.FishMaxYPercent);
        return new Vector3(x, y, 0);
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

    public string GetLeftGameTime()
    {
        int leftTime = Mathf.CeilToInt(GameStaticValue.AquaTime * EndTimeMulti - GameTimer);

        return string.Format("{0}:{1}", (leftTime / 60).ToString("D2"), (leftTime % 60).ToString("D2"));
    }

    private void Update()
    {
        if (IsStart)
        {
            GameTimer += Time.deltaTime;
            Money += EanMoney * Time.deltaTime;

            if (GameTimer > GameStaticValue.AquaTime * EndTimeMulti)
            {
                OpenFishSelectPopup();
            }
        }
    }

    private void OpenFishSelectPopup()
    {
        IsStart = false;
        RaceMgr.Instance.SettingBeforePlay();
        var data = CalFishValue();

        PopupMgr.MakeFishSelectPopup(data, () =>
        {
            EndGame();
        });
    }

    private Dictionary<string, FishData> CalFishValue()
    {
        Dictionary<string, FishData> result = new Dictionary<string, FishData>();
        List<InAquaFish> fishes = PoolFish.GetNowList();

        for (int i = 0; i < fishes.Count; i++)
        {
            string key = fishes[i].GetNowData().Fid;

            if (result.ContainsKey(key))
            {
                if (result[key].TotalValue < fishes[i].GetNowData().TotalValue)
                {
                    result[key] = fishes[i].GetNowData();
                }
            }
            else
            {
                result.Add(key, fishes[i].GetNowData());
            }
        }

        for (int i = 0; i < fishes.Count; i++)
        {
            string key = fishes[i].GetNowData().Fid;

            // 말은 안되지만 확인차
            if (!result.ContainsKey(key)) 
            { 
                continue;
            }

            if (result[key].TotalValue != fishes[i].GetNowData().TotalValue)
            {
                result[key].AddAdditionalValue(fishes[i].GetNowData().TotalValue);
            }
        }

        return result;
    }

    public void SelectFish(FishData data)
    {
        Selected = data;
    }
}