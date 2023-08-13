using System;
using Raylib_cs;

namespace SudokuVisualization
{
    public class SudokuGame
    {
        SudokuDrawer sudokuDrawer;
        public SudokuGame()
        {
            sudokuDrawer = new SudokuDrawer();

        }


        public void Run()
        {
            Raylib.InitWindow(800, 500, "Sudoku Board");

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.WHITE);

                sudokuDrawer.DrawSudokuBoard();

                sudokuDrawer.SolveSudokuStepByStep();

                sudokuDrawer.DrawSudokuBoard2();

                sudokuDrawer.DrawGameIsCompletedWithSuccess();

                sudokuDrawer.DrawNumberSelectionUI();

                sudokuDrawer.DrawFalseTryOutOfOppurtunity();


                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
