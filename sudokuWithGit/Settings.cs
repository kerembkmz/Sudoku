using System;
namespace SudokuVisualization;

public class Settings
{
	private static int userHiddenCellNumber = 40;
	private static int botHiddenCellNumber = 40;
	private static int timeForBotsEachMoveInSeconds = 3;
	private static int totalNumberForWrongTry_Users = 3;

	public static int GetUserHiddenCellNumber() {
		return userHiddenCellNumber;
	}

	public static int GetBotHiddenCellNumber() {
		return botHiddenCellNumber;
	}

	public static int GetTimeForBotsEachMoveInSeconds() {
		return timeForBotsEachMoveInSeconds;
	}

	public static int GetTotalNumberForWrongTryOfUsers() {
		return totalNumberForWrongTry_Users;
	}
}



