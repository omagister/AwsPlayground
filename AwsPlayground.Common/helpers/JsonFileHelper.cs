using System.Text.Json;

namespace AwsPlayground.Common.helpers
{
	public static class JsonFileHelper
	{
		public static async Task<T> ReadAsync<T>(string filePath)
		{
			try
			{
				var path = Environment.CurrentDirectory;

				using FileStream stream = File.OpenRead($"{path}\\{filePath}");
				return await JsonSerializer.DeserializeAsync<T>(stream);
			}
			catch (Exception ex)
			{

				throw ex;
			}
			
		}
	}
}
