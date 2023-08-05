using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace AwsPlayground.Common.helpers
{
	public class CredentialHelper
	{
		public CredentialHelper(string filePath, string awsUser)
		{
			var chain = new CredentialProfileStoreChain(filePath);
			chain.TryGetAWSCredentials(awsUser, out var credentials);
			Credentials = credentials;
		}
		public AWSCredentials Credentials { get; set; }
	}
}
