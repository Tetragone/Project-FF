using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceMgr : Singleton<RaceMgr>
{
    private PoolData<InRaceFish> PoolFish = new PoolData<InRaceFish>(GameStaticValue.RaceFishPath);
    [HideInInspector] public InRaceFish MyFish;

    public void InitRace(FishData fish)
    {
        MyFish = PoolFish.GetNew();
        MyFish.InitData(fish, true);
    }
}
