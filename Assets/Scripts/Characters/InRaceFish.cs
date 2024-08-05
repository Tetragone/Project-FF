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
        Data = data;
    }


}
