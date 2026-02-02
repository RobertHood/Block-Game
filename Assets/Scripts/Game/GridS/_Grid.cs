using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class _Grid : MonoBehaviour
{
    public ShapeStorage _shapeStorage;
    public int _columns = 0;
    public int _rows = 0;

    public float _squareGap = 0.1f;
    public GameObject _gridSquare;

    public Vector2 _startPosition = new Vector2(0, 0);
    public float _squareScale = 0.5f;
    public float _everySquareOffset = 0f;

    private Vector2 _offset = new Vector2(0, 0);
    private List<GameObject> _gridSquares = new List<GameObject>();
    private LineIndicator _lineIndicator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lineIndicator = GetComponent<LineIndicator>();
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
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SquareIndex = _squareIndex;
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

            if (_columnNumber > 0 && _columnNumber % 2 == 0)
            {
                _squareGapNumber.x++;
                pos_x_offset += _squareGap;
            }
            if (_rowNumber > 0 && _rowNumber % 2 == 0 && _row_moved == false)
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
    void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
    }
    void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
    }

    private void CheckIfShapeCanBePlaced()
    {
        var _squareIndexes = new List<int>();

        foreach (var s in _gridSquares)
        {
            var gridSquare = s.GetComponent<GridSquare>();
            if (gridSquare.Selected && !gridSquare.SquareOccupied)
            {
                _squareIndexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
            }
        }

        var currentSelectedShape = _shapeStorage.getCurrentSelectedShape();
        if (currentSelectedShape == null) return;

        if (currentSelectedShape._totalSquareNumber == _squareIndexes.Count)
        {   
            foreach(var s in _squareIndexes)
            {
                var gridSquare = _gridSquares[s].GetComponent<GridSquare>();
                _gridSquares[s].GetComponent<GridSquare>().PlaceShapeOnBoard();
            }
            var shapeLeft = 0;
            foreach(var s in _shapeStorage.shapeList)
            {
                if (s != currentSelectedShape && s.IsAnyOfShapeSquareActive())
                {
                    shapeLeft++;
                }
            }

            
            if (shapeLeft == 0)
            {
                GameEvents.RequestNewShapes();
            }
            else 
            {
                GameEvents.SetShapeInactive();
            }

            CheckIfAnyLinesAreCompleted();
        }
        else
        {
            Debug.Log($"Block placement failed - Expected {currentSelectedShape._totalSquareNumber} squares, but only {_squareIndexes.Count} were selected");
            GameEvents.MoveShapeToStartPosition();
        }
    }

    private void CheckIfAnyLinesAreCompleted()
    {
        List<int[]> lines = new List<int[]>();
        //columns
        foreach (var c in _lineIndicator._column_indexes)
        {
            lines.Add(_lineIndicator.GetVerticalLine(c));
        }
        //rows
        for (int row = 0; row < 8; row++){
            List<int> data = new List<int>(8);
            for (var i = 0; i < 8; i++){
                data.Add(_lineIndicator.line_data[row,i]);
            }
            lines.Add(data.ToArray());
        }

        var completedLines = CheckIfSquaresAreCompleted(lines);
        Debug.Log(completedLines);
    }

    private int CheckIfSquaresAreCompleted(List<int[]> data){
        List<int[]> completedLines = new List<int[]>();
        var linesCompleted = 0;
        foreach(var line in data){
            var lineCompleted = true;
            foreach(var squareIndex in line)
            {
                Debug.Log("squareIndex =" + squareIndex);
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if (!comp.SquareOccupied){
                    lineCompleted = false;
                }           
            }
            if (lineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach (var line in completedLines)
        {
            var completed = false;
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.Deactivate();
                // comp.ClearOccupied();
                completed = true;
            }

            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.Deactivate();
            }

            if (completed){
                linesCompleted++;
            }
        }
        return linesCompleted;
    }
}
