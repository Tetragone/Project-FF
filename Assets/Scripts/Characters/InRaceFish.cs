using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class InRaceFish : MonoBehaviour
{
    public SpriteRenderer SpriteFish;
    public Animator FishAniController;

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

        SpriteFish.sprite = AtlasMgr.Instance.GetFishesSprite(string.Format("{0}_adult", TableMgr.GetTableString("fish", data.Fid, "res")));
        SpriteFish.transform.rotation = TableMgr.GetTableInt("fish", data.Fid, "is_rotated") == 1 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, -90);

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

        if (raceFish != null && IsMy)
        {
            // 걍 같으면 이기는걸로
            if (raceFish.GetData().Size > Data.Size)
            {
                RaceMgr.Instance.MakeEndPopup();
                Die();
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

        if (FishAniController.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && RaceMgr.Instance.IsStart)
        {
            FishAniController.Rebind();
        }
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
