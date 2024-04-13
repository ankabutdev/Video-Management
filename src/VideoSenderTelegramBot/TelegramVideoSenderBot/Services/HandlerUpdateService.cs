using Refit;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotVideoSender.Interfaces;

namespace TelegramBotVideoSender.Services;

public class HandlerUpdateService
{
    private readonly ILogger<HandlerUpdateService> _logger;
    private readonly ITelegramBotClient _botClient;
    private IMyRefit _api;

    public HandlerUpdateService(ILogger<HandlerUpdateService> logger,
        ITelegramBotClient botClient)
    {
        _logger = logger;
        _botClient = botClient;
        _api = RestService.For<IMyRefit>("https://localhost:7265");
    }

    public async Task HandleUpdateAsync(Update update)
    {
        // Only process Message updates
        if (update.Message is not { } message)
            return;
        // Only process text messages
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        //var handler = update.Type switch
        //{
        //    UpdateType.Message => BotOnMessageReceived(update.Message, chatId, messageText),
        //    _ => UnknownUpdateTypeHandler(update)
        //};

        try
        {
            //await handler;
            if (messageText == "/start")
            {
                var resource = await _api.GetAllAsync();

                int[] numbers = new int[resource.LongCount()];
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = resource[i].Id;
                }

                var keyboardButtons = CreateButtons(numbers);

                var rows = SplitIntoRows(keyboardButtons, 3); // 3 buttons per row

                var replyKeyboardMarkup = new ReplyKeyboardMarkup(rows);
                replyKeyboardMarkup.ResizeKeyboard = true;

                var sentMessage = await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Choose a number.",
                    replyMarkup: replyKeyboardMarkup
                );

                Console.WriteLine($"Message sent with message ID: {sentMessage.MessageId}");
            }
            else
            {
                int selectednumber = int.Parse(messageText);
                var data = await _api.GetByIdAsync(selectednumber);

                var videoStream = await DownloadVideoAsync(data.VideoUrl);

                var inputFile = new InputFileStream(videoStream);

                var message1 = await _botClient.SendVideoAsync(chatId, inputFile);

                Console.WriteLine($"Video sent with message ID: {message.MessageId}");
            }
        }
        catch (Exception ex)
        {
            //await HandlerErrorAsync(ex);
            await _botClient.SendTextMessageAsync(
            chatId: chatId, text: $"Not in a correct format!\n\nPlease choose a number.\n{ex.Message}");
        }

    }

    private Task HandlerErrorAsync(Exception ex)
    {
        var errorMessage = ex switch
        {
            ApiRequestException apiRequestException =>
            $"Telegram API Error:\n{apiRequestException.ErrorCode}",
            _ => ex.ToString()
        };

        _logger.LogInformation(errorMessage);

        return Task.CompletedTask;
    }

    private Task UnknownUpdateTypeHandler(Update update)
    {
        _logger.LogInformation($"Unknown update type: {update.Type}");

        return Task.CompletedTask;
    }

    private async Task BotOnMessageReceived(Message? message, long chatId, string messageText)
    {
        _logger.LogInformation($"Message keldi: {message!.Type}");

        try
        {
            if (messageText == "/start")
            {
                var resource = await _api.GetAllAsync();

                int[] numbers = new int[resource.LongCount()];
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = resource[i].Id;
                }

                var keyboardButtons = CreateButtons(numbers);

                var rows = SplitIntoRows(keyboardButtons, 3); // 3 buttons per row

                var replyKeyboardMarkup = new ReplyKeyboardMarkup(rows);
                replyKeyboardMarkup.ResizeKeyboard = true;

                var sentMessage = await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Choose a number.",
                    replyMarkup: replyKeyboardMarkup
                );

                Console.WriteLine($"Message sent with message ID: {sentMessage.MessageId}");
            }
            else
            {
                int selectednumber = int.Parse(messageText);
                var data = await _api.GetByIdAsync(selectednumber);

                var videoStream = await DownloadVideoAsync(data.VideoUrl);

                var inputFile = new InputFileStream(videoStream);

                var message1 = await _botClient.SendVideoAsync(chatId, inputFile);

                Console.WriteLine($"Video sent with message ID: {message.MessageId}");
            }
        }
        catch
        {
            await _botClient.SendTextMessageAsync(
            chatId: chatId, text: "Not in a correct format!\n\nPlease choose a number.");
        }

    }

    // static fields
    private static async Task<Stream> DownloadVideoAsync(string videoUrl)
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(videoUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }
    }

    // Method to create buttons dynamically
    private static KeyboardButton[] CreateButtons(int[] numbers)
    {
        var buttons = new KeyboardButton[numbers.Length];
        for (int i = 0; i < numbers.Length; i++)
        {
            buttons[i] = new KeyboardButton(numbers[i].ToString());
        }
        return buttons;
    }

    // Helper method to split buttons into rows
    private static KeyboardButton[][] SplitIntoRows(KeyboardButton[] buttons, int buttonsPerRow)
    {
        var rows = new KeyboardButton[(int)Math.Ceiling(buttons.Length / (double)buttonsPerRow)][];
        for (var i = 0; i < rows.Length; i++)
        {
            rows[i] = new KeyboardButton[Math.Min(buttonsPerRow, buttons.Length - i * buttonsPerRow)];
            Array.Copy(buttons, i * buttonsPerRow, rows[i], 0, rows[i].Length);
        }
        return rows;
    }
}
