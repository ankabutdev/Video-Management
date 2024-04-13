using Refit;
using TelegramBotVideoSender.Models;

namespace TelegramBotVideoSender.Interfaces;

public interface IMyRefit
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