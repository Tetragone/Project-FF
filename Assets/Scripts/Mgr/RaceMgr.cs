using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceMgr : Singleton<RaceMgr>
{
    private PoolData<InRaceFish> PoolFish = new PoolData<InRaceFish>(GameStaticValue.RaceFishPath);
    [HideInInspector] public InRaceFish MyFish;
    
    private float Meter;
    private bool IsStart = false;
    private FishData MyData;
    private List<FishData> FishesData;

    public void InitRace(FishData fish)
    {
        MyFish = PoolFish.GetNew();
        MyFish.InitData(fish, true);
        MyData = fish;
        IsStart = true;
    }

    public void DisableFish(InRaceFish fish)
    {
        PoolFish.Remove(fish);
    }

    public void EndGame()
    {
        IsStart = false;
    }

    private void Update()
    {
        if (IsStart)
        {
            Meter += MyData.Speed * Time.deltaTime;

        }
    }
}
