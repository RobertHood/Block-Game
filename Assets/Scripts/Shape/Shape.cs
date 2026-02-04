using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public GameObject squareShapeImage;
    public Vector3 _shapeSelectedScale;
    public ShapeData CurrentShapeData;
    public int _totalSquareNumber {get; set;}
    public Vector2 offset;

    private List<GameObject> _currentShape = new List<GameObject>();
    private Vector3 _shapeStartScale;
    private RectTransform _transform;
    private bool _shapeDraggable = true;
    private Canvas _canvas;

    private CanvasGroup _canvasGroup;
    private Vector3 _startPosition;
    private bool _shapeActive = true;
    private List<GridSquare> _hitGridSquares = new List<GridSquare>();
    public void Awake()
    {
        _shapeStartScale = this.GetComponent<RectTransform>().localScale;
        _transform = this.GetComponent<RectTransform>();
        _canvasGroup = this.GetComponent<CanvasGroup>();
        _canvas = GetComponentInParent<Canvas>();
        _shapeDraggable = true;
        _shapeActive = true;
    }

    private void Start()
    {
        _startPosition = transform.localPosition;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        GameEvents.MoveShapeToStartPosition += MoveShapeToStartPosition;
        GameEvents.SetShapeInactive += SetShapeInactive;
    }

    private void OnDisable()
    {
        GameEvents.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        GameEvents.SetShapeInactive -= SetShapeInactive;
    }


    public bool IsOnStartPosition()
    {
        return (_transform.localPosition - _startPosition).sqrMagnitude < 0.01f;
    }

    public bool IsAnyOfShapeSquareActive()
    {
        foreach(var square in _currentShape)
        {
            if (square.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public void DeactivateShape()
    {
        if (_shapeActive)
        {
            foreach(var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().DeactivateShape();
            }            
        }
        _shapeActive = false;
    }

    private void SetShapeInactive(){
        if (IsOnStartPosition() == false && IsAnyOfShapeSquareActive()){
            foreach(var square in _currentShape){
                square?.GetComponent<ShapeSquare>().DeactivateShape();
            }
        }
    }
    public void ActivateShape()
    {
        if (!_shapeActive)
        {
            for (int i = 0; i < _totalSquareNumber; i++)
            {
                _currentShape[i]?.GetComponent<ShapeSquare>().ActivateShape();
            }
        }
        _shapeActive = true;
    }
    public void RequestNewShape(ShapeData shapeData)
    {
        transform.localPosition = _startPosition;
        CreateShape(shapeData);
    }
    public void CreateShape(ShapeData shapeData)
    {
        Debug.Log($"CreateShape called with shape: {shapeData.name}");
        CurrentShapeData = shapeData;
        _totalSquareNumber = GetNumberOfSquares(shapeData);
        Debug.Log($"Shape {gameObject.name} - _totalSquareNumber set to: {_totalSquareNumber}");
        
        while (_currentShape.Count < _totalSquareNumber)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }
        Debug.Log($"Shape {gameObject.name} - Created {_currentShape.Count} GameObject squares");

        foreach(var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);

        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);

        int _currentIndexInList = 0;

        for (var row = 0; row < shapeData.row; row++)
        {
            for (var column = 0; column < shapeData.column; column++)
            {
                if (shapeData.board[row]._column[column])
                {
                    _currentShape[_currentIndexInList].gameObject.SetActive(true);
                    _currentShape[_currentIndexInList].gameObject.GetComponent<RectTransform>().localPosition = new Vector2(GetXPositionForShapeSquare(shapeData, column, moveDistance), GetYPositionForShapeSquare(shapeData, row, moveDistance));
                    _currentIndexInList++;
                }
            }
        }


    }
    //trong editor/shapedata bao nhiêu ô được đánh trắng => return số ô đánh trắng
    private int GetNumberOfSquares(ShapeData shapeData)
    {
        int number = 0;

        Debug.Log($"=== CALCULATING SQUARES FOR SHAPE ===");
        Debug.Log($"ShapeData name: {shapeData.name}");
        Debug.Log($"ShapeData rows: {shapeData.row}, columns: {shapeData.column}");

        foreach(var rowData in shapeData.board)
        {
            foreach(var active in rowData._column)
            {
                if (active)
                {
                    number++;
                }
            }
        }

        Debug.Log($"Total active squares counted: {number}");
        Debug.Log($"=====================================");

        return number;
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float centerY = (shapeData.row - 1) * moveDistance.y / 2f;
        float squareY = row * moveDistance.y;
        return squareY - centerY;
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        float centerX = (shapeData.column - 1) * moveDistance.x / 2f;
        float squareX = column * moveDistance.x;
        return squareX - centerX;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        _canvasGroup.blocksRaycasts = false;
        _transform.localScale = new Vector3(1f,1f,1f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        _transform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

        foreach (var square in _hitGridSquares)
        {
            square.Highlight(false);
        }
        _hitGridSquares.Clear();

        foreach (var square in _currentShape)
        {
            if (!square.activeSelf) continue;

            var hits = Physics2D.OverlapPointAll(square.transform.position);
            foreach (var hit in hits)
            {
                var gridSquare = hit.GetComponent<GridSquare>();
                if (gridSquare != null && !gridSquare.SquareOccupied)
                {
                    gridSquare.Highlight(true);
                    _hitGridSquares.Add(gridSquare);
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        _canvasGroup.blocksRaycasts = true;
        _transform.localScale = new Vector3(1f,1f,1f); 

        if (eventData.pointerEnter != null)
        {
            var square = eventData.pointerEnter.GetComponentInParent<GridSquare>();
            if (square != null)
            {
                // Convert mouse position to local space of the Shape
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_transform, eventData.position, _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera, out localPoint);

                // Find the square within the shape that is closest to the mouse pointer
                GameObject closestSquare = null;
                float minDistance = float.MaxValue;

                foreach (var shapeSquare in _currentShape)
                {
                    if (!shapeSquare.activeSelf) continue;

                    float dist = Vector2.Distance(shapeSquare.GetComponent<RectTransform>().localPosition, localPoint);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        closestSquare = shapeSquare;
                    }
                }

                // Snap the shape so that the closest square aligns perfectly with the target GridSquare
                if (closestSquare != null)
                {
                    _transform.position = square.GetComponent<RectTransform>().position - _transform.TransformVector(closestSquare.GetComponent<RectTransform>().localPosition);
                }
                else
                {
                    _transform.position = square.GetComponent<RectTransform>().position;
                }

                GameEvents.CheckIfShapeCanBePlaced();
            }
            else
            {
                ClearAllHighlight();
                GameEvents.MoveShapeToStartPosition();
            }
        }
        else
        {
            ClearAllHighlight();
            GameEvents.MoveShapeToStartPosition();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    private void MoveShapeToStartPosition()
    {
        _transform.transform.localPosition = _startPosition;
    }

    private void ClearAllHighlight()
    {
        foreach (var s in _hitGridSquares)
        {
            s.Highlight(false);
        }
        _hitGridSquares.Clear();
    }
}
