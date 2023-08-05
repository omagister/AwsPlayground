using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AwsPlayground.Common.helpers
{
	public class SqsHelper
	{
		private readonly IAmazonSQS _client;
		
		public SqsHelper(IAmazonSQS client = null)
		{
			_client = client ?? new AmazonSQSClient(new AmazonSQSConfig() { RegionEndpoint = RegionEndpoint.USEast1 });
		}

		public async Task<DeleteMessageResponse> DeleteAsync(string queueUrl, string receiptHandle)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(queueUrl) || string.IsNullOrWhiteSpace(receiptHandle)) return null;

				return await _client.DeleteMessageAsync(new DeleteMessageRequest()
				{
					QueueUrl = queueUrl,
					ReceiptHandle = receiptHandle
				});
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<SendMessageResponse> SendAsync(string queueUrl, string message)
		{
			var response = await _client.SendMessageAsync(new SendMessageRequest()
			{
				QueueUrl = queueUrl,
				MessageBody = message
			});

			return response;
		}

		public async Task<ReceiveMessageResponse> GetAsync(string queueUrl)
		{
			try
			{
				var response = await _client.ReceiveMessageAsync(queueUrl);

				return response;
			}
			catch (Exception ex)
			{

				throw ex;
			}
			
		}
	}
}
