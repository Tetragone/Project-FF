# Project FF  
**Unity 2022.3.62f2 | 개인 프로젝트**

---

## 개발 철학 및 코드 스타일

1. 변경 가능성과 확장성에 대한 생각
   - 게임 흐름 및 구조는 고정해두고 흐름 내부에서는 유연하게 바꿀 수 있도록 구현
   - 공용적으로 사용될 부분 혹은 패턴등은 재사용 가능하도록 확실하게 구현

2. 네이밍 규칙
   - 이름을 보면 어떤 역활인지 드러나도록 명명.

3. 예외 처리에 대한 접근
   - 문제가 생길 수 있는 원인을 고치자.
   - 문제는 숨기지 말고 드러나도록 하자.


### 최대한 단순한 게임 구조 지향

> 단순하며 확실하게.

- 복잡한 아키텍처보다는 직관적인 흐름을 우선함. 
- 수정해야한다는 것을 항상 생각하고 작업을 함.
  - 게임 구조는 고정되어 있으나, 디테일한 부분은 언제든지 수정 가능하도록 설계함.
  - Obejct Pool 혹은 Singleton등 공통 패턴이 재사용이 가능도록 작업함.


#### 코드 예시

> 게임 구조는 state를 활용하여 start 에서 arrived로 이뤄지는 자연스러운 흐름대로 큰틀이 잡아둠  
> 실제 디테일한 구현은 state 안에서 언제든지 수정하도록 설계

```csharp
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
            }
            break;
        case GameState.arrived:
            {
                if (MyFish.transform.position.y > CameraMgr.CameraSize * GameStaticValue.NonWhiteSpaceOnX)
                {
                    MakeWinPopup();
                }
            }
            break;
    }
}

public enum GameState
{
    prepare, start, arrived, end
}
```

> Object Pool과 Singleton 같은 경우 제네릭을 이용해서 구현

```csharp
// object pool
public PoolData(string key)
{
    ResKey = key;
    LoadRes();
}

public List<T> GetNowList()
{
    return NowObjects;
}

public async Task<T> GetNew()
{
    if (QueueValues.Count > 0)
    {
        T result = QueueValues.Dequeue();
        result.gameObject.SetActive(true);

        return result;
    }
    else
    {
        if (Res == null)
        {
            if (!IsLoading)
            {
                LoadRes();
            }

            while (IsLoading)
            {
                await Task.Delay(100);
            }
        }

        if (Pool == null)
        {
            GameObject trans = new GameObject();
            trans.name = string.Format("pool parent {0}", Res.gameObject.name);
            Pool = trans.transform;
        }

        T result = Object.Instantiate<T>(Res, Pool);

        return result;
    }
}

public void Add(T obj)
{
    NowObjects.Add(obj);
}

public void Remove(T obj)
{
    obj.gameObject.SetActive(false);
    QueueValues.Enqueue(obj);
    NowObjects.Remove(obj);
}
```

```csharp
// singleton
public static T Instance;

private void Awake()
{
    if (Instance == null)
    {
       Instance = GetComponent<T>();
    }
}
```

### 네이밍 규칙

> 이름만 보아도 역활을 알수 있도록.

1. 기본 규칙
   - 함수 / 멤버 변수 : PascalCase
   - 지역 변수 : camelCase

2. 함수 명명 방식
   - __동사로 시작하는 것__ 을 지향.
   - 축약을 피하고, 함수의 이름으로 기능이 예상될 수 있도록 명명함.
   - 함수 이름만으로 의미를 드러내기 어려운 경우 __주석을 통해 보완__ 하는 편.
   - 예시 : InitRace(), CreateEmenyFish(), StartGame()
  
3. 변수 명명 방식
   - 멤버 변수로 사용될 경우 __역할과 의미가 드러도록 명명__
   - 지역 변수로 사용될 경우 의미는 알 수 있도록 __단순하게 명명__
   - 예시 : (멤버 변수)EndingMeter, RaceFishMaxSpeed, (지역 변수) speed, range


### 예외 처리에 대한 접근

> 문제가 생길 수 있는 __원인에 먼저 예외처리__ 를 하는편  
> 다만 문제가 생긴다면 가리는 것보다는 드러나게 만들고 고치는 쪽을 선호

- 코드 작성 후, 직접 플레이 테스트를 통해 Unity 콘솔의 Error/Warning을 확인하면서 null이 발생할 수 있는 지점을 하나씩 보완하는 스타일.
- 문제가 드러나도록 내버려두고 원인을 수정하는 방식을 선호
- 눈에 보이지 않는 문제는 명시적으로 Debug 로그를 남겨서 추적 가능하게 처리.

#### 코드예시

> MyFish가 null이라면 PoolFish가 문제인 상황이니, PoolFish를 고쳐 원인을 수정하는 코드

```csharp
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
```

> AdultSpirte, ChildSpirte가 null이라면 게임에서 하얀 이미지로 나오도록 하는 코드

```csharp
AdultSpirte = AtlasMgr.Instance.GetFishesSprite(string.Format("{0}_adult", TableMgr.GetTableString("fish", fid, "res")));
ChildSpirte = AtlasMgr.Instance.GetFishesSprite(string.Format("{0}_child", TableMgr.GetTableString("fish", fid, "res")));

if (isChild)
{
    SpriteFish.sprite = ChildSpirte;
    transform.localScale = new Vector3(GameStaticValue.FishChildSize, GameStaticValue.FishChildSize);
}
else
{
    SpriteFish.sprite = AdultSpirte;
    transform.localScale = new Vector3(GameStaticValue.FishMaxGrowSize, GameStaticValue.FishMaxGrowSize);
}
```

> Unity의 Error Log로 볼 수 없는 서버 통신의 경우 Debug를 남기는 코드

```csharp
public void CallHttps(string name, Dictionary<string, object> data
    , UnityAction<Dictionary<object, object>> successAction, UnityAction failAction = null)
{
    data.Add("uid", FireAuth.Instance.UID);
    functions.GetHttpsCallable(name).CallAsync(data)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                foreach (var e in task.Exception.Flatten().InnerExceptions)
                {
                    if (e is FunctionsException fe)
                    {
                        Debug.LogError($"Firebase 함수 에러: {fe.ErrorCode} - {fe.Message}");
                    }
                }

                if (failAction != null)
                {
                    failAction.Invoke();
                }
            }
            else
            {
                Debug.LogFormat("function {0} is success", name);
                var data = task.Result.Data;
                var result = (Dictionary<object, object>)data;
                successAction.Invoke(result);
            } 
        });
}
```
---

## 프로젝트 개요
> “작고 단순하지만 구조적으로 탄탄한 게임”을 목표로 한 개인 프로젝트입니다.

**Project FF**는 물고기를 성장시켜 목표 지점에 도달하게 만드는 간단한 캐주얼 게임입니다.  
Unity 엔진의 주요 기능을 사용하고, 프로젝트 구조 및 최적화를 고민하기 위해 제작되었습니다.
| 로비 | 어항 | 레이스 |
|------|------|-----|
| <img width="219" height="389" alt="image" src="https://github.com/user-attachments/assets/6a3dc216-c2ae-41d7-86dc-8102768c35f0" /> | <img width="221" height="394" alt="image" src="https://github.com/user-attachments/assets/1f3cc5f8-b326-4550-8daa-428b30e2d593" /> | <img width="219" height="395" alt="image" src="https://github.com/user-attachments/assets/aa769a65-027e-43ec-a0f3-061194967770" /> |

---

## 프로젝트 구조

<img width="532" height="71" alt="image" src="https://github.com/user-attachments/assets/fc565fa7-fa7c-424f-a3c6-44b4b11c5b91" />

### 씬 구조
| Scene | 역할 |
|--------|------|
| **Title.scene** | 데이터 초기화 및 로딩 (서버 연결 포인트 가정) |
| **Game.scene** | 로비, 어항, 레이스 씬 통합 관리 |

> 규모가 작은 프로젝트 특성상, 불필요한 씬 분리를 최소화했습니다.  
> 필요시 `Lobby.scene`, `Aqua.scene`, `Race.scene` 등으로 세분화할 수 있는 구조로 설계했습니다.

### 핵심 코드 설명

1. 인게임 코드

| 코드 | 설명 |
| ------ | ------ |
| AquaMgr | 어항에서 사용되는 코드로 어항에서 뽑을 수 있는 물고기 관리 및 먹이 관리를 담당하고 있습니다. |
| RaceMgr | 레이스에서 사용되는 코드로서 레이스에서 나오는 경쟁 물고기, 내 물고기 관리를 담당하고 있습니다. |

2. 리소스 관리

| 코드 | 설명 |
| ------ | ------ |
| AddressableMgr | 오브젝트를 로딩할때 필요한 Addressable을 관리하는 코드입니다. |
| AltasMgr | Sprite를 로딩할때 사용되는 코드입니다. Altas를 메인으로 관리하고 있으며, Altas로 만들수 없는 Sprite 또한 여기서 관리하고 있습니다. |
| PoolData | 오브젝트 풀링에서 사용되는 코드로 오브젝트 풀링이 필요한 경우 사용되는 코드입니다. |

3. 데이터 관리

| 코드 | 설명 |
| ------ | ------ |
| DataLoadMgr | 데이터 로딩 및 데이터 저장을 담당하는 코드로 여러 데이터 매니저들이 생겼을떄 이 코드를 통해서 데이터를 로딩, 저장할 수 있도록 만든 코드입니다.  |
| TableMgr | 테이블에서 값을 가져올수 있도록 하는 코드입니다. |
| UserDataMgr | 유저의 기본적인 데이터를 가지고 있는 코드입니다. |
| GameOptionData | 게임 옵션과 관련된 값을 저장하는 코드입니다. |

4. UI 관리

| 코드 | 설명 |
| ------ | ------ |
| UI_Lobby | 메인 UI로, 다른 UI들은 이 코드를 통해 사용되거나, 이 코드에서 호출되는 방식입니다. |
| PopupMgr | Popup과 관련되어서 개념상 메인 UI의 하위가 될 수 없는 경우 이 코드를 통해서 호출됩니다. |

---

## 주요 화면

| 구분 | 이미지 |
|------|--------|
| **로비** | ![로비](https://github.com/user-attachments/assets/b19dce98-8b34-489f-9699-e33d2ccaa975) |
| **어항** | ![어항](https://github.com/user-attachments/assets/e4949fb3-c919-4f3f-92ac-55d0165289dc) |
| **레이스** | ![레이스](https://github.com/user-attachments/assets/d1fe10ea-c875-4e04-a471-ed5a815eea79) |
| **전체 흐름도** | ![흐름도](https://github.com/user-attachments/assets/f909bb97-1018-4890-9505-0f94ed08184a) |

---

## 사용 기술 및 구조
> Addressable, Object Pooling, Data Table, Localization, Local Save, Sprite Atlas 등  
> 실무 개발에서 자주 사용했던 시스템과 아직 사용 못한 시스템도 적용했습니다.

| 기술 | 주요 사용 파일 | 설명 | 실무 사용 여부 | 
|------|----------------|------|-----|
| **Addressable** | `PopupMgr.cs`, `PoolData.cs` | 팝업 및 풀링 리소스 로드에 사용 | X |
| **Pooling System** | `PoolData.cs`, `AquaMgr.cs`, `RaceMgr.cs` | Object 생성 최적화 | O |
| **Data Table** | `TableMgr.cs` | 각종 설정값 및 데이터 관리 | O |
| **Localization** | `TransMgr.cs` | Unity Localization 활용 | X |
| **Auth** | `FireAuth.cs` | 로그인 | O |
| **Local Save** | `UpgradeMgr.cs`, `UserDataMgr.cs` | PlayerPrefs 기반 로컬 저장 | X |
| **Server Save** | `FireCloudFunction.cs` | FireFunction과 연결 | O |
| **Atlas 관리** | `AtlasMgr.cs` | Draw Call 감소 및 성능 최적화 | O |
| **Rewarded AD** | `AdMgr.cs` | 보상형 광고 | O |
---

## 주요 시스템 구현

### Addressable (리소스 로드 최적화)
`PopupMgr` 및 `PoolData`에서 Addressables을 사용하여,  
`Resources.Load` 대비 메모리 효율과 관리 편의성을 높였습니다.

#### 코드 예시
```csharp
public PoolData(string key)
{
    ResKey = key;
    LoadRes();
}

private void LoadRes()
{
    IsLoading = true;
    Addressables.LoadAssetAsync<GameObject>(ResKey).Completed += (handle) =>
    {
        Res = handle.Result.GetComponent<T>();
        IsLoading = false;
    };
}
```

#### 사용 예시
```csharp
private void Awake()
{
    PoolFish = new PoolData<InAquaFish>(GameStaticValue.FishPath);
    PoolFood = new PoolData<Food>(GameStaticValue.FoodPath);
}
```

---

### Object Pooling (오브젝트 재활용)
`PoolData.cs`를 통해 풀링 시스템을 설계하였으며,  
`AquaMgr`와 `RaceMgr` 등에서 반복 생성되는 오브젝트(물고기, 먹이)에 적용했습니다.

#### 코드 예시
```csharp
public async Task<T> GetNew()
{
    if (QueueValues.Count > 0)
    {
        var result = QueueValues.Dequeue();
        result.gameObject.SetActive(true);
        return result;
    }

    if (Res == null)
    {
        if (!IsLoading) LoadRes();
        while (IsLoading) await Task.Delay(100);
    }

    if (Pool == null)
    {
        GameObject trans = new GameObject($"pool parent {Res.gameObject.name}");
        Pool = trans.transform;
    }

    return Object.Instantiate<T>(Res, Pool);
}
```

> 미리 생성하지 않고 “필요 시 생성 + 재활용” 구조로 설계하여,  
> 초반 부하를 줄이고, 장기 플레이 시에도 퍼포먼스를 유지했습니다.

#### 외부 사용 예시
```csharp
public async void CreateFish()
{
    InAquaFish newFish = await PoolFish.GetNew();

    if (HasFishes.Count == 0)
        HasFishes = UpgradeMgr.Instance.GetHasFishes();

    int idx = Random.Range(0, HasFishes.Count);
    newFish.Init(HasFishes[idx], true);
    newFish.transform.position = SetFishPosition();
    PoolFish.Add(newFish);
}
```

---
### Data Table (테이블)
csv파일을 읽어오는 방식을 사용했으며,  
`TableMgr.cs`를 이용하여 데이터를 읽어 올 수 있습니다.

#### 코드 예시
```csharp
private Dictionary<string, string> TableName = new Dictionary<string, string>() 
{
    { "fish", "Tables/ff_fish" },
};

private void Awake()
{ 
    StartCoroutine(ReadTables());
}

private IEnumerator ReadTables()
{
    int cnt = 0;

    foreach (string key in TableName.Keys)
    {
        Tables.Add(key, CSVReader.Read(TableName[key]));
        cnt++;
            
        if (cnt == CorutineCount)
        {
            yield return null;
        }
    }

    IsReaded = true;
}

public static string GetTableString(string name, string id, string colume)
{
    return Instance.Tables[name][id][colume];
}
```

#### 사용 예시
```csharp
string sid = TableMgr.GetTableString("relic", key, "sid");
string stype = TableMgr.GetTableString("stat", sid, "type");
```
> 테이블 이름과 id, column 이름을 넣으면 테이블 값을 불러오도록 만들었습니디.
---
### Auth
Firebase의 Auth를 사용했습니다.
`FireAuth.cs`에서 이메일을 이용한 로그인이 가능합니다.

#### 코드 예시
```csharp
public void TryLogin(string email, string password)
{
    Auth = FirebaseAuth.DefaultInstance;
    Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
    {
        if (task.IsCanceled)
        {
            Debug.LogError("Sign in Email was canceled.");
            return;
        }
        if (task.IsFaulted)
        {
            Debug.LogError("Sign in Email encountered an error: " + task.Exception);
            return;
        }

        AuthResult result = task.Result;
        UpdateObserver();
    });
}
```

#### 사용 예시
```csharp
ButtonLogin.onClick.AddListener(() =>
{
    string id = InputId.text;
    string password = InputPass.text;

    FireAuth.Instance.TryLogin(id, password);
});
```
---
### Localization (다국어 지원)
Unity의 **Localization 패키지**를 사용해 번역을 관리했습니다.  
기존 직접 구현 방식과 비교하여 유지보수성과 동기화 편의성을 검증하기 위함이었습니다.

> **TransMgr.cs**에서 테이블 기반 언어 변경을 지원합니다.

#### 코드 예시
```csharp
public static string GetText(string text)
{
    Locale locale = LocalizationSettings.AvailableLocales.GetLocale(GetLocaleString(GameLanguage.en));
    string comfire = LocalizationSettings.StringDatabase.GetLocalizedString(GameStaticValue.TransTable, text, locale);

    if (comfire == text)
    {
        return string.Format("NoTrans_{0}", text);
    }
    else
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(GameStaticValue.TransTable, text
            , LocalizationSettings.SelectedLocale);
    }
}
```

#### 사용 예시
```csharp
private void InitTextTrans()
{
    TextTitle.text = string.Format("{0} : {1}", TransMgr.GetText("스테이지"), UserDataMgr.Instance.Stage);
    TextUpgrade.text = TransMgr.GetText("강화");
    TextFishes.text = TransMgr.GetText("물고기");
    TextGameMenu.text = TransMgr.GetText("게임");
    TextRelices.text = TransMgr.GetText("유물");
    TextShopMenu.text = TransMgr.GetText("상점");
    TextPlay.text = TransMgr.GetText("게임 시작");
}
```
---

### Local Save (데이터 저장)
간단한 게임 구조이므로 **PlayerPrefs**를 사용했습니다.  
`DataLoadMgr`에서 저장 방식을 선택가능하도록 설계했습니다.

> 예: PlayerPrefs ⇄ 서버 저장 방식 선택 가능 구조

#### 저장 방식에 따른 데이터 호출 코드
```csharp
private static bool IsLoadFromServer 
{
    get
    {
        return Application.internetReachability != NetworkReachability.NotReachable
            && FireAuth.Instance.IsLoginUser;
    }
}

private void Awake()
{
    IsLoaded = false;
    StartCoroutine(StartLoad());
}

public static void SaveLocalData()
{
    if (IsLoadFromServer)
    {
        UserDataMgr.Instance.SaveDataOnServer();
    }
    else
    {
        UserDataMgr.Instance.SaveData();
    }
}
```

#### 로컬 데이터 호출 코드
``` csharp
public class UserLocalData
{
    public string Gold;
    private static string GoldKey = "gk";

    public static void SaveData(UserLocalData data)
    {
        PlayerPrefs.SetString(GoldKey, data.Gold);
        PlayerPrefs.Save();
    }

    public static UserLocalData LoadData()
    {
        UserLocalData data = new UserLocalData();
        data.Gold = PlayerPrefs.GetString(GoldKey, "");
        return data;
    }
}
```
---
### Server Save
서버에 데이터를 저장하기 위해 Firebase의 Fuction 기능을 사용하여 저장하도록 했습니다.
#### Function 호출 코드
``` csharp
public void CallHttps(string name, Dictionary<string, object> data
    , UnityAction<Dictionary<object, object>> successAction, UnityAction failAction = null)
{
    data.Add("uid", FireAuth.Instance.UID);
    functions.GetHttpsCallable(name).CallAsync(data)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                foreach (var e in task.Exception.Flatten().InnerExceptions)
                {
                    if (e is FunctionsException fe)
                    {
                        Debug.LogError($"Firebase 함수 에러: {fe.ErrorCode} - {fe.Message}");
                    }
                }

                if (failAction != null)
                {
                    failAction.Invoke();
                }
            }
            else
            {
                Debug.LogFormat("function {0} is success", name);
                var data = task.Result.Data;
                var result = (Dictionary<object, object>)data;
                successAction.Invoke(result);
            } 
        });
}
```

#### 서버 데이터 호출 코드
``` csharp
public void LoadDataOnServer(UnityAction action = null)
{
    FireCloudFunction.Instance.CallHttps("user_load", new Dictionary<string, object>(), (data =>
    {
        if (data == null)
        {
            Debug.LogError("data loading is error");
            return;
        }

        Dictionary<object, object> user = data["user"] as Dictionary<object, object>;
        foreach (string key in user.Keys)
        {
            switch (key)
            {
                case "gold":
                    Gold = new SecureInt(int.Parse(user["gold"].ToString()));
                    break;
                case "gold_lv":
                    {
                        var goldLv = user["gold_lv"] as Dictionary<object, object>;
                        GoldUpgradeLv.Clear();
                        foreach (string gkey in goldLv.Keys)
                        {
                            int value = int.Parse(goldLv[gkey].ToString());
                            GoldUpgrade upgrade = (GoldUpgrade)Enum.Parse(typeof(GoldUpgrade), gkey);
                            GoldUpgradeLv.Add(upgrade, value);
                        }
                    }
                    break;
            }

            if (action != null)
            {
                action.Invoke();
            }
        }));
}
```
---

### Sprite Atlas (Draw Call 최적화)
`AtlasMgr.cs`를 통해 이미지 리소스를 통합 관리하며,  
UI 렌더링 시 Draw Call 수를 줄여 경량화했습니다.

``` csharp
[SerializeField] private SpriteAtlas Common;

private Dictionary<string, Sprite> DicCommon = new Dictionary<string, Sprite>();

public Sprite GetCommonSprite(string path)
{
    if (!DicCommon.ContainsKey(path))
    {
        Sprite sprite = Common.GetSprite(path);

        if (sprite == null)
        {
            return null;
        }

        DicCommon.Add(path, sprite);
    }

    return DicCommon[path];
}
```
---
### Rewarded AD
`AdMgr.cs`를 통해 모든 광고 관련된 내용을 제어,  
`RewardAd.cs`를 통해 실제 보상형 광고 구현을 진행하였습니다.

#### 보상형 광고 구현
``` csharp
public bool LoadRewarded()
{
    IsLoading = true;

    if (Counter > 10)
    {
        return false;
    }

    if (Application.internetReachability == NetworkReachability.NotReachable)
    {
        return false;
    }

    var adRequest = new AdRequest();

    RewardedAd.Load(adUnitId, adRequest, (ad, error) =>
    {
        IsLoading = false;
        UpdateObserver();
            
        if (error != null)
        {
            Counter++;
            return;
        }

        InitReloadCounter();
        rewarded = ad;
    });

    return true;
}
```

#### 사용 예시
``` csharp
PopupMgr.ActiveLoadingPopup(true);
AdMgr.Instance.SetRewardAction(() =>
{
    GetItems();
    PopupMgr.ActiveLoadingPopup(false);
},
() =>
{
    RefreshInteract();
    PopupMgr.ActiveLoadingPopup(false);
});
AdMgr.Instance.ShowRewarded();
```

---

### 요약 포인트
- Unity Addressable, Pooling, Localization 등 **실무형 구조 직접 설계**
- **성능과 확장성**을 고려한 프로젝트 아키텍처 설계 경험
- **단일 개발자 프로젝트**로, 기획 → 구현 → 최적화 전 과정을 직접 수행

