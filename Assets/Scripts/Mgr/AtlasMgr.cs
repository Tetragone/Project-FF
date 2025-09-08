using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasMgr : SingletonAllSecen<AtlasMgr>
{

    [SerializeField] private SpriteAtlas Fishes;
    [SerializeField] private SpriteAtlas Relics;
    [SerializeField] private Sprite[] Common;

    private Dictionary<string, Sprite> DicCommon = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> DicFish = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> DicRelics = new Dictionary<string, Sprite>();

    public Sprite GetCommonSprite(string path)
    {
        if (!DicCommon.ContainsKey(path))
        {
            Sprite sprite = null;

            foreach (Sprite item in Common)
            {
                if (item.name == path)
                {
                    sprite = item;
                    break;
                }
            }

            if (sprite == null)
            {
                return null;
            }

            DicCommon.Add(path, sprite);
        }

        return DicCommon[path];
    }

    public Sprite GetFishesSprite(string path)
    {
        if (!DicFish.ContainsKey(path))
        {
            Sprite sprite = Fishes.GetSprite(path);

            if (sprite == null)
            {
                return null;
            }

            DicFish.Add(path, sprite);
        }

        return DicFish[path];
    }

    public Sprite GetRelicsSprite(string path)
    {
        if (!DicRelics.ContainsKey(path))
        {
            Sprite rel = Relics.GetSprite(path);

            if (rel == null)
            {
                return null;
            }

            DicRelics.Add(path, rel);
        }

        return DicRelics[path];
    }
}
