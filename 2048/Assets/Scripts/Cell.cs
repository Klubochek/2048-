using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int Posx { get; private set; }
    public int Posy { get; private set; }
    public int Value { get; private set; }

    public int Point => IsEmpty ? 0 : (int)Mathf.Pow(2, Value);
    public bool IsEmpty => Value == 0;
    public bool HasMerged { get; private set; }

    public int MaxValue;

    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI pointOnCell;

    private CellsAnimation currentAnimation;

    public void SetValue(int posx, int posy, int value, bool updateUI = true ) 
    {
        Posx = posx;
        Posy = posy;
        Value = value;
        if (updateUI)
            ChangeCollor();
    }
    public void ChangeCollor()
    {
        pointOnCell.text=IsEmpty ? string.Empty : Point.ToString();

        if (Value > 12)
            image.color = ColorManager.Instance.colors[12];
        else
        image.color = ColorManager.Instance.colors[Value];
        
    }
    public void IncreaseValue()
    {
        Value++;
        HasMerged = true;

        GameController.Instance.AddPoints(Point);
        
    }
    public void ResetMerge()
    {
        HasMerged = false;
    }

    public void MergeWithCell(Cell other) 
    {
        AnimController.Instance.SmoothTrans(this, other, true);
        other.IncreaseValue();
        SetValue(Posx, Posy, 0);

        
    }
    public void MoveToCell(Cell target)
    {
        target.SetValue(target.Posx, target.Posy, Value,false);
        SetValue(Posx, Posy, 0);
        AnimController.Instance.SmoothTrans(this, target, false);
    }
    public void SetAnim(CellsAnimation animation)
    {
        currentAnimation = animation;
    }
    public void CanselAnim()
    {
        if (currentAnimation != null)
            currentAnimation.Destroy();
    }
}
