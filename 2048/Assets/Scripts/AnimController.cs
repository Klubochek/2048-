using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimController : MonoBehaviour
{
    [SerializeField]
    private CellsAnimation animPref;

    public static AnimController Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DOTween.Init();
    }
    public void SmoothTrans (Cell from,Cell to,bool isMerging)
    {
        Instantiate(animPref, transform, false).Move(from, to, isMerging);
    }

    public void SmoothAppear(Cell cell)
    {
        Instantiate(animPref, transform, false).Appear(cell);
    }
}
