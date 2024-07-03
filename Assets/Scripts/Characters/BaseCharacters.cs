using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacters : PoolableObject
{
    public Transform Position;
    public Rigidbody2D Body;

    #region stat
    private float MoveSpeed = 0f;
    #endregion

    public void Init(string eid)
    {
        if (eid == "")
        {
            return;
        }

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

    private void Update() 
    {
        
    }
}