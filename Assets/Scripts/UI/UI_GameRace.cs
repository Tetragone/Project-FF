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
    private static float Range = 100f;

    private void Update()
    {
        // 다중 터치시에는 첫번째 터치에만 작동하도록 
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                TouchStart(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved && IsDragging)
            {
                Touching(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                TouchEnd();
            }
        }


        // Unity에서 마우스 입력 받기 위함.
        if (Input.GetMouseButtonDown(0))
        {
            TouchStart(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && IsDragging)
        {
            Touching(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            TouchEnd();
        }

        TextMeter.text = RaceMgr.Instance.GetMeterToString();
    }

    private void TouchStart(Vector2 position)
    {
        ObjTouchBack.SetActive(true);
        ObjTouchBack.transform.position = position;
        ObjTouchFront.transform.localPosition = Vector3.zero;
        IsDragging = true;
        StartPosition = position;
    }

    private void Touching(Vector2 position)
    {
        Vector3 diff = position - StartPosition;
        // 특정 범위를 못 벗어나게 해주는 코드
        if (diff.magnitude > Range)
        {
            diff = diff.normalized * Range;
        }
        ObjTouchFront.transform.localPosition = diff;
        RaceMgr.Instance.SetMyFishDir(diff);
    }

    private void TouchEnd()
    {
        IsDragging = false;
        ObjTouchBack.SetActive(false);
    }
}