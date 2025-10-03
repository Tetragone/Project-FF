using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAquaFish : MonoBehaviour
{
    public Transform Position;
    public Rigidbody2D Body;
    public SpriteRenderer SpriteFish;
    public Animator MoveController;

    private FishData Data;
    
    private Sprite AdultSpirte = null;
    private Sprite ChildSpirte = null;

    private bool IsInit = false;
    private float RandomMoveTime;
    private float RandomMoveSpeed;
    private Vector3 NowDir;

    private float StarveTime;
    private float MoveTimer = 0f;
    private bool IsMoving = false;


    public void Init(string fid)
    {
        Body.velocity = new Vector2(0, 0);
        Position.rotation = Quaternion.Euler(0, 0, 0);
        AdultSpirte = AtlasMgr.Instance.GetFishesSprite(string.Format("{0}_adult", TableMgr.GetTableString("fish", fid, "res")));
        ChildSpirte = AtlasMgr.Instance.GetFishesSprite(string.Format("{0}_child", TableMgr.GetTableString("fish", fid, "res")));
        SpriteFish.sprite = ChildSpirte;

        RandomMoveTime = 0f;
        MoveTimer = 0f;
        IsMoving = true;
        IsInit = true;
        if (Data == null)
        {
            Data = new FishData();
        }
        Data.SetDataInit(fid);
    }

    public FishData GetNowData()
    {
        return Data;
    } 

    public Vector3 GetPosition()
    {
        return Position.position;
    }

    private void Update() 
    {
        if (IsInit)
        {
            Food food = AquaMgr.Instance.GetClosestFood(transform.position);

            StarveTime -= Time.deltaTime;

            if (food == null || StarveTime > 0f)
            {
                RandomMoveTime -= Time.deltaTime;

                if (RandomMoveTime < 0f)
                {
                    RandomMoveTime = UnityEngine.Random.Range(GameStaticValue.FishRandomMoveMinTime, GameStaticValue.FishRandomMoveMaxTime);
                    RandomMoveSpeed = UnityEngine.Random.Range(GameStaticValue.FishRandomMoveMinSpeed, GameStaticValue.FishRandomMoveMaxSpeed);
                    int angle = UnityEngine.Random.Range(0, 360);

                    // 각도 계산을 편하게 하기 위해 sin, cos으로 계산
                    Move(new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle)), RandomMoveSpeed);
                    FilpObejct(angle > 180);
                } 
                else
                {
                    if (IsChangeMove())
                    {
                        RandomMoveTime = 0f;
                    }
                }
            }
            else
            {
                RandomMoveTime = 0f;
                Vector3 dir = food.transform.position - transform.position;
                dir = new Vector3(dir.x, dir.y, 0);
                if (dir.magnitude < GameStaticValue.FoodEatRange)
                {
                    Data.EatFood(food);

                    if (Data.IsMaxGrow())
                    {
                        StarveTime = GameStaticValue.AdultStarveTime;
                        SpriteFish.sprite = AdultSpirte;
                        transform.localScale = new Vector3(GameStaticValue.FishMaxGrowSize, GameStaticValue.FishMaxGrowSize);
                    } 
                    else
                    {
                        StarveTime = GameStaticValue.ChildStarveTime;
                        transform.localScale = new Vector3(GameStaticValue.FishChildSize, GameStaticValue.FishChildSize);
                    }
                }
                else
                {
                    Move(dir, 10f);
                    FilpObejct(NowDir.x < 0);
                }
            } 

            if (MoveController.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                IsMoving = false;
                MoveTimer = 0f;
            }
            else
            {
                MoveTimer += Time.deltaTime;

                if (MoveTimer > GameStaticValue.MoveCooldown)
                {
                    IsMoving = true;
                    MoveController.Rebind();
                }
            }
        }
    }

    private void FilpObejct(bool isFilp)
    {
        SpriteFish.flipX = !isFilp;
    }

    // 움직임을 자연스럽게 하기 위한 방법이 필요함. normalized된 값으로 바꾼다.
    private void Move(Vector3 dir, float speed = 1f)
    {
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
        Body.velocity = IsMoving ? dir.normalized * speed * GameStaticValue.MoveSpeedUp : dir.normalized * speed;
    }

    private void OnDisable()
    {
        IsInit = false;
    }

    private bool IsChangeMove()
    {
        bool result = false;

        result = Mathf.Abs(transform.position.x) > CameraMgr.CameraSize * GameStaticValue.NonWhiteSpaceOnX && NowDir.x * transform.position.x > 0;
        result = (transform.position.y > CameraMgr.CameraSize * GameStaticValue.FishMaxYPercent && NowDir.y > 0) || result;
        result = (transform.position.y < CameraMgr.CameraSize * GameStaticValue.FishMinYPercent && NowDir.y < 0) || result;

        return result;
    }
}