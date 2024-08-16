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
    private bool IsMy = false;
    private Vector3 NowDir;

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
        IsMy = isMy;
        if (!IsMy)
        {
            SetMove(new Vector3(0, data.Speed - RaceMgr.Instance.GameBaseSpeed(), 0));
        }
    }

    private void SetMove(Vector3 vel)
    {
        Rigid.velocity = vel;
    }

    public void SetDirForMy(Vector3 dir)
    {
        if (!IsMy)
        {
            return;
        }

        dir = dir.normalized;

        if (Mathf.Abs(transform.position.x) > CameraMgr.CameraSize * GameStaticValue.NonWhiteSpaceOnX && dir.x * transform.position.x > 0)
        {
            dir = new Vector3(0, dir.y);
        }

        if (transform.position.y > CameraMgr.CameraSize * GameStaticValue.FishMaxYPercent && dir.y > 0)
        {
            dir = new Vector3(dir.x, 0);
        }

        if (transform.position.y < CameraMgr.CameraSize * GameStaticValue.FishMinYPercent && dir.y < 0)
        {
            dir = new Vector3(dir.x, 0);
        }
        NowDir = dir;

        SetMove(dir * 3f);
    }

    public FishData GetData()
    {
        return Data;
    }

    public void Die()
    {
        RaceMgr.Instance.DisableFish(this);
    }

    public void SetLocalScale(float percent)
    {
        transform.localScale = Vector3.one * percent * GameStaticValue.BaseFishSize;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InRaceFish raceFish = collision.GetComponent<InRaceFish>();

        if (raceFish != null)
        {
            // 걍 같으면 이기는걸로
            if (raceFish.GetData().Size > Data.Size)
            {
                RaceMgr.Instance.MakeEndPopup();
            } 
            else
            {
                Data.EatFish(raceFish.GetData());
                raceFish.Die();
            }
        }
    }

    private void Update()
    {
        if (IsMy && IsChangeMove())
        {
            SetDirForMy(NowDir);
        }

        IsDisablePosition();
    }

    private bool IsChangeMove()
    {
        bool result = false;

        result = Mathf.Abs(transform.position.x) > CameraMgr.CameraSize * GameStaticValue.NonWhiteSpaceOnX && NowDir.x * transform.position.x > 0;
        result = (transform.position.y > CameraMgr.CameraSize * GameStaticValue.FishMaxYPercent && NowDir.y > 0) || result;
        result = (transform.position.y < CameraMgr.CameraSize * GameStaticValue.FishMinYPercent && NowDir.y < 0) || result;

        return result;
    }

    private void IsDisablePosition()
    {
        if (transform.position.y > CameraMgr.CameraSize * GameStaticValue.FishRaceYPercent + 1f)
        {
            this.Die();
        }

        if (transform.position.y < CameraMgr.CameraSize * GameStaticValue.FishRaceYPercent * -1f - 1f)
        {
            this.Die();
        }
    }
}
