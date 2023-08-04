using System;
using Raylib_cs;
using sudoku;

namespace SudokuVisualization
{
    public class Program
    {
        private static int screenWidth = 360;
        private static int screenHeight = 360;
        private static int boardSize = 9;
        private static int cellSize = 40;
        private static Board sudokuBoard = new Board(); 

        public static void Main()
        {
            Raylib.InitWindow(screenWidth, screenHeight, "Sudoku Board");
            sudokuBoard.FillRandomBoard(); //Generate the board
            sudokuBoard.hideRandomBoard(60); //Hide the wanted number of cells.


            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.WHITE);

                DrawSudokuBoard();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        private static void DrawSudokuBoard()
        {
            for (int i = 0; i <= boardSize; i++)
            {
                int x = i * cellSize;
                int y = i * cellSize;

                Raylib.DrawLine(x, 0, x, screenHeight, Color.BLACK);
                Raylib.DrawLine(0, y, screenWidth, y, Color.BLACK);
            }

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    
                    int x = col * cellSize + cellSize / 2;
                    int y = row * cellSize + cellSize / 2;

                    // Draw the number in each cell
                    int number = sudokuBoard.GetBoardValue(row, col);
                    bool hideOrNot = sudokuBoard.isFilled(row, col);

                    if (number != 0 && !hideOrNot)
                    {
                        Raylib.DrawText(number.ToString(), x, y, 20, Color.BLACK);
                    }
                }
            }
        }
    }
}
