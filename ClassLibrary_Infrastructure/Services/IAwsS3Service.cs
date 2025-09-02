namespace ClassLibrary_Infrastructure.Services;

public interface IAwsS3Service
{
    Task<string> SavePdfToBucketAsync(Stream pdfStream);
    Task<Stream> GetFileStreamFromBucketAsync(string fileName);
    string GeneratePreSignedUrl(string fileName, TimeSpan duration);
}
