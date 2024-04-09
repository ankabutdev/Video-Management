using Refit;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VideoSenderBot;

var botClient = new TelegramBotClient("6702931513:AAF6xid2RjeDeeYc3XSGS4QjE8QtWQ0qVhg");

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
};
HttpClient httpClient = new();
var api = RestService.For<ITestRefit>("https://localhost:7265");

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;
    var firstName = message.From!.FirstName;
    var username = message.From.Username;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    try
    {
        if (messageText == "/start")
        {
            var current = Directory.GetCurrentDirectory();

            var resource = await api.GetAllAsync();

            int[] numbers = new int[resource.LongCount()];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = resource[i].Id;
            }

            var keyboardButtons = CreateButtons(numbers);

            var rows = SplitIntoRows(keyboardButtons, 3); // 3 buttons per row

            var replyKeyboardMarkup = new ReplyKeyboardMarkup(rows);

            var sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Choose a number",
                replyMarkup: replyKeyboardMarkup
            );

            Console.WriteLine($"Message sent with message ID: {sentMessage.MessageId}");
        }
        else
        {
            int selectednumber = int.Parse(messageText);
            var data = await api.GetByIdAsync(selectednumber);

            var videoStream = await DownloadVideoAsync(data.VideoUrl);

            var inputFile = new InputFileStream(videoStream);

            var message1 = await botClient.SendVideoAsync(chatId, inputFile);

            Console.WriteLine($"Video sent with message ID: {message.MessageId}");

        }

    }
    catch (Exception ex)
    {
        await Console.Out.WriteLineAsync($"Error: {ex.Message}");
    }

}

static async Task<Stream> DownloadVideoAsync(string videoUrl)
{
    using (var httpClient = new HttpClient())
    {
        var response = await httpClient.GetAsync(videoUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync();
    }
}

// Method to create buttons dynamically
static KeyboardButton[] CreateButtons(int[] numbers)
{
    var buttons = new KeyboardButton[numbers.Length];
    for (int i = 0; i < numbers.Length; i++)
    {
        buttons[i] = new KeyboardButton(numbers[i].ToString());
    }
    return buttons;
}

// Helper method to split buttons into rows
static KeyboardButton[][] SplitIntoRows(KeyboardButton[] buttons, int buttonsPerRow)
{
    var rows = new KeyboardButton[(int)Math.Ceiling(buttons.Length / (double)buttonsPerRow)][];
    for (var i = 0; i < rows.Length; i++)
    {
        rows[i] = new KeyboardButton[Math.Min(buttonsPerRow, buttons.Length - i * buttonsPerRow)];
        Array.Copy(buttons, i * buttonsPerRow, rows[i], 0, rows[i].Length);
    }
    return rows;
}
Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}