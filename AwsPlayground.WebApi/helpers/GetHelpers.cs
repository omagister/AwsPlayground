using Amazon.SimpleNotificationService;
using Amazon.SQS;
using AwsPlayground.Common.helpers;

namespace AwsPlayground.WebApi.helpers
{
	public static class GetHelpers
	{
		public static SqsHelper GetSqsHelper(string port, string region)
		{
			var config = new AmazonSQSConfig { ServiceURL = $"http://localhost:{port}", AuthenticationRegion = region };
			var sqsClient = new AmazonSQSClient(config);
			return new SqsHelper(sqsClient);
		}

		public static SnsHelper GetSnsHelper(string port, string region)
		{
			var config = new AmazonSimpleNotificationServiceConfig { ServiceURL = $"http://localhost:{port}", AuthenticationRegion = region };
			var snsClient = new AmazonSimpleNotificationServiceClient(config);
			return new SnsHelper(snsClient);
		}
	}
}
