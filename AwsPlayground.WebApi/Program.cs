using AwsPlayground.WebApi.helpers;
using AwsPlayground.WebApi.models;
using Microsoft.AspNetCore.Mvc;

var localstackSettings = new LocalstackSettings();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
	options.AddPolicy("MyAllowedOrigins",
		policy =>
		{
			policy.WithOrigins("https://localhost:4566") // note the port is included 
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseCors();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyAllowedOrigins");

app.MapGet("/aws/credentials", async () =>
{
	var credentials = await ConfigHelper.GetAsyncCredentials();
	
	return credentials.GetCredentials();
})
.WithName("GetAwsCredentials");

app.MapPost("/localstack/settings", async ([FromHeader(Name = "port")] string port, [FromHeader(Name = "region")] string region) =>
{
	localstackSettings.Port = port;
	localstackSettings.Region = region;

	return localstackSettings;
})
.WithName("PostLocalstackSettings");

app.MapGet("/aws/sqs/message", async ([FromHeader(Name = "queueName")] string queueName) =>
{
	var sqsHelper = GetHelpers.GetSqsHelper(localstackSettings.Port, localstackSettings.Region);
	var queueUrl = $"http://localhost:{localstackSettings.Port}/000000000000/{queueName}";
	var messages = await sqsHelper.GetAsync(queueUrl);

	return messages;
})
.WithName("GetAwsSqsMessage");

app.MapPost("/aws/sqs/message", async ([FromHeader(Name = "queueName")] string queueName, [FromHeader(Name = "message")] string message) =>
{
	var sqsHelper = GetHelpers.GetSqsHelper(localstackSettings.Port, localstackSettings.Region);
	var queueUrl = $"http://localhost:{localstackSettings.Port}/000000000000/{queueName}";
	var messages = await sqsHelper.SendAsync(queueUrl,message);

	return messages;
})
.WithName("PostAwsSqsMessage");

app.MapDelete("/aws/sqs/message", async ([FromHeader(Name = "queueName")] string queueName, [FromHeader(Name = "receiptHandle")] string receiptHandle) =>
{
	var sqsHelper = GetHelpers.GetSqsHelper(localstackSettings.Port, localstackSettings.Region);
	var queueUrl = $"http://localhost:{localstackSettings.Port}/000000000000/{queueName}";
	var messages = sqsHelper.DeleteAsync(queueUrl, receiptHandle);

	return messages;
})
.WithName("DeleteAwsSqsMessage");

app.MapGet("/aws/sns/message", async ([FromHeader(Name = "topic")] string topic, [FromHeader(Name = "message")] string message) =>
{
	var snsHelper = GetHelpers.GetSnsHelper(localstackSettings.Port, localstackSettings.Region);
	var topicArn = $"arn:aws:sns:{localstackSettings.Region}:000000000000:{topic}";
	var response = await snsHelper.SendSimpleMessageAsync(topicArn, message);

	return response;
})
.WithName("GetAwsSnsMessage");

app.Run();
