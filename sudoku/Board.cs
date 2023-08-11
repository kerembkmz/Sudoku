using System;


namespace sudoku;



public class Board
{
    private int[,] board = new int[9, 9];
    private bool[,] constantCells = new bool[9, 9];
    private bool[,] userAddedCells = new bool[9, 9];
    private Random random = new Random();
    private bool safeForUser = false;
    private int totalWrongAnswer = 0;



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

    public int[,] CreateNewBoardFromCells()
    {
        int[,] newBoard = new int[9, 9];

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (!constantCells[row, col] || userAddedCells[row, col])
                {
                    newBoard[row, col] = board[row, col];
                }
            }
        }

        return newBoard;
    }

    public Board ReturnBoardFromCells(Board board1)
    {
        Board newBoard = new Board();

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (!constantCells[row, col] || userAddedCells[row, col])
                {
                    newBoard.SetBoardValue(row, col, board1.GetBoardValue(row,col)); 
                }
            }
        }

        return newBoard;
    }



    // Get the value of the cell in the specified row and column
    // for writing it in the drawing board.
    public int GetBoardValue(int row, int col)
    {
        return board[row, col];
    }


    //// check if the cell is already filled
    public bool isFilled(int row, int col)
    {
        return constantCells[row, col];
    }

    public bool isFilledUser(int row, int col) {
        return userAddedCells[row, col];
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

    public void incrementWrongNumber()
    {
        totalWrongAnswer += 1;
    }

    public int GetWrongNumber() {
        return totalWrongAnswer;
    }

    public bool isSafeForUser(int[,] board2, int row, int col, int value)
    {
        /*

               for (int row1 = 0; row1 < 9; row1++)
                {
                    for (int col1 = 0; col1 < 9; col1++)
                    {
                       Console.Write(board2[row1, col1] + " ");
                    }
                        Console.WriteLine();
                }

        Console.WriteLine("-----------------------------------------");

                for (int row1 = 0; row1 < 9; row1++)
                {
                    for (int col1 = 0; col1 < 9; col1++)
                    {
                        Console.Write(board[row1, col1] + " ");
                    }
                    Console.WriteLine();
               }
        */

//*
        for (int i = 0; i < 9; i++)
        {
            if (board2[row, i] == value)
            {
                Console.WriteLine("Came Here 1");
                return false;
            }
        }

        //check for col
        for (int i = 0; i < 9; i++)
        {
            if (board2[i, col] == value)
            {
                Console.WriteLine("Came Here 2 ");
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
                if (board2[startRow + i, startCol + j] == value)
                {
                    Console.WriteLine("Came Here 3 ");
                    return false;
                }
            }
        }
//*/


        return true;
    }







    public void SetBoardValue(int row, int col, int num) {
        board[row, col] = num;
        constantCells[row, col] = false;
        userAddedCells[row, col] = true;
    }

    public bool IsValidPlacement(int row, int col, int num) {
        int[,] board2 = CreateNewBoardFromCells();
        if (isSafeForUser(board2, row, col, num))
        {
            safeForUser = true;
            return safeForUser;
        }
        else
        {
            safeForUser = false;
            return safeForUser;
        }
    }

    private void HideRandomCells(int numCellsToHide)
    {
        if (numCellsToHide <= 0 || numCellsToHide >= 81)
        {
            throw new ArgumentOutOfRangeException("numCellsToHide", "Number of cells to hide must be between 1 and 80.");
        }

        int cellsHidden = 0;

        // For preventing the case where [4,4] comes twice, we will check for the condition.
        while (cellsHidden < numCellsToHide)
        {
            int randomRow = random.Next(9);
            int randomCol = random.Next(9);

            if (!constantCells[randomRow, randomCol])
            {
                constantCells[randomRow, randomCol] = true;
                cellsHidden++;
            }
        }
    }

    public void hideRandomBoard(int numCellsToHide)
    {
        HideRandomCells(numCellsToHide);
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


    public bool IsSudokuCompleted()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                bool hideOrNot = isFilled(row, col);
                bool userOrNot = isFilledUser(row, col);
                int cellValue = GetBoardValue(row, col);

                if ((!hideOrNot || !userOrNot))
                {
                    Console.WriteLine("Hide or not " + hideOrNot);
                    Console.WriteLine("user or not " + userOrNot);
                    Console.WriteLine("Came here returns false");
                    return false;

                }
            }
        }
        Console.WriteLine("came here returns true");
        return true;
    }



}
