using Amazon.Runtime;
using AwsPlayground.Common.helpers;
using AwsPlayground.Common.models;

namespace AwsPlayground.WebApi.helpers
{
	public static class ConfigHelper
	{
		public static async Task<AWSCredentials> GetAsyncCredentials()
		{
			AWSProfile awsProfile = await JsonFileHelper.ReadAsync<AWSProfile>("aws-profile.json");
			CredentialHelper credentialHelper = new CredentialHelper(awsProfile.AWS.ProfilesLocation, awsProfile.AWS.Profile);
			return credentialHelper.Credentials;
		}
	}
}
