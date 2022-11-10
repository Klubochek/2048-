using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    private void Awake()
    {
        SwipeController.OnSwipeEvent+= SwipeController_SwipeEvent;
    }

    private void SwipeController_SwipeEvent()
    {
        print("1");
        audioSource.Play();
    }
}
