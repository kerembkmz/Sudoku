using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using System.Diagnostics;
using Raylib_cs;
using sudoku;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SudokuVisualization
{
    public class Program
    {

        private static int screenWidth = 800;
        private static int screenHeight = 500;
        private static int boardSize = 9;
        private static int cellSize = 40;
        private static Board sudokuBoard = new Board();
        private static int selectedNumber = -1;
        private static bool safeForUser;
        private static int totalTryNumber = 3;
        private static bool userCanPlay = true;
        private static bool sudokuCompleted = false;
        private static bool sudokuCompleted2 = false;
        private static int filledByUser = 0;
        private static int totalHiddenNum = 45;
        private static Board sudokuBoard2 = new Board(); 


        public static void Main()
        {
            Raylib.InitWindow(screenWidth, screenHeight, "Sudoku Board");
            sudokuBoard.FillRandomBoard(); //Generate the board
            sudokuBoard.hideRandomBoard(totalHiddenNum); //Hide the wanted number of cells.
            sudokuBoard2 = sudokuBoard.ReturnBoardFromCells(sudokuBoard);

            List<int[,]> solutionSteps = new List<int[,]>();
            


            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();

                Raylib.ClearBackground(Color.WHITE);

                DrawSudokuBoard();
                



                SolveSudokuStepByStep(solutionSteps);

                DrawSudokuBoard2();

                DrawGameIsCompletedWithSuccess();
                

                DrawNumberSelectionUI();

                DrawFalseTryOutOfOppurtunity();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        private static void DrawFalseTryOutOfOppurtunity() {

            if (!sudokuCompleted)
            {
                if (sudokuBoard.GetWrongNumber() < totalTryNumber)
                {
                    string output = "Wrong tries:" + sudokuBoard.GetWrongNumber() + "/" + totalTryNumber;
                    Raylib.DrawText(output, 20, 400, 20, Color.BLACK);
                }
                else
                {
                    string output = "Failed to complete the SUDOKU puzzle, 3 wrong answers already.";
                    Raylib.DrawText(output, 20, 400, 15, Color.BLACK);
                    userCanPlay = false;

                }
            }
        }

        private static void DrawGameIsCompletedWithSuccess() {
            if(totalHiddenNum == filledByUser) {
                sudokuCompleted = true;
                string output = "Sudoku completed succesfully";
                Raylib.DrawText(output, 400, 400, 20, Color.BLACK);
            }
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


        private static void DrawSudokuBoard2()
        {


            for (int i = 0; i <= boardSize; i++)
            {
                int x = i * cellSize ;
                int y = i * cellSize ;

                Raylib.DrawLine(400+x, 20, 400+x, 380, Color.BLACK);
                Raylib.DrawLine(400, y+20, 760, y+20, Color.BLACK);

                // adding colored drawings to the 3x3 squares for easier visualization.
                Raylib.DrawLine(3 * cellSize + 400, 20, 3 * cellSize + 400, 380, Color.BLUE);
                Raylib.DrawLine(3 * cellSize + 401, 20, 3 * cellSize + 401, 380, Color.DARKBLUE);
                Raylib.DrawLine(6 * cellSize + 400, 20, 6 * cellSize + 400, 380, Color.BLUE);
                Raylib.DrawLine(6 * cellSize + 401, 20, 6 * cellSize + 401, 380, Color.DARKBLUE);

                Raylib.DrawLine(400, 3 * cellSize + 20, 760, 3 * cellSize + 20, Color.BLUE);
                Raylib.DrawLine(400, 3 * cellSize + 21, 760, 3 * cellSize + 21, Color.DARKBLUE);
                Raylib.DrawLine(400, 6 * cellSize + 20, 760, 6 * cellSize + 20, Color.BLUE);
                Raylib.DrawLine(400, 6 * cellSize + 21, 760, 6 * cellSize + 21, Color.DARKBLUE);

            }


           

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {

                    int x = col * cellSize + cellSize / 2 + 400;
                    int y = row * cellSize + cellSize / 2 + 20;
                    
                    int number = sudokuBoard2.GetBoardValue(row, col);
                    bool hideOrNot = sudokuBoard2.isFilled(row, col);
                    bool userOrNot = sudokuBoard2.isFilledUser(row, col);
                    bool botOrNot = sudokuBoard2.isFilledBot(row, col);


                    if (number != 0 && !hideOrNot)
                    {
                        if (userOrNot)
                        {


                            if (!botOrNot)
                            {
                                Raylib.DrawText(number.ToString(), x - 5, y - 10, 20, Color.GRAY);
                            }
                            else {
                                Raylib.DrawText(number.ToString(), x - 5, y - 10, 20, Color.BLACK);
                            }
                            
                            
                        }

                        
                        
                    }

                    if (sudokuCompleted2)
                    {
                        string output = "Sudoku completed successfully by bot";
                        Raylib.DrawText(output, 400, 400, 20, Color.BLACK);
                        Raylib.DrawText(number.ToString(), x - 5, y - 10, 20, Color.GREEN);
                    }

                }
            }



                }


        private static bool SolveSudokuStepByStep(List<int[,]> solutionSteps)
        {
            //The commands lines will show the solutions logic in the output, for debugging purposes.

            double currentTime = 0;
            double moveInterval = 4; // Move every 3 seconds

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (sudokuBoard2.GetBoardValue(row, col) == 0)
                    {
                        for (int num = 1; num <= 9; num++)
                        {
                            double startTime = Raylib.GetTime();

                            
                            while (currentTime < moveInterval)
                            {
                                currentTime = Raylib.GetTime() - startTime;

                                
                                Raylib.ClearBackground(Color.WHITE);

                                
                                DrawSudokuBoard();
                                DrawSudokuBoard2();  
                                DrawNumberSelectionUI();
                                DrawFalseTryOutOfOppurtunity();
                                DrawGameIsCompletedWithSuccess();
                                

                                
                                Raylib.EndDrawing();
                            }

                            if (Board.isSafe(sudokuBoard2.CreateNewBoardFromCells(), row, col, num))
                            {
                                sudokuBoard2.SetBoardValue(row, col, num);

                                //Console.WriteLine("Adding " + row + " " + col + ":" + num);
                                solutionSteps.Add(sudokuBoard2.CreateNewBoardFromCells());

                                if (SolveSudokuStepByStep(solutionSteps))
                                {
                                    return true; // Successfully solved
                                }
                                
                                sudokuBoard2.SetBoardValue(row, col, 0);
                                solutionSteps.Add(sudokuBoard2.CreateNewBoardFromCells());

                                double endTime = Raylib.GetTime();
                                double elapsedTime = endTime - startTime;

                                
                                currentTime += elapsedTime;
                            }
                        }
                        //Console.WriteLine("Backtracking to " + row + " " + col);
                        return false; //Backtrack
                    }
                }
            }

            sudokuCompleted2 = true;
            return true; 
        }

        private static void DrawSudokuBoard()
        {

            /*
            for (int row1 = 0; row1 < 9; row1++)
            {
                for (int col1 = 0; col1 < 9; col1++)
                {
                    Console.Write(sudokuBoard2.GetBoardValue(row1,col1) + " ");
                }
                Console.WriteLine();
            }
            */

            for (int i = 0; i <= boardSize; i++)
            {
                int x = i * cellSize;
                int y = i * cellSize;

                Raylib.DrawLine(x + 10, 20, x + 10, 380, Color.BLACK);
                Raylib.DrawLine(10, y + 20, 370, y + 20, Color.BLACK);

                // adding drawings to the 3x3 squares for easier visualization.
                Raylib.DrawLine(3 * cellSize + 10, 20, 3 * cellSize + 10, 380, Color.BLUE);
                Raylib.DrawLine(3 * cellSize + 11, 20, 3 * cellSize + 11, 380, Color.DARKBLUE);
                Raylib.DrawLine(6 * cellSize + 10, 20, 6 * cellSize + 10, 380, Color.BLUE);
                Raylib.DrawLine(6 * cellSize + 11, 20, 6 * cellSize + 11, 380, Color.DARKBLUE);

                Raylib.DrawLine(10 , 3*cellSize +20, 370 , 3*cellSize+20, Color.BLUE);
                Raylib.DrawLine(10, 3 * cellSize + 21, 370, 3 * cellSize + 21, Color.DARKBLUE);
                Raylib.DrawLine(10 , 6*cellSize +20, 370, 6*cellSize+20, Color.BLUE);
                Raylib.DrawLine(10, 6 * cellSize + 21, 370, 6 * cellSize + 21, Color.DARKBLUE);

            }

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    
                    int x = col * cellSize + cellSize / 2;
                    int y = row * cellSize + cellSize / 2;

                    int number = sudokuBoard.GetBoardValue(row, col);
                    bool hideOrNot = sudokuBoard.isFilled(row, col);
                    bool userOrNot = sudokuBoard.isFilledUser(row, col);
                    if (number != 0 && !hideOrNot)
                    {
                        if (userOrNot)
                        {
                            if (safeForUser)
                            {
                                Raylib.DrawText(number.ToString(), x + 5, y + 10, 20, Color.DARKGRAY);
                            }
                            else if (!safeForUser)
                            {

                                Raylib.DrawText(number.ToString(), x + 5, y + 10, 20, Color.RED);
                            }
                        }
                        else
                        {
                            Raylib.DrawText(number.ToString(), x + 5, y + 10, 20, Color.BLACK);

                        }
                    }

                    if (sudokuCompleted) {
                        Raylib.DrawText(number.ToString(), x + 5, y + 10, 20, Color.GREEN);
                    }

                }
            }
            if (Raylib.GetMouseX() < boardSize * cellSize && Raylib.GetMouseY() < boardSize * cellSize && userCanPlay)
            {

                int clickedRow = Raylib.GetMouseY() / cellSize;
                int clickedCol = Raylib.GetMouseX() / cellSize;

                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    //Console.WriteLine("Mouse Left Button Pressed");
                    //Console.WriteLine($"Clicked Cell: Row {clickedRow}, Col {clickedCol}");

                    if ((sudokuBoard.isFilled(clickedRow,clickedCol) || sudokuBoard.isFilledUser(clickedRow,clickedCol )) && selectedNumber != -1)
                    {
                        //Console.WriteLine($"Setting Board Value: Row {clickedRow}, Col {clickedCol}, Number {selectedNumber}");
                        safeForUser =  sudokuBoard.IsValidPlacement(clickedRow, clickedCol, selectedNumber);
                        if (!safeForUser) {
                            sudokuBoard.incrementWrongNumber();
                        }
                        if (safeForUser) {
                            filledByUser += 1;
                        }
                        //Console.WriteLine($"safeForUser {safeForUser}");
                        sudokuBoard.SetBoardValue(clickedRow, clickedCol, selectedNumber);
                    }
                }
            }
        }
       
        }
    }

