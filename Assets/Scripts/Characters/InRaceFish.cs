using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class InRaceFish : MonoBehaviour
{
    private Rigidbody2D Rigid;
    private CircleCollider2D Collider;
    private FishData Data;

    public void InitData(FishData data, bool isMy)
    {
        if (Rigid == null)
        {
            Rigid = GetComponent<Rigidbody2D>();
        }

        if (Collider == null)
        {
            Collider = GetComponent<CircleCollider2D>();
        }

        Data = data;
        gameObject.layer = isMy ? GameStaticValue.MyFishLayer : GameStaticValue.EnemyFishLayer;
    }

    public FishData GetData()
    {
        return Data;
    }

    public void Die()
    {
        RaceMgr.Instance.DisableFish(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InRaceFish raceFish = collision.GetComponent<InRaceFish>();

        if (raceFish != null)
        {
            // 걍 같으면 이기는걸로
            if (raceFish.GetData().Size > Data.Size)
            {
                RaceMgr.Instance.EndGame();
            } 
            else
            {
                Data.EatFish(raceFish.GetData());
                raceFish.Die();
            }
        }
    }
}
