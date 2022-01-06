namespace SimpleLocalizationSystem.Editor
{
	public static class Result
	{
		public const int OK = 0;
		public const int ERROR_KEY_ALREADY_EXIST = -1;
		public const int ERROR_LANGUAGE_ALREADY_EXISTS = -2;
		public const int ERROR_CULTURE_NOT_FOUND = -3;

		public static bool Succeeded(int result)
		{
			return result >= 0;
		}
	}
}