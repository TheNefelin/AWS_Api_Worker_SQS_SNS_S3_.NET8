namespace ClassLibrary_Infrastructure.Config;

public class AwsSettings
{
    public string SnsTopicArn { get; set; }
    public string SqsUrl { get; set; }
    public string S3BucketName { get; set; }
}
