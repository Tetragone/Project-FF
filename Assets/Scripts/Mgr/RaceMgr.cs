using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class RaceMgr : Singleton<RaceMgr>
{
    private PoolData<InRaceFish> PoolFish;
    [HideInInspector] public InRaceFish MyFish;

    public int Stage = 1;
    public bool IsEnd { get; private set; } = false;
    public GameState State { get; private set; } = GameState.prepare;

    [HideInInspector] public float EndingMeter = 100f;
    private float Meter;
    private float ChangeBgMeter;
    private bool IsChangedBg = false;
    private FishData MyData;
    private float Timer = 0f;

    protected override void SetDataInAwake()
    {
        PoolFish = new PoolData<InRaceFish>(GameStaticValue.RaceFishPath);
    }

    public async void SettingBeforePlay()
    {
        if (PoolFish == null)
        {
            SetDataInAwake();
        }

        MyFish = await PoolFish.GetNew();
    }

    public void InitRace(FishData fish)
    {
        MyFish.InitData(fish, true);
        MyFish.SetLocalScale(1f);
        MyFish.transform.position = Vector3.zero;
        MyFish.gameObject.SetActive(false);
        PoolFish.Add(MyFish);
        MyData = fish;
        Meter = 0f;
        CalEndingMeter();
        State = GameState.prepare;
        IsChangedBg = false;
    }

    private async void CreateEmenyFish()
    {
        InRaceFish fish = await PoolFish.GetNew();
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
        float mean = (MyData.Size + sizeAdder) * Mathf.Pow(1.001f, UserDataMgr.Instance.Stage);
        float size = NRandom.NormalRandom(mean, GameStaticValue.RaceFishSizeNormal);

        if (size < 0f)
        {
            size = 0.05f;
        }

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
        data.Fid = GachaMgr.GachaFish(1)[0];

        data.SetDataValueForEnemy(speed - MyData.Speed, size);
        fish.InitData(data, false);
        fish.SetLocalScale(size / MyData.Size);
        PoolFish.Add(fish);
    }

    private void CalEndingMeter()
    {
        EndingMeter = 30 * Mathf.Pow(1.2f, UserDataMgr.Instance.Stage);
        ChangeBgMeter = SceneGame.Instance.GetBgHeight(SceneGame.Instance.RaceFinishBg);
    }

    public void StartGame()
    {
        State = GameState.start;
        MyFish.gameObject.SetActive(true);
        Timer = 0f;

        for (int i = 0; i < GameStaticValue.RaceInitFishCount; i++)
        {
            CreateEmenyFish();
        }
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
        State = GameState.end;
        int gold = Mathf.RoundToInt(Meter);
        PopupMgr.MakeCommonPopup("", gold.ToString(), false, false, () =>
        {
            UserDataMgr.Instance.AddGoods(gold, GoodsType.gold);
            RaceMgr.Instance.EndGame();
        });
    }

    public void EndGame()
    {
        State = GameState.end;
        StartCoroutine(OpenLobyyUI());
    }

    private IEnumerator OpenLobyyUI()
    {
        UI_Lobby.Instance.StartFadeIn(0.5f);
        yield return new WaitForSeconds(0.5f);
        PoolFish.RemoveAll();
        UI_Lobby.Instance.SetActiveMenu(UI_Lobby.MenuType.game_menu);
        UI_Lobby.Instance.StartFadeOut(0.5f);
    }

    public string GetMeterToString()
    {
        return string.Format("{0}m", Mathf.RoundToInt(Meter));
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.start:
                {
                    Meter += MyData.Speed * Time.deltaTime;
                    SceneGame.Instance.MoveBg(MyData.Speed * Time.deltaTime);

                    if (Timer > GameStaticValue.RaceFishCreateTime)
                    {
                        Timer -= GameStaticValue.RaceFishCreateTime;
                        CreateEmenyFish();
                    }

                    Timer += Time.deltaTime;

                    if (Meter > EndingMeter)
                    {
                        ArriveGoal();
                    }

                    if (EndingMeter - Meter < ChangeBgMeter * 1.5f)
                    {
                        if (!IsChangedBg)
                        {
                            SceneGame.Instance.SetLastBg(SceneGame.Instance.RaceFinishBg);
                            IsChangedBg = true;
                        }
                    }
                }
                break;
            case GameState.arrived:
                {
                    SetMyFishDir(Vector3.up);

                    if (MyFish.transform.position.y > CameraMgr.CameraSize * GameStaticValue.NonWhiteSpaceOnX + 0.3f)
                    {
                        MakeWinPopup();
                        SetMyFishDir(Vector3.zero);
                    }
                }
                break;
        }
    }

    private void ArriveGoal()
    {
        State = GameState.arrived;
    }

    private void MakeWinPopup()
    {
        Debug.Log("end");
        State = GameState.end;
        int gold = Mathf.RoundToInt(EndingMeter * GameStaticValue.WinMulti);
        string title = string.Format("stage {0} clear", Stage);
        PopupMgr.MakeCommonPopup(title, gold.ToString(), false, false, () =>
        {
            UserDataMgr.Instance.AddGoods(gold, GoodsType.gold);
            UserDataMgr.Instance.AddGoods(GameStaticValue.StageClearGachaPoint, GoodsType.gacha_point);
            UserDataMgr.Instance.StageClear();
            RaceMgr.Instance.EndGame();
        });
    }
}


public enum GameState
{
    prepare, start, arrived, end
}