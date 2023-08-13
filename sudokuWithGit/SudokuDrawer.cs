using System;
using Raylib_cs;
using sudoku;

namespace SudokuVisualization
{
    public class SudokuDrawer
    {
        private  Board sudokuBoard = new Board();
        private Board randomizerBoard = new Board();
        private  Board sudokuBoard2 = new Board();
        private  int screenHeight = 500;
        private  int boardSize = 9;
        private  int cellSize = 40;
        private  int selectedNumber = -1;
        private  bool safeForUser = false;
        private  int totalTryNumber = Settings.GetTotalNumberForWrongTryOfUsers(); 
        private  bool userCanPlay = true;
        private  bool sudokuCompleted = false;
        private  bool sudokuCompleted2 = false;
        private  int filledByUser = 0;
        private  int totalHiddenNumUser = Settings.GetUserHiddenCellNumber();
        private int totalHiddenNumBot = Settings.GetBotHiddenCellNumber();
        double moveInterval = Settings.GetTimeForBotsEachMoveInSeconds();
         List<int[,]> solutionSteps = new List<int[,]>();

        public SudokuDrawer()
        {
            sudokuBoard.FillRandomBoard();
            randomizerBoard.FillRandomBoard();
            sudokuBoard.hideRandomBoard(totalHiddenNumUser);
            sudokuBoard2 = sudokuBoard.ReturnBoardFromCells(randomizerBoard);
            sudokuBoard2.hideRandomBoard(totalHiddenNumBot);
        }

        public  void DrawFalseTryOutOfOppurtunity()
        {

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

        public  void DrawGameIsCompletedWithSuccess()
        {
            if (totalHiddenNumUser == filledByUser)
            {
                sudokuCompleted = true;
                string output = "Sudoku completed succesfully";
                Raylib.DrawText(output, 400, 400, 20, Color.BLACK);
            }
        }







        public  void DrawNumberSelectionUI()
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


        public  void DrawSudokuBoard2()
        {


            for (int i = 0; i <= boardSize; i++)
            {
                int x = i * cellSize;
                int y = i * cellSize;

                Raylib.DrawLine(400 + x, 20, 400 + x, 380, Color.BLACK);
                Raylib.DrawLine(400, y + 20, 760, y + 20, Color.BLACK);

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
                            else
                            {
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


        public  bool SolveSudokuStepByStep()
        {
            

            double currentTime = 0;
             

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

                                if (SolveSudokuStepByStep())
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

        public  void DrawSudokuBoard()
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

                Raylib.DrawLine(10, 3 * cellSize + 20, 370, 3 * cellSize + 20, Color.BLUE);
                Raylib.DrawLine(10, 3 * cellSize + 21, 370, 3 * cellSize + 21, Color.DARKBLUE);
                Raylib.DrawLine(10, 6 * cellSize + 20, 370, 6 * cellSize + 20, Color.BLUE);
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

                    if (sudokuCompleted)
                    {
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

                    if ((sudokuBoard.isFilled(clickedRow, clickedCol) || sudokuBoard.isFilledUser(clickedRow, clickedCol)) && selectedNumber != -1)
                    {
                        //Console.WriteLine($"Setting Board Value: Row {clickedRow}, Col {clickedCol}, Number {selectedNumber}");
                        safeForUser = sudokuBoard.IsValidPlacement(clickedRow, clickedCol, selectedNumber);
                        if (!safeForUser)
                        {
                            sudokuBoard.incrementWrongNumber();
                        }
                        if (safeForUser)
                        {
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
