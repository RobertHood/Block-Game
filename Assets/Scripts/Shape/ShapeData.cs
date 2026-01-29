using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu]
[System.Serializable]
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public bool[] _column;
        private int _size;

        public Row(){}

        public Row(int _size)
        {
            CreateRow(_size);
        }

        public void CreateRow(int size)
        {
            _size = size;
            _column = new bool[_size];
            ClearRow();
        }

        public void ClearRow()
        {
            for (int i = 0; i < _size; i++)
            {
                _column[i] = false;
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public int column = 0;
    public int row = 0;
    public Row[] board;
    public void Clear()
    {
        for (var i = 0; i < row; i++)
        {
            board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        board = new Row[row];
        for (int i = 0; i < row; i++)
        {
            board[i] = new Row(column);
        }
    }
}
