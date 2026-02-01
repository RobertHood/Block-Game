using System;
using UnityEngine;

public class LineIndicator : MonoBehaviour
{
    public int[,] line_data = new int[8, 8]
    {
        { 0, 1, 2, 3, 4, 5, 6, 7},
        { 8, 9,10,11,12,13,14,15},
        {16,17,18,19,20,21,22,23},
        {24,25,26,27,28,29,30,31},
        {32,33,34,35,36,37,38,39},
        {40,41,42,43,44,45,46,47},
        {48,49,50,51,52,53,54,55},
        {56,57,58,59,60,61,62,63}
    };

    [HideInInspector]
    public int[,] square_data = new int[0, 0]; // Not used in 8x8 grid

    [HideInInspector]
    public int[] _column_indexes = new int[8]
    {
        0,1,2,3,4,5,6,7
    };

    public int[] GetVerticalLine(int square_index)
    {
        int[] line = new int[8];
        var _squarePositionColumn = GetSquarePosition(square_index).Item2;
        for (int i = 0; i < 8; i++)
        {
            line[i] = line_data[i,_squarePositionColumn];
        }
        return line;
    }

    private (int, int) GetSquarePosition(int square_index)
    {
        int pos_row = -1;
        int pos_col = -1;

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0;col < 8; col++)
            {
                if (line_data[row,col] == square_index)
                {
                    pos_row = row;
                    pos_col = col;
                    break;
                }
            }
        }

        return (pos_row, pos_col);
    
    }

    public int GetGridSquareIndex(int square)
    {
        int rows = square_data.GetLength(0);
        int cols = square_data.GetLength(1);
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < cols; column++)
            {
                if (square_data[row,column] == square)
                {
                    return row;
                }
            }
        }
        return -1;
    }
}
