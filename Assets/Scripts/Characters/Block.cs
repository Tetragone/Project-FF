using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : PoolableObject
{
    // 아마도 고정으로 할듯? 하지만 나중에 수정될수도 있으니 이렇게
    public int BlockCount { get; set; } = 4;
    public BlockType Type { get; set; } = BlockType.O;
    public BlockRotataion Rotataion { get; set; } = BlockRotataion.up;
    // Color 값의 경우는 그냥 어디선가 가져온 값으로 모양에 따라서 넣어주자.
    public Color BlockColor = Color.black;
    public List<Sqaure> ListBlock = new List<Sqaure>();
    public List<Vector2> ListBlockPosition { get; private set; } = new List<Vector2>();

    public void InitBlock()
    {
        int typeNum = Random.Range(0, System.Enum.GetValues(typeof(BlockType)).Length - 1);
        int rotat = Random.Range(0, System.Enum.GetValues(typeof(BlockRotataion)).Length);

        Type = (BlockType)typeNum;
        Rotataion = (BlockRotataion)rotat;

        SetPosition();
        for (int i = 0; i < rotat; i++)
        {
            SetRotation();
        }
    }

    private void SetPosition()
    {
        int x = 0;
        int y = 0;

        switch(Type)
        {
            case BlockType.I:
                ListBlockPosition.Add(new Vector2(x, y + 1));
                ListBlockPosition.Add(new Vector2(x, y));
                ListBlockPosition.Add(new Vector2(x, y - 1));
                ListBlockPosition.Add(new Vector2(x, y - 2));
                break;
            case BlockType.O:
                ListBlockPosition.Add(new Vector2(x, y));
                ListBlockPosition.Add(new Vector2(x, y + 1));
                ListBlockPosition.Add(new Vector2(x + 1, y + 1));
                ListBlockPosition.Add(new Vector2(x + 1, y));
                break;
            case BlockType.L:
                ListBlockPosition.Add(new Vector2(x, y));
                ListBlockPosition.Add(new Vector2(x + 1, y));
                ListBlockPosition.Add(new Vector2(x, y + 1));
                ListBlockPosition.Add(new Vector2(x, y + 2));
                break;
            case BlockType.J:
                ListBlockPosition.Add(new Vector2(x, y));
                ListBlockPosition.Add(new Vector2(x - 1, y));
                ListBlockPosition.Add(new Vector2(x, y + 1));
                ListBlockPosition.Add(new Vector2(x, y + 2));
                break;
            case BlockType.Z:
                ListBlockPosition.Add(new Vector2(x, y));
                ListBlockPosition.Add(new Vector2(x - 1, y));
                ListBlockPosition.Add(new Vector2(x, y - 1));
                ListBlockPosition.Add(new Vector2(x + 1, y - 1));
                break;
            case BlockType.S:
                ListBlockPosition.Add(new Vector2(x, y));
                ListBlockPosition.Add(new Vector2(x + 1, y));
                ListBlockPosition.Add(new Vector2(x, y - 1));
                ListBlockPosition.Add(new Vector2(x - 1, y - 1));
                break;
            case BlockType.T:
                ListBlockPosition.Add(new Vector2(x, y));
                ListBlockPosition.Add(new Vector2(x + 1, y));
                ListBlockPosition.Add(new Vector2(x - 1, y));
                ListBlockPosition.Add(new Vector2(x, y - 1));
                break;
        }
    }

    private void SetRotation()
    {
        for (int i = 0; i < ListBlockPosition.Count; i++)
        {
            float x = ListBlockPosition[i].x;
            float y = ListBlockPosition[i].y;

            Vector2 newValue = new Vector2(y, -x);
            ListBlockPosition[i] = newValue;
        }
    }
}
