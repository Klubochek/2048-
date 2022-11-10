using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellsAnimation : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI text;

    private float moveTime = 0.1f;
    private float appeatTime = 0.1f;

    private Sequence sequence;
    public void Move(Cell from, Cell to, bool isMerging)
    {
        from.CanselAnim();
        to.SetAnim(this);

        image.color = ColorManager.Instance.colors[from.Value];
        text.text = from.Point.ToString();
        text.color = Color.black;

        transform.position = from.transform.position;
        sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(to.transform.position, moveTime).SetEase(Ease.InOutQuad));
        if (isMerging)
        {
            sequence.AppendCallback(() =>
            {
                image.color = ColorManager.Instance.colors[to.Value];
                text.text = to.Point.ToString();
                text.color = Color.black;
            });
            sequence.Append(transform.DOScale(1.2f, appeatTime));
            sequence.Append(transform.DOScale(1f, appeatTime));
        }
        sequence.AppendCallback(() =>
        {
            to.ChangeCollor();
            Destroy();
        });
    }
    public void Appear(Cell cell)
    {
        cell.CanselAnim();
        cell.SetAnim(this);

        image.color = ColorManager.Instance.colors[cell.Value];
        text.text = cell.Point.ToString();
        text.color = Color.black;

        transform.position = cell.transform.position;
        transform.localScale = Vector2.zero;

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1.2f, appeatTime * 2));
        sequence.Append(transform.DOScale(1f, appeatTime * 2));

        sequence.AppendCallback(() =>
        {
            cell.ChangeCollor();
            Destroy();
        });
    }
    public void Destroy()
    {
        sequence.Kill();
        Destroy(gameObject);
    }
}