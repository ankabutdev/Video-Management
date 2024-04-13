namespace TelegramBotVideoSender.Models;

public class ResponseVideo
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string VideoUrl { get; set; } = string.Empty;

    public int SortNumber { get; set; }
}
