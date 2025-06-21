using System.Collections;
using Ge.Singular.Roulette.Procurios.Public;

namespace ge.singular.roulette;

public class CheckBets
{
	private static List<string> VALID_BET_TYPES = new List<string>
	{
		"n", "v", "h", "s", "c", "d", "row", "twelve", "half", "even",
		"odd", "red", "black"
	};

	private static int[] BOARD_ROW1 = new int[12]
	{
		1, 4, 7, 10, 13, 16, 19, 22, 25, 28,
		31, 34
	};

	private static int[] BOARD_ROW2 = new int[12]
	{
		2, 5, 8, 11, 14, 17, 20, 23, 26, 29,
		32, 35
	};

	private static int[] BOARD_ROW3 = new int[12]
	{
		3, 6, 9, 12, 15, 18, 21, 24, 27, 30,
		33, 36
	};

	private static int[] BOARD_TWELVE1 = new int[12]
	{
		1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
		11, 12
	};

	private static int[] BOARD_TWELVE2 = new int[12]
	{
		13, 14, 15, 16, 17, 18, 19, 20, 21, 22,
		23, 24
	};

	private static int[] BOARD_TWELVE3 = new int[12]
	{
		25, 26, 27, 28, 29, 30, 31, 32, 33, 34,
		35, 36
	};

	private static int[] BOARD_HALF1 = new int[18]
	{
		1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
		11, 12, 13, 14, 15, 16, 17, 18
	};

	private static int[] BOARD_HALF2 = new int[18]
	{
		19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
		29, 30, 31, 32, 33, 34, 35, 36
	};

	private static int[] BOARD_EVEN = new int[18]
	{
		2, 4, 6, 8, 10, 12, 14, 16, 18, 20,
		22, 24, 26, 28, 30, 32, 34, 36
	};

	private static int[] BOARD_ODD = new int[18]
	{
		1, 3, 5, 7, 9, 11, 13, 15, 17, 19,
		21, 23, 25, 27, 29, 31, 33, 35
	};

	private static int[] BOARD_RED = new int[18]
	{
		1, 3, 5, 7, 9, 12, 14, 16, 18, 19,
		21, 23, 25, 27, 30, 32, 34, 36
	};

	private static int[] BOARD_BLACK = new int[18]
	{
		2, 4, 6, 8, 10, 11, 13, 15, 17, 20,
		22, 24, 26, 28, 29, 31, 33, 35
	};

	private static int[][] BOARD_SQUARE = new int[25][]
	{
		new int[1] { -1 },
		new int[3] { 0, 2, 3 },
		new int[4] { 2, 3, 5, 6 },
		new int[4] { 5, 6, 8, 9 },
		new int[4] { 8, 9, 11, 12 },
		new int[4] { 11, 12, 14, 15 },
		new int[4] { 14, 15, 17, 18 },
		new int[4] { 17, 18, 20, 21 },
		new int[4] { 20, 21, 23, 24 },
		new int[4] { 23, 24, 26, 27 },
		new int[4] { 26, 27, 29, 30 },
		new int[4] { 29, 30, 32, 33 },
		new int[4] { 32, 33, 35, 36 },
		new int[3] { 0, 1, 2 },
		new int[4] { 1, 2, 4, 5 },
		new int[4] { 4, 5, 7, 8 },
		new int[4] { 7, 8, 10, 11 },
		new int[4] { 10, 11, 13, 14 },
		new int[4] { 13, 14, 16, 17 },
		new int[4] { 16, 17, 19, 20 },
		new int[4] { 19, 20, 22, 23 },
		new int[4] { 22, 23, 25, 26 },
		new int[4] { 25, 26, 28, 29 },
		new int[4] { 28, 29, 31, 32 },
		new int[4] { 31, 32, 34, 35 }
	};

	public static IsBetValidResponse IsValid(string bet)
	{
		IsBetValidResponse isBetValidResponse = new IsBetValidResponse();
		isBetValidResponse.setIsValid(isValid: false);
		isBetValidResponse.setBetAmount(0L);
		try
		{
			ArrayList arrayList = (ArrayList)JSON.JsonDecode(bet);
			if (arrayList == null || arrayList.Count < 1)
			{
				return isBetValidResponse;
			}
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			for (int i = 0; i < arrayList.Count; i++)
			{
				Hashtable hashtable = arrayList[i] as Hashtable;
				if (!VALID_BET_TYPES.Contains(hashtable["T"].ToString()))
				{
					return isBetValidResponse;
				}
				if (!IsValidTypeId(hashtable["T"].ToString(), int.Parse(hashtable["I"].ToString())))
				{
					return isBetValidResponse;
				}
				int num = int.Parse(hashtable["K"].ToString());
				int num2 = int.Parse(hashtable["C"].ToString());
				if (num2 < 1 || num2 > 200)
				{
					return isBetValidResponse;
				}
				string key = hashtable["T"].ToString() + "_" + hashtable["I"].ToString();
				if (!dictionary.ContainsKey(key))
				{
					dictionary[key] = num2;
				}
				else
				{
					dictionary[key] = int.Parse(dictionary[key].ToString()) + num2;
				}
				isBetValidResponse.setBetAmount(isBetValidResponse.getBetAmount() + num2 * num);
			}
			foreach (KeyValuePair<string, int> item in dictionary)
			{
				string[] array = item.Key.ToString().Split(new char[1] { '_' });
				if (GetMinBetByType(array[0]) > item.Value)
				{
					return isBetValidResponse;
				}
				if (GetMaxBetByType(array[0], array[1]) < item.Value)
				{
					return isBetValidResponse;
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
			return isBetValidResponse;
		}
		isBetValidResponse.setIsValid(isValid: true);
		return isBetValidResponse;
	}

	public static int EstimateWin(string bet, int winnum)
	{
		int num = 0;
		try
		{
			ArrayList arrayList = (ArrayList)JSON.JsonDecode(bet);
			if (arrayList == null || arrayList.Count < 1)
			{
				return -1;
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				Hashtable hashtable = arrayList[i] as Hashtable;
				int num2 = int.Parse(hashtable["I"].ToString());
				int num3 = int.Parse(hashtable["K"].ToString());
				int num4 = int.Parse(hashtable["C"].ToString());
				int num5 = 0;
				switch (hashtable["T"].ToString())
				{
				case "n":
					if (num2 == winnum)
					{
						num5 = num4 * 36;
					}
					break;
				case "v":
				{
					int num6 = num2 - 3;
					if (num6 < 0)
					{
						num6 = 0;
					}
					if (num2 == winnum || num6 == winnum)
					{
						num5 = num4 * 18;
					}
					break;
				}
				case "h":
					if (num2 == winnum || num2 + 1 == winnum)
					{
						num5 = num4 * 18;
					}
					break;
				case "s":
					if (IsInArray(BOARD_SQUARE[num2], winnum))
					{
						num5 = num4 * (36 / BOARD_SQUARE[num2].Length);
					}
					break;
				case "c":
				{
					int num6 = (num2 - 1) * 3;
					if (num6 + 1 == winnum || num6 + 2 == winnum || num6 + 3 == winnum)
					{
						num5 = num4 * 12;
					}
					break;
				}
				case "d":
				{
					if (num2 == 1)
					{
						if (0 <= winnum && winnum <= 3)
						{
							num5 = num4 * 9;
						}
						break;
					}
					int num6 = (num2 - 1) * 3;
					if (num6 - 2 <= winnum && winnum <= num6 + 3)
					{
						num5 = num4 * 6;
					}
					break;
				}
				case "row":
					switch (num2)
					{
					case 1:
						if (IsInArray(BOARD_ROW1, winnum))
						{
							num5 = num4 * 3;
						}
						break;
					case 2:
						if (IsInArray(BOARD_ROW2, winnum))
						{
							num5 = num4 * 3;
						}
						break;
					case 3:
						if (IsInArray(BOARD_ROW3, winnum))
						{
							num5 = num4 * 3;
						}
						break;
					}
					break;
				case "twelve":
					switch (num2)
					{
					case 1:
						if (IsInArray(BOARD_TWELVE1, winnum))
						{
							num5 = num4 * 3;
						}
						break;
					case 2:
						if (IsInArray(BOARD_TWELVE2, winnum))
						{
							num5 = num4 * 3;
						}
						break;
					case 3:
						if (IsInArray(BOARD_TWELVE3, winnum))
						{
							num5 = num4 * 3;
						}
						break;
					}
					break;
				case "half":
					switch (num2)
					{
					case 1:
						if (IsInArray(BOARD_HALF1, winnum))
						{
							num5 = num4 * 2;
						}
						break;
					case 2:
						if (IsInArray(BOARD_HALF2, winnum))
						{
							num5 = num4 * 2;
						}
						break;
					}
					break;
				case "even":
					if (IsInArray(BOARD_EVEN, winnum))
					{
						num5 = num4 * 2;
					}
					break;
				case "odd":
					if (IsInArray(BOARD_ODD, winnum))
					{
						num5 = num4 * 2;
					}
					break;
				case "red":
					if (IsInArray(BOARD_RED, winnum))
					{
						num5 = num4 * 2;
					}
					break;
				case "black":
					if (IsInArray(BOARD_BLACK, winnum))
					{
						num5 = num4 * 2;
					}
					break;
				}
				if (num5 > 0)
				{
					num += num5 * num3;
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
			return -1;
		}
		return num;
	}

	private static bool IsInArray(int[] a, int id)
	{
		for (int i = 0; i < a.Length; i++)
		{
			if (a[i] == id)
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsValidTypeId(string type, int id)
	{
		return type switch
		{
			"n" => id >= 0 && id <= 36, 
			"v" => id >= 1 && id <= 36, 
			"h" => id >= 1 && id <= 36 && id % 3 != 0, 
			"s" => id >= 1 && id <= 24, 
			"c" => id >= 1 && id <= 12, 
			"d" => id >= 1 && id <= 12, 
			"row" => id >= 1 && id <= 3, 
			"twelve" => id >= 1 && id <= 3, 
			"half" => id == 1 || id == 2, 
			"even" => id == 1, 
			"odd" => id == 1, 
			"red" => id == 1, 
			"black" => id == 1, 
			_ => false, 
		};
	}

	private static int GetMinBetByType(string type)
	{
		switch (type)
		{
		case "n":
		case "v":
		case "h":
		case "s":
		case "c":
		case "d":
			return 1;
		case "row":
		case "twelve":
			return 5;
		case "half":
		case "even":
		case "odd":
		case "red":
		case "black":
			return 10;
		default:
			return 1;
		}
	}

	private static int GetMaxBetByType(string type, string id)
	{
		switch (type)
		{
		case "n":
			return 20;
		case "v":
		case "h":
			return 40;
		case "s":
			if (id == "1" || id == "13")
			{
				return 60;
			}
			return 80;
		case "c":
			return 60;
		case "d":
			if (id == "1")
			{
				return 80;
			}
			return 120;
		case "row":
		case "twelve":
			return 100;
		case "half":
		case "even":
		case "odd":
		case "red":
		case "black":
			return 200;
		default:
			return 0;
		}
	}
}
