# Project FF  
**Unity 2022.3.62f2 | 개인 프로젝트**

---

## 프로젝트 개요
> “작고 단순하지만 구조적으로 탄탄한 게임”을 목표로 한 개인 프로젝트입니다.

**Project FF**는 물고기를 성장시켜 목표 지점에 도달하게 만드는 간단한 캐주얼 게임입니다.  
Unity 엔진의 주요 기능을 사용하고, 프로젝트 구조 및 최적화를 고민하기 위해 제작되었습니다.
| 로비 | 어항 | 레이스 |
|------|------|-----|
| <img width="219" height="389" alt="image" src="https://github.com/user-attachments/assets/6a3dc216-c2ae-41d7-86dc-8102768c35f0" /> | <img width="221" height="394" alt="image" src="https://github.com/user-attachments/assets/1f3cc5f8-b326-4550-8daa-428b30e2d593" /> | <img width="219" height="395" alt="image" src="https://github.com/user-attachments/assets/aa769a65-027e-43ec-a0f3-061194967770" /> |

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
| **Local Save** | `UpgradeMgr.cs`, `UserDataMgr.cs` | PlayerPrefs 기반 로컬 저장 | X |
| **Atlas 관리** | `AtlasMgr.cs` | Draw Call 감소 및 성능 최적화 | O |

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
서버 저장 구조로의 확장도 고려하여, `DataLoadMgr`에서 저장 방식을 유연하게 설계했습니다.

> 예: PlayerPrefs ⇄ 서버 저장 방식 선택 가능 구조

#### 저장 방식에 따른 데이터 호출 코드
```csharp
private void Awake()
{
    DataLoad data = DataLoad.LoadLocal();
    IsLoadFromServer = data.IsSaveServer;
}

public static void SaveLocalData()
{
    if (IsLoadFromServer)
    {
        // 서버 저장 방식이 추가될때 사용할 공간
    }
    else
    {
        UserDataMgr.Instance.SaveData();
        UpgradeMgr.Instance.SaveData();
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

## 프로젝트 구조

| Scene | 역할 |
|--------|------|
| **Title.scene** | 데이터 초기화 및 로딩 (서버 연결 포인트 가정) |
| **Game.scene** | 로비, 어항, 레이스 씬 통합 관리 |

> 규모가 작은 프로젝트 특성상, 불필요한 씬 분리를 최소화했습니다.  
> 필요시 `Lobby.scene`, `Aqua.scene`, `Race.scene` 등으로 세분화할 수 있는 구조로 설계했습니다.

---

## 주요 화면

| 구분 | 이미지 |
|------|--------|
| **로비** | ![로비](https://github.com/user-attachments/assets/b19dce98-8b34-489f-9699-e33d2ccaa975) |
| **어항** | ![어항](https://github.com/user-attachments/assets/e4949fb3-c919-4f3f-92ac-55d0165289dc) |
| **레이스** | ![레이스](https://github.com/user-attachments/assets/d1fe10ea-c875-4e04-a471-ed5a815eea79) |
| **전체 흐름도** | ![흐름도](https://github.com/user-attachments/assets/f909bb97-1018-4890-9505-0f94ed08184a) |

---

### 요약 포인트
- Unity Addressable, Pooling, Localization 등 **실무형 구조 직접 설계**
- **성능과 확장성**을 고려한 프로젝트 아키텍처 설계 경험
- **단일 개발자 프로젝트**로, 기획 → 구현 → 최적화 전 과정을 직접 수행

