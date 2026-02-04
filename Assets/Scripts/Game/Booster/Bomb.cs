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
                    Vector3[] positions = new Vector3[] 
                    { 
                        centerSquare.transform.position, //tâm
                        centerSquare.transform.position + new Vector3(0, squareSize.y, 0), //ô bên trên center
                        centerSquare.transform.position - new Vector3(0, squareSize.y, 0), //ô bên dưới center
                        centerSquare.transform.position + new Vector3(squareSize.x, 0, 0), //ô bên phải center
                        centerSquare.transform.position - new Vector3(squareSize.x, 0, 0)  //ô bên trái center
                    };

                    foreach (var pos in positions) { 
                        Collider2D neighbor = Physics2D.OverlapPoint(pos); 
                        if (neighbor != null) { 
                            GridSquare square = neighbor.GetComponent<GridSquare>(); 
                            if (square != null && square.SquareOccupied) 
                            { 
                                square.Deactivate(); 
                                Debug.Log("Bomb: " + pos); 
                            } 
                        } 
                    }
                }
            }
        }

        BackToStartingPosition();
    }
}
