using Refit;

namespace VideoSenderBot;

public interface ITestRefit
{
    [Get("/api/products/{id}")]
    Task<ResponseVideo> GetByIdAsync(int id);

    [Get("/api/products")]
    Task<IList<ResponseVideo>> GetAllAsync();

    [Get("/media/videos/")]
    Task<string> RootPath();

    [Get("/api/filename")]
    Task<Stream> GetVideoByFileNameAsync(string fileName);
}


public class ResponseVideo
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string VideoUrl { get; set; } = string.Empty;

    public int SortNumber { get; set; }
}