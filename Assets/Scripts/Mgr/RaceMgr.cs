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
    private bool IsEnd = false;
    private FishData MyData;
    private List<FishData> FishesData;
    private float Timer = 0f;

    public void InitRace(FishData fish)
    {
        MyFish = PoolFish.GetNew();
        MyFish.InitData(fish, true);
        MyFish.SetLocalScale(1f);
        MyData = fish;
        IsStart = true;
        IsEnd = false;
        Timer = 0f;

        for (int i = 0; i < GameStaticValue.RaceInitFishCount; i++)
        {
            CreateEmenyFish();
        }
    }

    private void CreateEmenyFish()
    {
        InRaceFish fish = PoolFish.GetNew();
        // Speed 랜덤부터
        float middleSpeed = MyData.Speed;
        
        if (middleSpeed < GameStaticValue.RaceFishMinSpeed)
        {
            middleSpeed = GameStaticValue.RaceFishMinSpeed;
        }

        if (middleSpeed > GameStaticValue.RaceFishMaxSpeed)
        {
            middleSpeed = GameStaticValue.RaceFishMaxSpeed;
        }

        float speed = NRandom.Range(GameStaticValue.RaceFishMinSpeed, GameStaticValue.RaceFishMaxSpeed, middleSpeed);

        // Speed가 결정되면 그 값에 따라서 난이도 조정을 위한 Size값 변경.
        // 시간에 따라서 난이도가 바뀔수 있게 공식을 추가하는 것도 필요,
        // 속도 차이에 따라서 뭔가 더 차이가 나도록 수정하는 것도 필요
        float sizeAdder = speed > MyData.Speed ? GameStaticValue.RaceFishSizeAdder : GameStaticValue.RaceFishSizeAdder * -1;
        float size = NRandom.NormalRandom(MyData.Size + sizeAdder);

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

        data.SetDataValueForEnemy(speed - MyData.Speed, size);
        fish.InitData(data, false);
        fish.SetLocalScale(size / MyData.Size);
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

    public void MakeEndPopup()
    {
        IsEnd = true;
        PopupMgr.MakePopupEndGame(100);
    }

    public void EndGame()
    {
        IsStart = false;
        UI_Lobby.Instance.SetActiveMenu(UI_Lobby.MenuType.game_menu);
    }

    public string GetMeterToString()
    {
        return string.Format("{0}m", Mathf.RoundToInt(Meter));
    }

    private void Update()
    {
        if (IsStart)
        {
            if (!IsEnd) Meter += MyData.Speed * Time.deltaTime;

            if (Timer > GameStaticValue.RaceFishCreateTime)
            {
                Timer -= GameStaticValue.RaceFishCreateTime;
                CreateEmenyFish();
            }

            Timer += Time.deltaTime;
        }
    }
}
