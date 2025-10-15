using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneGame : Singleton<SceneGame>
{
    public List<SpriteRenderer> ObjBg;

    public Sprite AquaBg;
    public Sprite RaceRepeatBg;
    public Sprite RaceFinishBg;

    private float Height;
    private float LeftLength;

    private PoolData<InAquaFish> PoolFish;

    protected override void SetDataInAwake()
    {
        SetBg(AquaBg);
        PoolFish = new PoolData<InAquaFish>(GameStaticValue.FishPath);
        SetActiveLobbyFish(true);
    }

    public void SetBg(Sprite sprite)
    {
        if (sprite == null)
        {
            foreach (SpriteRenderer r in ObjBg)
            {
                r.gameObject.SetActive(false);
            }
        }
        else
        {
            // 카메라의 높이 계산 (Orthographic)
            float worldScreenHeight = Camera.main.orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight * Screen.width / Screen.height;
            int idx = 0;

            foreach (SpriteRenderer r in ObjBg)
            {
                r.gameObject.SetActive(true);
                r.sprite = sprite;

                // 스프라이트 크기 (World 단위)
                float spriteWidth = r.sprite.bounds.size.x;
                float spriteHeight = r.sprite.bounds.size.y;

                float scaleX = worldScreenWidth / spriteWidth;

                transform.localScale = new Vector3(scaleX, scaleX, 1);

                Height = spriteHeight * scaleX;
                LeftLength = (worldScreenHeight - Height) / 2;
                r.transform.position = new Vector3(0, (Height - 0.01f) * idx++ - LeftLength);
            }
        }
    }

    public void MoveBg(float speed)
    {
        foreach (SpriteRenderer r in ObjBg)
        {
            Vector3 before = r.transform.position;
            before.y -= speed * 0.5f;

            if (before.y < -Height - LeftLength)
            {
                before.y += Height * ObjBg.Count;
            }

            r.transform.position = before;
        }
    }

    public void SetLastBg(Sprite sprite)
    {
        SpriteRenderer rend = null;

        foreach (SpriteRenderer r in ObjBg)
        {
            if (rend == null)
            {
                rend = r;
            }
            else
            {
                if (rend.transform.position.y < r.transform.position.y)
                {
                    rend = r;
                }
            }
        }

        rend.sprite = sprite;
    }

    public async void SetActiveLobbyFish(bool isActive)
    {
        if (isActive)
        {
            if (PoolFish.GetNowList().Count > 0)
            {
                return;
            }

            foreach (string key in UpgradeMgr.Instance.GetHasFishes())
            {
                InAquaFish newFish = await PoolFish.GetNew();
                newFish.Init(key, false);
                PoolFish.Add(newFish);
            }
        }
        else
        {
            PoolFish.RemoveAll();
        }
    }
}

