using System;
namespace sudoku;

public class Board
{
    //create 9x9 matrix
    private int[,] board = new int[9, 9];
    private bool[,] constantCells = new bool[9, 9];
    private static Random random = new Random();

    public Board()
    {
        //None of the cells are constant in the start, they are all set to 0.
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                constantCells[row, col] = false;
            }
        }
    }

    // Get the value of the cell in the specified row and column
    // for writing it in the drawing board.
    public int GetBoardValue(int row, int col)
    {
        return board[row, col];
    }


    //// check if the cell is already filled
    public static bool isFilled(bool[,] constantCells, int row, int col)
    {
        return constantCells[row, col];
    }


    //each number must appear only once (1 to 9), in each row and in each column.
    //inside each 3x3 box, each number must appear only once(1 to 9).
    public static bool isSafe(int[,] board, int row, int col, int value)
    {
        //check for row
        for (int i = 0; i < 9; i++)
        {
            if (board[row, i] == value)
            {
                return false;
            }
        }

        //check for col
        for (int i = 0; i < 9; i++)
        {
            if (board[i, col] == value)
            {
                return false;
            }
        }

        //check for 3x3 box
        int startRow = row - row % 3;
        int startCol = col - col % 3;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[startRow + i, startCol + j] == value)
                {
                    return false;
                }
            }
        }

        return true;
    }



    // Fill the board.
    public void FillRandomBoard()
    {
        //Clear first
        ClearBoard();
        FillRandomRecursive(0, 0);
    }


    //Set all values to 0 again, set all constants to false.
    //Works like a constructor. 
    private void ClearBoard()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                board[row, col] = 0;
                constantCells[row, col] = false;
            }
        }
    }

    
    private bool FillRandomRecursive(int row, int col)
    {
        if (row == 9) // Base case: Filled all cells successfully
            return true;

        int nextRow = row;
        int nextCol = col + 1;
        if (nextCol == 9) 
        {
            nextRow++;
            nextCol = 0;
        }

        if (constantCells[row, col]) 
            return FillRandomRecursive(nextRow, nextCol);

        // Randomize the order of numbers 1 to 9 so that we get a different board every time we run. 
        int[] randomOrder = GetRandomOrder();

        foreach (int num in randomOrder)
        {
            if (isSafe(board, row, col, num))
            {
                board[row, col] = num;
                if (FillRandomRecursive(nextRow, nextCol))
                    return true;
                board[row, col] = 0; // Backtrack if the solution is not valid
            }
        }

        return false; // No valid number found, need to backtrack further
    }


    // Helper method to get a random order of numbers 1 to 9
    private int[] GetRandomOrder()
    {
        int[] randomOrder = new int[9];
        for (int i = 0; i < 9; i++)
            randomOrder[i] = i + 1;

        for (int i = 0; i < 9; i++)
        {
            int temp = randomOrder[i];
            int randomIndex = random.Next(i, 9);
            randomOrder[i] = randomOrder[randomIndex];
            randomOrder[randomIndex] = temp;
        }

        return randomOrder;
    }


}
