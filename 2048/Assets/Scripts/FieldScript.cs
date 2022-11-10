using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class FieldScript : MonoBehaviour
{

    public static FieldScript Instance;

    [Header("Prams")]
    private int cellSize = 175;
    private int offset = 20;
    private int fieldSizeInPx = 800;
    public int fieldSizeInCells = 4;
    public int InitCells = 2;

    private bool anyCellMove;

    [SerializeField]
    private Cell cellpfbs;

    [SerializeField]
    private RectTransform rectTransform;


    private Cell[,] cells;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        SwipeController.SwipeEvent += OnInput;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            OnInput(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D))
            OnInput(Vector2.right);
        if (Input.GetKeyDown(KeyCode.W))
            OnInput(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S))
            OnInput(Vector3.down);
    }

    private void OnInput(Vector2 direction)
    {
        if (!GameController.Instance.isGameStarted)
            return;
        anyCellMove = false;
        ResetCellsFlag();

        Move(direction);
        if (anyCellMove)
        {
            AddRandomCells();
            CheckGameResult();
        }

    }
    private void Move(Vector2 direction)
    {
        int startXY = direction.x > 0 || direction.y < 0 ? fieldSizeInCells - 1 : 0;
        int direc = direction.x != 0 ? (int)direction.x : -(int)direction.y;

        for(int i = 0; i < fieldSizeInCells; i++) 
        {
            for (int j = startXY; j >= 0 && j < fieldSizeInCells; j -= direc)
            {
                var cell = direction.x != 0 ? cells[j, i] : cells[i, j];

                if (cell.IsEmpty)
                    continue;
                var cellToMerge = FindCellToMerge(cell, direction);
                if (cellToMerge != null)
                {
                    cell.MergeWithCell(cellToMerge);
                    anyCellMove = true;
                    continue;
                }
                var emptyCell = FindEmptyCell(cell, direction);
                if (emptyCell!=null)
                {
                    cell.MoveToCell(emptyCell);
                    anyCellMove = true;
                }
            }
        }
    }
    private Cell FindCellToMerge(Cell cell,Vector2 direction)
    {
        int startX = cell.Posx + (int)(direction.x);
        int startY = cell.Posy - (int)(direction.y);

        for(int x=startX,y=startY;
            x>=0&&x<fieldSizeInCells&&
            y>=0&&y<fieldSizeInCells;
            x += (int)direction.x, y -= (int)direction.y)
        {
            if (cells[x, y].IsEmpty)
                continue;
            if (cells[x, y].Value == cell.Value && !cells[x, y].HasMerged)
                return cells[x, y];
            break;
        }
        return null;
    }

    private Cell FindEmptyCell(Cell cell, Vector2 direction)
    {
        Cell emptyCell = null;
        int startX = cell.Posx + (int)(direction.x);
        int startY = cell.Posy - (int)(direction.y);

        for (int x = startX, y = startY;
            x >= 0 && x < fieldSizeInCells &&
            y >= 0 && y < fieldSizeInCells;
            x += (int)direction.x, y -= (int)direction.y)
        {
            if (cells[x, y].IsEmpty)
                emptyCell = cells[x, y];
            else
                break;
        }
        return emptyCell;

    }
    private void CheckGameResult()
    {
        bool lose = true;
        for (int x = 0; x < fieldSizeInCells; x++)
            for (int y = 0; y < fieldSizeInCells; y++)
            {
                if (lose&&
                    cells[x, y].IsEmpty||              
                    FindCellToMerge(cells[x, y], Vector2.left) ||
                    FindCellToMerge(cells[x, y], Vector2.right) ||
                    FindCellToMerge(cells[x, y], Vector2.down) ||
                    FindCellToMerge(cells[x, y], Vector2.up))
                {
                    lose = false;
                }
                
            }
        if (lose)
            GameController.Instance.Lose();
    }
    public void Create()
    {
        cells = new Cell[fieldSizeInCells, fieldSizeInCells];

        // Стратовые позиции х,у. Расчет как половина ширины поля - отступ-половина клетки;
        float startPosX = -fieldSizeInPx / 2 + offset + cellSize / 2;
        float startPosY = fieldSizeInPx / 2 - offset - cellSize / 2;

        for (int x = 0; x < fieldSizeInCells; x++)
        {
            for (int y = 0; y < fieldSizeInCells; y++)
            {
                Cell cell = Instantiate(cellpfbs, transform, false);
                Vector3 cellPosition = new Vector3(startPosX + (x * (cellSize + offset)), startPosY - (y * (cellSize + offset)), 0);

                cell.transform.localPosition = cellPosition;

                cells[x, y] = cell;

                cell.SetValue(x, y, 0);
            }
        }
    }
    public void AddRandomCells()
    {
        List<Cell> emptyCellList = new List<Cell>();
        for (int x = 0; x < fieldSizeInCells; x++)
        {
            for (int y = 0; y < fieldSizeInCells; y++)
            {
                if (cells[x, y].IsEmpty)
                    emptyCellList.Add(cells[x, y]);
            }
        }
        if (emptyCellList.Count == 0)
        {
            throw new System.Exception("gameover");
        }
        else
        {
            // выбираем 2 рандомные клетки из листа пустых, если клеток меньше чем 2 - выбираем 1
            var randcell = emptyCellList[Random.Range(0, emptyCellList.Count)];
            int newValue;
            int propability = Random.Range(1, 10);

            if (propability == 1)
                newValue = 2;
            else
                newValue = 1;

            randcell.SetValue(randcell.Posx, randcell.Posy, newValue,false);

            AnimController.Instance.SmoothAppear(randcell);
        }
    }
    public void RegenerateField()
    {
        
        if (cells == null)
            Create();
        for (int x = 0; x < fieldSizeInCells; x++)
            for (int y = 0; y < fieldSizeInCells; y++)
                cells[x, y].SetValue(x, y, 0);
        for (int x = 0; x < InitCells; x++)
            AddRandomCells();
    }
    private void ResetCellsFlag()
    {
        for (int x = 0; x < fieldSizeInCells; x++)
            for (int y = 0; y < fieldSizeInCells; y++)
                cells[x, y].ResetMerge();
    }
}
