using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAquaFish : MonoBehaviour
{
    public Transform Position;
    public Rigidbody2D Body;

    private FishData Data;
    private bool IsInit = false;
    private float RandomMoveTime;
    private float RandomMoveSpeed;

    public void Init(string eid)
    {
        if (eid == "")
        {
            return;
        }

        Body.velocity = new Vector2(0, 0);
        Position.rotation = Quaternion.Euler(0, 0, 0);
        RandomMoveTime = 0f;
        IsInit = true;
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

            if (food == null)
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
            }
            else
            {
                Vector3 dir = food.transform.position - transform.position;
                if (dir.magnitude < GameStaticValue.FoodEatRange)
                {
                    Data.EatFood(food);
                }
                else
                {
                    Move(dir);
                    FilpObejct(dir.x > 0);
                }
            } 
        }
    }

    private void FilpObejct(bool isFilp)
    {

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

        Body.velocity = dir.normalized * speed;
    }

    private void OnDisable()
    {
        IsInit = false;
    }
}