using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceMgr : Singleton<RaceMgr> {
    [HideInInspector] public Board Board;
    [HideInInspector] public Block NowBlock;
    [HideInInspector] public List<InAquaFish> ListEnemy = new List<InAquaFish>();
    // Enemy와의 거리 계산을 통해서 어떤 적이 가장 출구에 가까이 있는지 계산.
    public Transform CenterPosition;

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
    // awake로 하면 instance가 없을수도 있다. 
    private void Start() {
        StartCoroutine(InitPools());
    }

    private void Update() {

    }
    #endregion

    private IEnumerator InitPools() {
        Board = new Board();

        foreach (var date in GameStaticValue.DIC_EMENY) {
            InAquaFish character = Resources.Load<InAquaFish>(date.Value);
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
    #endregion

    #region enemy controll
    #region get enemy
    #endregion

    public void DiedEnmey() {
        IsCheckingDied = true;
    }


    private void ResponeEnemy() {

    }
    #endregion

    #region game controll
    public void AddLife(int value) {

    }

    public void EndGame() {

    }
    #endregion
}