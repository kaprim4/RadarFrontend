namespace FileProcessor
{
    public interface IFileProcessor
    {
        DeploymentData Process(string filePath);
    }
}