using UnityEngine;
using UnityEngine.EventSystems;

public class Bomb : Booster
{
    public override void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        Collider2D hit = Physics2D.OverlapPoint(_transform.position);
        if (hit != null)
        {
            GridSquare centerSquare = hit.GetComponent<GridSquare>();
            if (centerSquare != null)
            {
                Vector2 squareSize = Vector2.zero;
                BoxCollider2D collider = centerSquare.GetComponent<BoxCollider2D>();

                if (collider != null)
                {
                    squareSize = collider.size * centerSquare.transform.localScale;
                }
                else
                {
                    RectTransform rect = centerSquare.GetComponent<RectTransform>();
                    if (rect != null)
                    {
                        squareSize = new Vector2(rect.rect.width, rect.rect.height) * centerSquare.transform.localScale;
                    }
                }

                if (squareSize != Vector2.zero)
                {
                    Vector2 areaSize = squareSize * 2;
                    Collider2D[] hits = Physics2D.OverlapBoxAll(centerSquare.transform.position, areaSize, 0f);

                    foreach (var h in hits)
                    {
                        GridSquare square = h.GetComponent<GridSquare>();
                        if (square != null && square.SquareOccupied)
                        {
                            square.Deactivate();
                        }
                    }
                }
            }
        }

        BackToStartingPosition();
    }
}
