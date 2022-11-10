using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public static int Points { get; private set; }
    public bool isGameStarted { get; private set; }

    [SerializeField]
    private TextMeshProUGUI gamePoint;
    [SerializeField]
    private TextMeshProUGUI pointText;
    [SerializeField]
    private GameObject gameResultPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Start()
    {
        GameStart();
    }
    public void GameStart()
    {
        gameResultPanel.SetActive(false);
        gamePoint.text = string.Empty;
        isGameStarted = true;
        SetPoints(0);
        FieldScript.Instance.RegenerateField();

    }
    public void AddPoints(int points)
    {
        SetPoints(Points + points);
    }
    public void SetPoints(int points)
    {
        Points = points;
        pointText.text = Points.ToString();


    }
    public void Lose()
    {
        gameResultPanel.SetActive(true);
        isGameStarted = false;
        gamePoint.text = $"You lost\nYour result{Points}";
    }
}
