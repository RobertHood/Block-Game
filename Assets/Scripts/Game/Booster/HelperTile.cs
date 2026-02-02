using UnityEngine;
using UnityEngine.EventSystems;

public class HelperTile : Booster
{
    public ShapeData shapeData;

    public override void OnDrag(PointerEventData eventData)
    {
        _transform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        Collider2D hit = Physics2D.OverlapPoint(_transform.position);

        if (hit != null)
        {
            GridSquare square = hit.GetComponent<GridSquare>();
            if (square != null && !square.SquareOccupied)
            {
                square.ActivateSquare();
                // gameObject.SetActive(false);
                BackToStartingPosition();
                return;
            }
        }

        BackToStartingPosition();
    }
}
