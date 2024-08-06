using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GameRace : MonoBehaviour
{
    public TextMeshProUGUI TextMeter;

    public GameObject ObjTouchBack;
    public GameObject ObjTouchFront;

    private Vector2 StartPosition;
    private bool IsDragging = false;
    private static float Range = 5f;

    void Update()
    {
        // 다중 터치시에는 첫번째 터치에만 작동하도록 
        if (Input.touchCount >= 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // 터치 시작
                ObjTouchBack.SetActive(true);
                ObjTouchBack.transform.position = touch.position;
                ObjTouchFront.transform.localPosition = Vector3.zero;
                IsDragging = true;
                StartPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && IsDragging)
            {
                Vector3 diff = touch.position - StartPosition;
                // 특정 범위를 못 벗어나게 해주는 코드
                if (diff.magnitude > Range)
                {
                    diff = diff.normalized * Range; 
                }
                ObjTouchFront.transform.localPosition = diff;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // 터치가 끝나면 드래깅 종료
                IsDragging = false;
            }
        }
    }
}