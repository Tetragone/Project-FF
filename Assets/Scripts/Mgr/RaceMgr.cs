using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RaceMgr : Singleton<RaceMgr>
{
    private PoolData<InRaceFish> PoolFish = new PoolData<InRaceFish>(GameStaticValue.RaceFishPath);
    [HideInInspector] public InRaceFish MyFish;
    
    private float Meter;
    private bool IsStart = false;
    private FishData MyData;
    private List<FishData> FishesData;
    private float Timer = 0f;

    public void InitRace(FishData fish)
    {
        MyFish = PoolFish.GetNew();
        MyFish.InitData(fish, true);
        MyData = fish;
        IsStart = true;
        Timer = 0f;

        for (int i = 0; i < GameStaticValue.RaceInitFishCount; i++)
        {
            CreateEmenyFish();
        }
    }

    private void CreateEmenyFish()
    {
        InRaceFish fish = PoolFish.GetNew();
        float speed = Random.Range(GameStaticValue.RaceFishMinSpeed, GameStaticValue.RaceFishMaxSpeed);
        float size = 0f;
        float range = CameraMgr.CameraSize * GameStaticValue.NonWhiteSpaceOnX;
        float randomX = Random.Range(range * -1, range);

        if (speed > MyData.Speed)
        {
            fish.transform.position = new Vector3(randomX, CameraMgr.CameraSize * GameStaticValue.FishRaceYPercent * -1, 0f);
        }
        else
        {
            fish.transform.position = new Vector3(randomX, CameraMgr.CameraSize * GameStaticValue.FishRaceYPercent, 0f);
        }

        FishData data = fish.GetData();

        if (data == null)
        {
            data = new FishData();
        }

        data.SetDataValueForEnemy(speed, size);
        fish.InitData(data, false);
    }

    public float GameBaseSpeed()
    {
        return MyData.Speed;
    }

    public void SetMyFishDir(Vector3 dir)
    {
        MyFish.SetDirForMy(dir);
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

            if (Timer > GameStaticValue.RaceFishCreateTime)
            {
                Timer -= GameStaticValue.RaceFishCreateTime;
                CreateEmenyFish();
            }

            Timer += Time.deltaTime;
        }
    }
}
