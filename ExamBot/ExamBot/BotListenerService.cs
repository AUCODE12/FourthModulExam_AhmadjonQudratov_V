using ExamBot.Bll.Services;
using ExamBot.Dal.Entities;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace ExamBot;

public class BotListenerService
{
    private static string botToken = "7203124088:AAH8zi7YPtrqEbeVIpAX_BwOnYllK5rAuyc";
    private TelegramBotClient botClient = new TelegramBotClient(botToken);
    private readonly IBotUserService botUserService;
    private readonly IUserInfoService userInfoService;

    public BotListenerService(IBotUserService userService, IUserInfoService userInfoService)
    {
        this.botUserService = userService;
        this.userInfoService = userInfoService;
    }

    public async Task StartBot()
    {
        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync
            );

        Console.WriteLine("Bot is runing");

        Console.ReadKey();
    }
    private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message)
        {
            var user = update.Message.Chat;
            var message = update.Message;
            var botUserId = await botUserService.GetBotUserIdByTelegramUserIdAsync(user.Id);

            if (message.Text == "/start")
            {
                await SendStartMenu(bot, user.Id);

                return;
            }

            if (message.Text == "Main menu")
            {
                var menu = new ReplyKeyboardMarkup(new[]
                {
                    new[]
                    {
                        new KeyboardButton("Fill Data"),
                        new KeyboardButton("Get Data"),
                        new KeyboardButton("Delete Data")
                    },
                })
                {
                    ResizeKeyboard = true
                };

                await botClient.SendTextMessageAsync(
                    chatId: user.Id,
                    text: "You get main menu",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: menu
                );

                return;
            }

            if (message.Text == "Fill Data")
            {
                var userInfo = await userInfoService.GetUserInfoByBotUserIdAsync(botUserId);

                var menu = new ReplyKeyboardMarkup()
                {
                    ResizeKeyboard = true
                };

                var textOfUserInfo = "";

                if (userInfo == null)
                {
                    textOfUserInfo = "Your info not found press\nCreate user info button to create";

                    menu.AddButtons(
                        new KeyboardButton("Fill user info"),
                        new KeyboardButton("Main menu"));
                }
                else
                {
                    textOfUserInfo = $"Your personal info below\n" +
                                     $"UserId      : {userInfo.UserInfoId}\n" +
                                     $"Firstname   : {userInfo.FirstName}\n" +
                                     $"Lastname    : {userInfo.LastName}\n" +
                                     $"Phonenumber : {userInfo.PhoneNumber}\n" +
                                     $"Email       : {userInfo.Email}\n" +
                                     $"Address     : {userInfo.Address}\n" +
                                     $"DateOfBirth     : {userInfo.DateOfBirth}\n";

                    menu.AddButtons(
                        new KeyboardButton("Main menu"));
                }

                await bot.SendTextMessageAsync(
                chatId: user.Id,
                text: textOfUserInfo,
                parseMode: ParseMode.Markdown,
                replyMarkup: menu
                );

                return;
            }

            if (message.Text == "Fill user info")
            {
                var userInfoText = "Please enter your details in the following format:\n\n" +
                      "*First Name*\n" +
                      "*Last Name*\n" +
                      "*Email*\n" +
                      "*Phone Number*\n" +
                      "*Address*\n" +
                      "*Summary*\n\n" +
                      "Example:\n\n" +
                      "Fill user info\nJohn\nDoe\njohn.doe@example.com\n+1234567890\n123 Main St, City, Country\n2024/02/16";

                await bot.SendTextMessageAsync(
                chatId: user.Id,
                text: userInfoText,
                parseMode: ParseMode.Markdown
                );

                return;
            }

            if (message.Text.StartsWith("Fill user info"))
            {
                var userInfotext = message.Text;
                var data = userInfotext.Split("\n");
                var userInfo = new UserInfo()
                {
                    FirstName = data[1].Trim(),
                    LastName = data[2].Trim(),
                    Email = data[3].Trim(),
                    PhoneNumber = data[4].Trim(),
                    Address = data[5].Trim(),
                    DateOfBirth = data[6].Trim(),
                    BotUserId = botUserId
                };

                var resFromAddUserInfoAsync = await userInfoService.AddUserInfoAsync(userInfo);

                var textToBotUser = "";

                if (resFromAddUserInfoAsync == 0)
                {
                    textToBotUser = "Error occuried while saving";
                }
                else
                {
                    textToBotUser = "Successfully saved";
                }

                await bot.SendTextMessageAsync(
                chatId: user.Id,
                text: textToBotUser,
                parseMode: ParseMode.Markdown
                );

                await SendStartMenu(bot, user.Id);
            }
            if (message.Text == "Delete all user info")
            {
                await bot.SendTextMessageAsync(
                    chatId: user.Id,
                    text: "Delete user info enter like this format\n\nDelete",
                    parseMode: ParseMode.Markdown
                );

                return;
            }

            if (message.Text.StartsWith("Delete"))
            {
                //var deletionId = long.Parse(message.Text.Substring(20).Trim());

                await userInfoService.DeleteUserInfoAsync(1);

                await bot.SendTextMessageAsync(
                chatId: user.Id,
                text: "User Info is deleted",
                parseMode: ParseMode.Markdown
                );

                return;
            }

        }

        else if (update.Type == UpdateType.CallbackQuery)
        {
            var id1 = update.CallbackQuery.Id;
            var id2 = update.CallbackQuery.InlineMessageId;
            var id = update.CallbackQuery.From.Id;

            CallbackQuery res = update.CallbackQuery;

            var rep = update.CallbackQuery.Data;


            await bot.SendTextMessageAsync(id, $"your option : {update.CallbackQuery.Data}");
        }
    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception.Message);
    }

    private static async Task SendStartMenu(ITelegramBotClient botClient, long userId)
    {
        var menu = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("Fill Data"),
                new KeyboardButton("Get Data"),
                new KeyboardButton("Delete Data")
            },
        })
        {
            ResizeKeyboard = true
        };



        var introText = @"Salom";

        await botClient.SendTextMessageAsync(
            chatId: userId,
            text: introText,
            parseMode: ParseMode.Markdown,
            replyMarkup: menu
        );
    }
}