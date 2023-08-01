using System;

public class Board
{
	//create 9x9 matrix
	int[,] board = new int[9, 9];

	public Board(int level)
	{
		switch (level)
		{
			//easy level : 30 cells filled
			case 1:
				FillRandomCells(board,30);
				break;

			//medium level : 25 cells filled
			case 2:
				FillRandomCells(board,25);
				break;

			//hard level : 17 cells filled
			case 3:
				FillRandomCells(board,17);
				break;
		}
	}


	//// check if the cell is already filled
	static bool isFilled(int[,] board, int row, int col)
	{
		if (board[row, col] != 0)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

    //each number must appear only once (1 to 9), in each row and in each column.
    //inside each 3x3 box, each number must appear only once(1 to 9).
	static bool isSafe(int[,] board, int row, int col, int value)
	{
		//check for row
		for(int i=0; i<9; i++)
		{
			if(board[row,i] == value)
			{
				return false;
			}
		}

		//check for col
		for(int i=0; i<9; i++)
		{
			if (board[i,col] == value)
			{
				return false;
			}
		}

		//check for 3x3 box
		int startRow = row - row % 3;
		int startCol = col - col % 3;
		for(int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++) 
			{
					if (board[startRow + i, startCol+j] == value)
					{
						return false;
					}
			}
		}

		return true;
	}


    static void FillRandomCells(int[,] board, int numberOfCells)
	{
		Random random = new Random();
		for(int i=0; i<numberOfCells; i++)
		{
			int row = random.Next(9);
			int col = random.Next(9);

            while (isFilled(board, row, col))
            {
                row = random.Next(9);
                col = random.Next(9);
            }

			int value = random.Next(1, 10);
			while (!isSafe(board, row, col, value))
			{
				value = random.Next(1, 10); 
			}

			board[row, col] = value;
        }

	}

	public void printBoard()
	{
		for(int i=0; i<9; i++)
		{
			for(int j=0; j<9; j++)
			{
				Console.Write(board[i, j] + " ");
				if((j+1)%3 == 0 && j != 8)
				{
					Console.Write("| ");
				}
			}
			Console.WriteLine();
			if((i+1)%3 == 0 && i != 8)
			{
				Console.Write("------+------+------");
			}
		}
	}

}

