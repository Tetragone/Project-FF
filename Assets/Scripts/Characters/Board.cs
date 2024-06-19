using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : ISubject
{
    public List<IObserver> ListObservers = new List<IObserver>();

    public List<List<BlockType>> ListBlock = new List<List<BlockType>>();
    private List<Vector2> ListChangedBlock = new List<Vector2>();

    public Board()
    {
        for (int i = 0; i < GameStaticValue.BOARD_WIDTH; i++)
        {
            ListBlock.Add(new List<BlockType>(GameStaticValue.BOARD_HEIGTH));
        }

        ClearBlocks();
    }

    public void ClearBlocks()
    {
        for (int i = 0; i < ListBlock.Count; i++)
        {
            for (int j = 0; j < ListBlock[i].Count; j++)
            {
                ListBlock[i][j] = BlockType.none;
            }
        }
    }

    public int GetBlocksCountAll()
    {
        int count = 0;

        for (int i = 0; i < GameStaticValue.BOARD_WIDTH; i++)
        {
            count += GetBlocksCount(i);
        }

        return count;
    } 

    public int GetBlocksCount(int vertiNum)
    {
        if (ListBlock.Count > vertiNum)
        {
            return 0;
        }
        else
        {
            int count = 0;

            for (int i = 0; i < GameStaticValue.BOARD_HEIGTH; i++)
            {
                if (ListBlock[vertiNum][i] != BlockType.none) count++;
            }

            return count;
        }
    }

    public BlockType[][] GetBlocks()
    {
        BlockType[][] blocks = new BlockType[GameStaticValue.BOARD_WIDTH][];

        for (int i = 0; i < GameStaticValue.BOARD_WIDTH; i++)
        {
            blocks[i] = ListBlock[i].ToArray();
        }

        return blocks;
    }

    public bool IsValidPosition(Vector2 position, Block block)
    {
        if (block == null)
        {
            return false;
        }

        for (int i = 0; i < block.ListBlock.Count; i++)
        {
            int x = (int)(position.x + block.ListBlockPosition[i].x);
            int y = (int)(position.y + block.ListBlockPosition[i].y);

            if (!IsValidX(x) || !IsValidY(y) || ListBlock[y][x] != BlockType.none)
            {
                return false;
            }
        }

        return true;
    }

    public void ChangeSelectedBlockColor(Vector2 position, Block block)
    {
        if (block == null)
        {
            return;
        }

        if (ListChangedBlock.Count != 0)
        {
            CancleSelect();
        }

        for (int i = 0; i < block.ListBlock.Count; i++)
        {
            int x = (int)(position.x + block.ListBlockPosition[i].x);
            int y = (int)(position.y + block.ListBlockPosition[i].y);
            ListBlock[y][x] = block.Type;
            ListChangedBlock.Add(new Vector2(x, y));
        }

        UpdateObserver();
    }

    public void CancleSelect()
    {
        for (int i = 0; i < ListChangedBlock.Count; i++)
        {
            ListBlock[(int)ListChangedBlock[i].y][(int)ListChangedBlock[i].x] = BlockType.none;
        }

        ListChangedBlock.Clear();
    }

    public void FilledSelectedBlock()
    {
        ListChangedBlock.Clear();
    }

    private bool IsValidX(int x)
    {
        if (x >= 0 && x < GameStaticValue.BOARD_WIDTH) return true;
        else return false;
    }

    private bool IsValidY(int y)
    {
        if (y >= 0 && y < GameStaticValue.BOARD_HEIGTH) return true;
        else return false;
    }

    public void UpdateObserver()
    {
        for (int i = 0; i < ListObservers.Count; i++)
        {
            ListObservers[i].UpdateObserver();
        }
    }

    public void RegistObserver(IObserver obverser)
    {
        ListObservers.Add(obverser);
    }

    public void RemoveObserver(IObserver obverser)
    {
        for (int i = 0; i < ListObservers.Count; i++)
        {
            if (ListObservers[i] == obverser)
            {
                ListObservers.RemoveAt(i);
                return;
            }
        }
    }
}
