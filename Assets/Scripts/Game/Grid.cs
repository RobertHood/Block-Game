using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Grid : MonoBehaviour
{
    public int _columns = 0;
    public int _rows = 0;

    public float _squareGap = 0.1f;
    public GameObject _gridSquare;

    public Vector2 _startPosition = new Vector2(0, 0);
    public float _squareScale = 0.5f;
    public float _everySquareOffset = 0f;

    private Vector2 _offset = new Vector2(0, 0);
    private List<GameObject> _gridSquares = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPosition();
    }
    private void SpawnGridSquares()
    {   
        int _squareIndex = 0;
        for (var row = 0; row < _rows; row++)
        {
            for (var column = 0; column < _columns; column++)
            {
                _gridSquares.Add(Instantiate(_gridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(_squareScale, _squareScale, _squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetImage(_squareIndex % 2 == 0);
                _squareIndex++;
            }
        }
    }
    private void SetGridSquaresPosition()
    {
        int _columnNumber = 0;
        int _rowNumber = 0;
        Vector2 _squareGapNumber = new Vector2(0.0f, 0.0f);
        bool _row_moved = false;

        var _square_rect = _gridSquares[0].GetComponent<RectTransform>();
        _offset.x = _square_rect.rect.width * _square_rect.transform.localScale.x + _everySquareOffset;
        _offset.y = _square_rect.rect.height * _square_rect.transform.localScale.y + _everySquareOffset;

        foreach (GameObject square in _gridSquares)
        {
            if (_columnNumber + 1 > _columns)
            {
                _squareGapNumber.x = 0;
                _columnNumber = 0;
                _rowNumber++;
                _row_moved = true;
            }
        
            var pos_x_offset = _offset.x * _columnNumber + (_squareGapNumber.x * _squareGap);
            var pos_y_offset = _offset.y * _rowNumber + (_squareGapNumber.y * _squareGap);

            if (_columnNumber > 0 && _columnNumber % 3 == 0)
            {
                _squareGapNumber.x++;
                pos_x_offset += _squareGap;
            }
            if (_rowNumber > 0 && _rowNumber % 3 == 0 && _row_moved == false)
            {
                _row_moved = true;
                _squareGapNumber.y++;
                pos_y_offset += _squareGap;
            }
            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(_startPosition.x + pos_x_offset, _startPosition.y - pos_y_offset);
            
            square.GetComponent<RectTransform>().localPosition = new Vector3(_startPosition.x + pos_x_offset, _startPosition.y - pos_y_offset, 0.0f);

            _columnNumber++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
