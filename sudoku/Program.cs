using System;
using Raylib_cs;
using sudoku;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SudokuVisualization
{
    public class Program
    {

        private static int screenWidth = 500;
        private static int screenHeight = 500;
        private static int boardSize = 9;
        private static int cellSize = 40;
        private static Board sudokuBoard = new Board();
        private static int selectedNumber = -1;
        private static bool safeForUser;

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
                DrawNumberSelectionUI();
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        private static void DrawNumberSelectionUI()
        {
            int numberButtonSize = 40;
            int numberButtonMargin = 5;
            int numberButtonY = screenHeight - numberButtonSize - numberButtonMargin;

            for (int num = 1; num <= 9; num++)
            {
                int numberButtonX = num * (numberButtonSize + numberButtonMargin) - numberButtonSize;

                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) &&
                    Raylib.GetMouseY() >= numberButtonY && Raylib.GetMouseY() <= numberButtonY + numberButtonSize &&
                    Raylib.GetMouseX() >= numberButtonX && Raylib.GetMouseX() <= numberButtonX + numberButtonSize)
                {
                    selectedNumber = num;
                }

                Color buttonColor = (selectedNumber == num) ? Color.GRAY : Color.LIGHTGRAY;

                Raylib.DrawRectangle(numberButtonX, numberButtonY, numberButtonSize, numberButtonSize, buttonColor);
                Raylib.DrawText(num.ToString(), numberButtonX + 10, numberButtonY + 10, 20, Color.BLACK);
            }
        }




        private static void DrawSudokuBoard()
        {
            

            for (int i = 0; i <= boardSize; i++)
            {
                int x = i * cellSize;
                int y = i * cellSize;

                Raylib.DrawLine(x, 0, x, 360, Color.BLACK);
                Raylib.DrawLine(0, y, 360, y, Color.BLACK);

                // adding drawings to the 3x3 squares for easier visualization.
                Raylib.DrawLine(3 * cellSize, 0, 3 * cellSize, 360, Color.BLUE);
                Raylib.DrawLine(6 * cellSize, 0, 6 * cellSize, 360, Color.BLUE);

                Raylib.DrawLine(0 , 3*cellSize , 360 , 3*cellSize, Color.BLUE);
                Raylib.DrawLine(0 , 6*cellSize , 360, 6*cellSize, Color.BLUE);

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
                    bool userOrNot = sudokuBoard.isFilledUser(row, col);
                    if (number != 0 && !hideOrNot)
                    {
                        if (userOrNot)
                        {
                            //Console.WriteLine($"safeForUser second check {safeForUser}");
                            if (safeForUser)
                            {
                                Raylib.DrawText(number.ToString(), x, y, 20, Color.DARKGRAY);
                            }
                            else if(!safeForUser){
                                Raylib.DrawText(number.ToString(), x, y, 20, Color.RED);
                            }
                        }
                        else
                        {
                            Raylib.DrawText(number.ToString(), x, y, 20, Color.BLACK);

                        }
                    }
                }
            }
            if (Raylib.GetMouseX() < boardSize * cellSize && Raylib.GetMouseY() < boardSize * cellSize)
            {

                

                // ... Initialize the board, constantCells, and userAddedCells ...

                // Create the new board based on constantCells and userAddedCells
                

                // Calculate clicked row and column indices
                int clickedRow = Raylib.GetMouseY() / cellSize;
                int clickedCol = Raylib.GetMouseX() / cellSize;

                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    //Console.WriteLine("Mouse Left Button Pressed");
                    //Console.WriteLine($"Clicked Cell: Row {clickedRow}, Col {clickedCol}");

                    if ((sudokuBoard.isFilled(clickedRow,clickedCol) || sudokuBoard.isFilledUser(clickedRow,clickedCol ))&& selectedNumber != -1)
                    {
                        //Console.WriteLine($"Setting Board Value: Row {clickedRow}, Col {clickedCol}, Number {selectedNumber}");
                        safeForUser =  sudokuBoard.IsValidPlacement(clickedRow, clickedCol, selectedNumber);
                        //Console.WriteLine($"safeForUser {safeForUser}");
                        sudokuBoard.SetBoardValue(clickedRow, clickedCol, selectedNumber);
                    }
                }
            }
        }
       
        }
    }

