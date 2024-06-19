using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacters : PoolableObject
{
    public Transform Position;
    public Rigidbody2D Body;
    #region stat
    private float MoveSpeed = 0f;
    private long Hp = 0L;
    private long MaxHp = 0L;
    private int Damage = 1;
    #endregion
    // 이동과 관련된것은 물리로 처리하자.
    // 방향 전환은 앞에 벽을 두고 벽과 충돌했다면 이동하는 방식으로 진행하자. (좌표계로 계산시에는 오차가 생길수 있기 때문)
    // 적과 충돌하는 것은 벽과 미사일 뿐
    public void Init(string eid)
    {
        if (eid == "")
        {
            return;
        }

        MaxHp = TableMgr.GetTableLong("enemy", eid, "maxHp");
        Hp = MaxHp;
        MoveSpeed = TableMgr.GetTableInt("enemy", eid, "speed");
        Body.velocity = new Vector2(0, MoveSpeed);
        Position.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetPosition(Vector3 position)
    {
        Position.position = position;
    }

    public Vector3 GetPosition()
    {
        return Position.position;
    }

    public void SetDamage(double damage)
    {
        Hp -= (long)damage;

        if (Hp < 0)
        {
            GameMgr.Instance.DiedEnmey();
            Push();
        }
    }

    public long GetHp()
    {
        return Hp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wall"))
        {
            Position.rotation = Quaternion.Euler(0, 0, Position.rotation.z - 90);
            Vector2 vel = Body.velocity;
            Body.velocity = new Vector2(vel.y, -vel.x);
        }
        else if (collision.CompareTag("end_point")) 
        {
            GameMgr.Instance.AddLife(-Damage);
        }
    }
}