using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoBehaviour
{
    public static float CameraSize = 1f;

    private void Awake()
    {
        float beforeSize = Camera.main.orthographicSize;
        // 현재 카메라의 종횡비 계산
        float screenAspect = (float)Screen.width / (float)Screen.height;

        // 카메라의 orthographicSize 설정
        Camera.main.orthographicSize = beforeSize / screenAspect;
        CameraSize = beforeSize;
    }
}
