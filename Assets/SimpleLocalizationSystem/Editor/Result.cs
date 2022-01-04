namespace SimpleLocalizationSystem.Editor
{
	public static class Result
	{
		public const int OK = 0;
		public const int ERROR_KEY_ALREADY_EXIST = -1;

		public static bool Succeeded(int result)
		{
			return result >= 0;
		}
	}
}