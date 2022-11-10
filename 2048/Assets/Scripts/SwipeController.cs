using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    
    public static event Action<Vector2> SwipeEvent;
    public static event Action OnSwipeEvent;

    private Vector2 tapPosition;
    private Vector2 swipeDelta;
    private float deadZone = 80;

    private bool isSwiping;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount>0)
        {
            if(Input.GetTouch(0).phase==TouchPhase.Began)
            {
                isSwiping = true;
                tapPosition = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Canceled|| Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                ResetSwipe();
            }
        }
        CheckSwipe();
    }
    private void CheckSwipe()
    {
        swipeDelta = Vector2.zero;
        if (isSwiping)
        {
            if (Input.touchCount > 0)
                swipeDelta = Input.GetTouch(0).position - tapPosition;
        }
        if (swipeDelta.magnitude>deadZone)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                SwipeEvent?.Invoke(swipeDelta.x > 0 ? Vector2.right : Vector2.left);
                OnSwipeEvent?.Invoke();
            }
            else
            {
                SwipeEvent?.Invoke(swipeDelta.y > 0 ? Vector2.up : Vector2.down);
                OnSwipeEvent?.Invoke();
            }
            ResetSwipe();
        }
        
    }
    private void ResetSwipe()
    {
        isSwiping = false;
        tapPosition = Vector2.zero;
        swipeDelta = Vector2.zero;
    }
}
