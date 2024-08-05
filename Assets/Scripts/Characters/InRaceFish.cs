using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class InRaceFish : MonoBehaviour
{
    private FishData Data;

    public void InitData(FishData data)
    {
        Data = data;
    }
}
