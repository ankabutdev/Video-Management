using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBotVideoSender.Services;

namespace TelegramBotVideoSender.Controllers;

public class WebHooksController : ControllerBase
{
    private readonly HandlerUpdateService _updateService;

    public WebHooksController(HandlerUpdateService updateService)
        => _updateService = updateService;


    [HttpPost]
    public async Task<IActionResult> Index([FromBody] Update update)
    {
        Console.WriteLine("Echoga ketayapti");
        await _updateService.HandleUpdateAsync(update);

        return Ok();
    }
}
