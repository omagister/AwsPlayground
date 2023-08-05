using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace AwsPlayground.Common.helpers
{
	public class SnsHelper
	{
		private readonly IAmazonSimpleNotificationService _client;

		public SnsHelper(IAmazonSimpleNotificationService client)
		{
			_client = client;
		}

		public async Task<PublishResponse> SendSimpleMessageAsync(string topicArn, string message)
		{
			if (string.IsNullOrEmpty(topicArn)) throw new ArgumentNullException(topicArn, constants.MessagesConst.SnsTopicNotInformed);

			PublishResponse publish = await SendMessageAsync(new PublishRequest()
			{
				TopicArn = topicArn,
				Message = message
			});

			return publish;
		}
		public async Task<PublishResponse> SendMessageAsync(string topicArn, PublishRequest message, Dictionary<string, string> messageAttributes)
		{
			message.TopicArn = topicArn;
			message.MessageAttributes = GetMessageAttributes(messageAttributes);

			PublishResponse publish = await SendMessageAsync(message);

			return publish;
		}

		private async Task<PublishResponse> SendMessageAsync(PublishRequest message)
		{
			try
			{
				PublishResponse publish = await _client.PublishAsync(message);

				return publish;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		}

		private Dictionary<string, MessageAttributeValue> GetMessageAttributes(Dictionary<string, string> messageAttributes)
		{
			var attributes = new Dictionary<string, MessageAttributeValue>();

			foreach (var item in messageAttributes)
			{
				if (string.IsNullOrWhiteSpace(item.Value)) continue;
				attributes.Add(item.Key, new MessageAttributeValue() { DataType = "String", StringValue = item.Value });
			}

			return attributes;
		}
	}
}
