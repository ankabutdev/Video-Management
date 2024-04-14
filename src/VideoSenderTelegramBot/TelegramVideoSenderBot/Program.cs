using Telegram.Bot;
using TelegramBotVideoSender.Models;
using TelegramBotVideoSender.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var config = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
builder.Services.AddHttpClient("webhook").AddTypedClient<ITelegramBotClient>(
    httpClient => new TelegramBotClient(config!.Token, httpClient));

// Add hosted service and scoped service
builder.Services.AddHostedService<ConfigureWebHook>();
builder.Services.AddScoped<HandlerUpdateService>();
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

app.UseRouting();
app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "webhook",
        pattern: $"bot/{config!.Token}",
        new { controller = "WebHooks", action = "Index" });

    endpoints.MapControllers();
});
app.Run();