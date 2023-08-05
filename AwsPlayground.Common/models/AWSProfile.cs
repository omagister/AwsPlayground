namespace AwsPlayground.Common.models
{
	public class AWSProfile
	{
		public AWS AWS { get; set; }
	}
	public class AWS
	{
		public string Profile { get; set; }
		public string ProfilesLocation { get; set; }
		public string Region { get; set; }
	}
}
