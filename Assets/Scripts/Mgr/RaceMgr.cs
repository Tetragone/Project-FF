using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceMgr : Singleton<RaceMgr> {
    [HideInInspector] public Board Board;
    [HideInInspector] public Block NowBlock;
    [HideInInspector] public List<BaseCharacters> ListEnemy = new List<BaseCharacters>();
    // Enemy���� �Ÿ� ����� ���ؼ� � ���� ���� �ⱸ�� ������ �ִ��� ���.
    public Transform CenterPosition;

    public EnemyMgr EnemyMgr { get; private set; }
    public bool IsSelectBlock { get; private set; } = true;
    public bool IsLoadingObjectPool { get; set; } = false;
    public int Life { get; private set; } = GameStaticValue.MAX_LIFE;

    private bool IsCheckingDied = false;
    private bool IsInited = false;
    private float EnemyCreateTimer = 0.0f;
    private int IndexEnemy = 0;
    private int SpecialIndex = 0;
    private float Diff = 1f;

    #region Unity Function
    // awake�� �ϸ� instance�� �������� �ִ�. 
    private void Start() {
        StartCoroutine(InitPools());
    }

    private void Update() {
        if (!IsInited) {
            return;
        }

        if (IsCheckingDied) {
            CheckListEnemy();
        }

        if (EnemyCreateTimer <= 0.0f) {
            ResponeEnemy();
            EnemyCreateTimer += GameStaticValue.ENEMY_INTERVAL;
        }
    }
    #endregion

    private IEnumerator InitPools() {
        Board = new Board();
        EnemyMgr = new EnemyMgr();

        foreach (var date in GameStaticValue.DIC_EMENY) {
            BaseCharacters character = Resources.Load<BaseCharacters>(date.Value);
            ObjectPoolMgr.Instance.InitPools(date.Key, character);
        }

        yield return null;

        while (ObjectPoolMgr.Instance.MakePoolCount != 0) {
            yield return null;
        }
        IsLoadingObjectPool = true;
        IsInited = true;
    }

    public void AddBoardObserver(IObserver observer) {
        Board.RegistObserver(observer);
    }

    public void RemoveBoardObserver(IObserver observer) {
        Board.RemoveObserver(observer);
    }

    #region block_controll
    public void CreateBlock() {
        IsSelectBlock = true;
        NowBlock.InitBlock();
    }

    public void SetBlockParent(Transform parent) {
        NowBlock.gameObject.transform.parent = transform;
        NowBlock.gameObject.transform.localPosition = Vector3.zero;
    }

    public void Attack() {
        for (int i = 0; i < GameStaticValue.BOARD_WIDTH; i++) {

        }

        Board.ClearBlocks();
    }

    public Block GetNowBlock() {
        if (IsSelectBlock && NowBlock != null) {
            return NowBlock;
        } else {
            return null;
        }
    }
    #endregion

    #region enemy controll
    #region get enemy
    // �����ս� ���õ� ������ ���� ���� �ѹ��� ó�� ���� ���� ã������ ���� Ȯ���� ����� �Ѵ�.
    public BaseCharacters GetFirstEnemy() {
        if (IsCheckingDied)
            CheckListEnemy();
        if (ListEnemy.Count == 0)
            return null;

        BaseCharacters enemy = ListEnemy[0];

        for (int i = 0; i < ListEnemy.Count; i++) {
            Vector3 diffNearest = enemy.GetPosition() - CenterPosition.position;
            Vector3 diff = ListEnemy[i].GetPosition() - CenterPosition.position;

            if (diff.x == diffNearest.x) {
                if (diff.y * diff.x < diffNearest.y * diffNearest.x) {
                    enemy = ListEnemy[i];
                }
            } else {
                if (diff.x < diffNearest.x) {
                    enemy = ListEnemy[i];
                }
            }
        }

        return enemy;
    }

    // �����ս� ���õ� ������ ���� ���� �ѹ��� ó�� ���� ���� ã������ ���� Ȯ���� ����� �Ѵ�.
    public List<BaseCharacters> GetAllEnemy() {
        if (IsCheckingDied)
            CheckListEnemy();
        return ListEnemy;
    }

    // �����ս� ���õ� ������ ���� ���� �ѹ��� ó�� ���� ���� ã������ ���� Ȯ���� ����� �Ѵ�.
    public BaseCharacters GetRandomEnemy() {
        if (IsCheckingDied)
            CheckListEnemy();
        if (ListEnemy.Count > 0) {
            int rand = Random.Range(0, ListEnemy.Count);
            return ListEnemy[rand];
        } else {
            return null;
        }
    }
    #endregion

    public void DiedEnmey() {
        IsCheckingDied = true;
    }

    // ���� ���� �ѹ��� ó���ؾ��Ѵ�. (�����ս� ���� --> ��� ���Ͱ� �ѹ��� �� �׾��� ��� n^3�̱� ����. �ѹ��� ó���ϸ� n^2)
    private void CheckListEnemy() {
        IsCheckingDied = false;

        for (int i = 0; i < ListEnemy.Count; i++) {
            if (ListEnemy[i].GetHp() <= 0L) {
                ListEnemy.RemoveAt(i);
                i--;
            }
        }
    }

    private void ResponeEnemy() {
        string now = "";

        if (IndexEnemy < Mathf.CeilToInt(GameStaticValue.SPECIAL_ENEMY_INTERVAL * Diff)) {
            now = "normal";
        } else {
            IndexEnemy = 0;
            now = GameStaticValue.SPECIAL_ENEMY_ID[SpecialIndex++];

            if (SpecialIndex >= GameStaticValue.SPECIAL_ENEMY_ID.Length) {
                SpecialIndex = 0;
            }
        }

        BaseCharacters enemy = EnemyMgr.GetChar(now);
        ListEnemy.Add(enemy);
    }
    #endregion

    #region game controll
    public void AddLife(int value) {
        Life += value;

        if (Life <= 0) {
            EndGame();
        } else if (Life >= GameStaticValue.MAX_LIFE) {
            Life = GameStaticValue.MAX_LIFE;
        }
    }

    public void EndGame() {

    }
    #endregion
}