using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bomb : Booster
{
    private List<GridSquare> _effectedGridSquares = new List<GridSquare>();
    public override void OnDrag(PointerEventData eventData)
    {
        _transform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        
        ClearAllHighlight();

        Collider2D hit = Physics2D.OverlapPoint(_transform.position);
        if (hit != null)
        {
            GridSquare _centerHoverSquare = hit.GetComponent<GridSquare>();

            Vector2 squareSize = Vector2.zero;
            BoxCollider2D collider = _centerHoverSquare.GetComponent<BoxCollider2D>();

            if (collider != null)
                {
                    squareSize = collider.size * _centerHoverSquare.transform.localScale;
                }
                else
                {
                    RectTransform rect = _centerHoverSquare.GetComponent<RectTransform>();
                    if (rect != null)
                    {
                        squareSize = new Vector2(rect.rect.width, rect.rect.height) * _centerHoverSquare.transform.localScale;
                    }
                }
    
                if (squareSize != Vector2.zero)
                {
                    Vector3[] positions = new Vector3[] 
                    { 
                        _centerHoverSquare.transform.position, //tâm
                        _centerHoverSquare.transform.position + new Vector3(0, squareSize.y, 0), //ô bên trên center
                        _centerHoverSquare.transform.position - new Vector3(0, squareSize.y, 0), //ô bên dưới center
                        _centerHoverSquare.transform.position + new Vector3(squareSize.x, 0, 0), //ô bên phải center
                        _centerHoverSquare.transform.position - new Vector3(squareSize.x, 0, 0)  //ô bên trái center
                    };

                    foreach (var pos in positions) { 
                        Collider2D neighbor = Physics2D.OverlapPoint(pos); 
                        GridSquare square = neighbor.GetComponent<GridSquare>();
                        if (neighbor != null) { 
                            square.BoosterHighlight(true);
                            _effectedGridSquares.Add(square);
                    }
                }
            }
        }
    }

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
        ClearAllHighlight();

    }

    private void ClearAllHighlight()
    {
        foreach (var s in _effectedGridSquares)
        {
            s.BoosterHighlight(false);
        }

    }
}
