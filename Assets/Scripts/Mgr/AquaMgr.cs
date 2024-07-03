using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaMgr : Singleton<AquaMgr>
{
    [HideInInspector] public List<BaseCharacters> ListEnemy = new List<BaseCharacters>();

    public bool IsLoadingObjectPool { get; set; } = false;

    private bool IsCheckingDied = false;
    private bool IsInited = false;
    private float EnemyCreateTimer = 0.0f;
    private int IndexEnemy = 0;
    private int SpecialIndex = 0;
    private float Diff = 1f;

    #region Unity Function
    // awake로 하면 instance가 없을수도 있다. 
    private void Start()
    {
        StartCoroutine(InitPools());
    }

    private void Update()
    {
        if (!IsInited)
        {
            return;
        }

        if (IsCheckingDied)
        {
            CheckListEnemy();
        }

        if (EnemyCreateTimer <= 0.0f)
        {
            ResponeEnemy();
            EnemyCreateTimer += GameStaticValue.ENEMY_INTERVAL;
        }
    }
    #endregion

    private IEnumerator InitPools()
    {
        Board = new Board();
        EnemyMgr = new EnemyMgr();

        foreach (var date in GameStaticValue.DIC_EMENY)
        {
            BaseCharacters character = Resources.Load<BaseCharacters>(date.Value);
            ObjectPoolMgr.Instance.InitPools(date.Key, character);
        }

        yield return null;

        while (ObjectPoolMgr.Instance.MakePoolCount != 0) 
        {
            yield return null;
        }
        IsLoadingObjectPool = true;
        IsInited = true;
    }

    public void AddBoardObserver(IObserver observer)
    {
        Board.RegistObserver(observer);
    }

    public void RemoveBoardObserver(IObserver observer)
    {
        Board.RemoveObserver(observer);
    }

    #region block_controll
    public void CreateBlock()
    {
        IsSelectBlock = true;
        NowBlock.InitBlock();
    }

    public void SetBlockParent(Transform parent)
    {
        NowBlock.gameObject.transform.parent = transform;
        NowBlock.gameObject.transform.localPosition = Vector3.zero;
    }

    public void Attack()
    {
        for (int i = 0; i < GameStaticValue.BOARD_WIDTH; i++)
        {

        }

        Board.ClearBlocks();
    }

    public Block GetNowBlock()
    {
        if (IsSelectBlock && NowBlock != null)
        {
            return NowBlock;
        }
        else
        {
            return null;
        }
    }
    #endregion

    #region enemy controll
    #region get enemy
    // 퍼포먼스 관련된 이유로 죽은 것은 한번에 처리 따라서 적을 찾을때는 먼저 확인을 해줘야 한다.
    public BaseCharacters GetFirstEnemy()
    {
        if (IsCheckingDied) CheckListEnemy();
        if (ListEnemy.Count == 0) return null;

        BaseCharacters enemy = ListEnemy[0];

        for (int i = 0; i < ListEnemy.Count; i++)
        {
            Vector3 diffNearest = enemy.GetPosition() - CenterPosition.position;
            Vector3 diff = ListEnemy[i].GetPosition() - CenterPosition.position;

            if (diff.x == diffNearest.x)
            {
                if (diff.y * diff.x < diffNearest.y * diffNearest.x)
                {
                    enemy = ListEnemy[i];
                }
            }
            else
            {
                if (diff.x < diffNearest.x)
                {
                    enemy = ListEnemy[i];
                }
            }
        }

        return enemy;
    }

    // 퍼포먼스 관련된 이유로 죽은 것은 한번에 처리 따라서 적을 찾을때는 먼저 확인을 해줘야 한다.
    public List<BaseCharacters> GetAllEnemy()
    {
        if (IsCheckingDied) CheckListEnemy();
        return ListEnemy;
    }

    // 퍼포먼스 관련된 이유로 죽은 것은 한번에 처리 따라서 적을 찾을때는 먼저 확인을 해줘야 한다.
    public BaseCharacters GetRandomEnemy()
    {
        if (IsCheckingDied) CheckListEnemy();
        if (ListEnemy.Count > 0)
        {
            int rand = Random.Range(0, ListEnemy.Count);
            return ListEnemy[rand];
        }
        else
        {
            return null;
        }
    }
    #endregion

    public void DiedEnmey()
    {
        IsCheckingDied = true;
    }

    // 죽은 것은 한번에 처리해야한다. (퍼포먼스 관련 --> 모든 몬스터가 한번에 다 죽었을 경우 n^3이기 때문. 한번에 처리하면 n^2)
    private void CheckListEnemy()
    {
        IsCheckingDied = false;

        for (int i = 0; i < ListEnemy.Count; i++)
        {
            if (ListEnemy[i].GetHp() <= 0L)
            {
                ListEnemy.RemoveAt(i);
                i--;
            }
        }
    }

    private void ResponeEnemy()
    {
        string now = "";

        if (IndexEnemy < Mathf.CeilToInt(GameStaticValue.SPECIAL_ENEMY_INTERVAL * Diff))
        {
            now = "normal";
        }
        else
        {
            IndexEnemy = 0;
            now = GameStaticValue.SPECIAL_ENEMY_ID[SpecialIndex++];

            if (SpecialIndex >= GameStaticValue.SPECIAL_ENEMY_ID.Length)
            {
                SpecialIndex = 0;
            }
        }

        BaseCharacters enemy = EnemyMgr.GetChar(now);
        ListEnemy.Add(enemy);
    }
    #endregion

    #region game controll
    public void AddLife(int value)
    {
        Life += value;

        if (Life <= 0)
        {
            EndGame();
        }
        else if (Life >= GameStaticValue.MAX_LIFE)
        {
            Life = GameStaticValue.MAX_LIFE;
        }
    }

    public void EndGame()
    {

    }
    #endregion
}