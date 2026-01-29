using System.Collections.Generic;

using System.Reflection;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public GameObject squareShapeImage;

    public ShapeData CurrentShapeData;

    private List<GameObject> _currentShape = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void CreateShape(ShapeData shapeData)
    {
        CurrentShapeData = shapeData;

        var _totalSquareNumber = GetNumberOfSquares(shapeData);
        while (_currentShape.Count <= _totalSquareNumber)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }

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

        return number;
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0f;

        if (shapeData.row > 1)
        {
            if (shapeData.row % 2 != 0)
            {
                var middleSquareIndex = (shapeData.row - 1) / 2;
                var multiplier = (shapeData.row - 1) / 2;
                if (row < middleSquareIndex)
                {
                    shiftOnY = moveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
                else if (row > middleSquareIndex)
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
            }
        }
        else
        {
            var middleSquareIndex2 = (shapeData.row == 2) ? 1 : (shapeData.row / 2);
            var middleSquareIndex1 = (shapeData.row == 2) ? 0 : (shapeData.row - 1);  
            var multiplier = shapeData.row / 2;

            if (row == middleSquareIndex1 || row == middleSquareIndex2)
            {
                if (row == middleSquareIndex2)
                {
                    shiftOnY = moveDistance.y / 2;
                }
                if (row == middleSquareIndex1)
                {
                    shiftOnY = (moveDistance.y / 2) * -1;
                }
            }
            if (row < middleSquareIndex1 && row < middleSquareIndex2)
            {
                shiftOnY = moveDistance.x * -1;
                shiftOnY *= multiplier;
            }
            else if (row > middleSquareIndex1 && row > middleSquareIndex2)
            {
                shiftOnY = moveDistance.x * 1;
                shiftOnY *= multiplier;
            }
        }
        return shiftOnY;
    }


    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        float shiftOnX = 0f;

        if (shapeData.column > 1)
        {
            if (shapeData.column % 2 != 0)
            {
                var middleSquareIndex = (shapeData.column - 1) / 2;
                var multiplier = (shapeData.column - 1) / 2;
                if (column < middleSquareIndex)
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if (column > middleSquareIndex)
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
        }
        else
        {
            var middleSquareIndex2 = (shapeData.column == 2) ? 1 : (shapeData.column / 2);
            var middleSquareIndex1 = (shapeData.column == 2) ? 0 : (shapeData.column - 1);
            var multiplier = shapeData.column / 2;

            if (column == middleSquareIndex1 || column == middleSquareIndex2)
            {
                if (column == middleSquareIndex2)
                {
                    shiftOnX = moveDistance.x / 2;
                }
                if (column == middleSquareIndex1)
                {
                    shiftOnX = (moveDistance.x / 2) * -1;
                }
            }

            if (column < middleSquareIndex1 && column < middleSquareIndex2)
            {
                shiftOnX = moveDistance.x * -1;
                shiftOnX *= multiplier;
            }
            else if (column > middleSquareIndex1 && column > middleSquareIndex2)
            {
                shiftOnX = moveDistance.x * 1;
                shiftOnX *= multiplier;
            }
        }

        return shiftOnX;
    }
}