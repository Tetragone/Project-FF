using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Board : MonoBehaviour, IObserver
{
    #region inspector
    [Header("라인 안에서의 오브젝트를 넣는 순서도 중요하다, 아래부터 0번째 위로 갈수록 높은 숫자가 필요.")]
    public List<Sqaure> VerticalLine1;
    public List<Sqaure> VerticalLine2;
    public List<Sqaure> VerticalLine3;
    public List<Sqaure> VerticalLine4;
    public List<Sqaure> VerticalLine5;
    #endregion

    // 사용의 편의성을 위한 변수
    public List<List<Sqaure>> ListBlock = new List<List<Sqaure>>();

    private void Awake()
    {
        ListBlock.Add(VerticalLine1);
        ListBlock.Add(VerticalLine2);
        ListBlock.Add(VerticalLine3);
        ListBlock.Add(VerticalLine4);
        ListBlock.Add(VerticalLine5);

        InitButtonCallback();
    }

    private void InitButtonCallback()
    {
        for (int i = 0; i < ListBlock.Count; i++)
        {
            for (int j = 0; j < ListBlock[i].Count; j++)
            {
                int tempI = i;
                int tempJ = j;

                ListBlock[i][j].ButtonBlockSelet.onClick.AddListener(() =>
                {
                    if (AquaMgr.Instance.Board.IsValidPosition(new Vector2(tempI, tempJ), AquaMgr.Instance.GetNowBlock()))
                    {
                        AquaMgr.Instance.Board.ChangeSelectedBlockColor(new Vector2(tempI, tempJ), AquaMgr.Instance.GetNowBlock());
                    }
                });
            }
        }
    }

    public void UpdateObserver()
    {
        BlockType[][] blocks = AquaMgr.Instance.Board.GetBlocks();
        for (int i = 0; i < blocks.Length; i++)
        {
            for (int j = 0; j < blocks[i].Length; j++)
            {
                if (blocks[i][j] != BlockType.none)
                {
                    ListBlock[i][j].IsFilled = true;
                    ListBlock[i][j].ColorBlock = GameStaticValue.LIST_COLOR_BLOCK[(int)blocks[i][j]];
                }
                else
                {
                    ListBlock[i][j].IsFilled = false;
                    ListBlock[i][j].ColorBlock = GameStaticValue.BASE_COLOR;

                    if (AquaMgr.Instance.IsSelectBlock)
                    {
                        ListBlock[i][j].ButtonBlockSelet.interactable
                            = AquaMgr.Instance.Board.IsValidPosition(new Vector2(i, j), AquaMgr.Instance.GetNowBlock());
                    }
                }
            }
        }
    }

}
