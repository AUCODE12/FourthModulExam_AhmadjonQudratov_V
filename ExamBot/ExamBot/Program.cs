using ExamBot.Bll.Services;
using ExamBot.Dal;
using Microsoft.Extensions.DependencyInjection;

namespace ExamBot;

internal class Program
{
    static async Task Main(string[] args)
    {

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddScoped<IUserInfoService, UserInfoService>();
        serviceCollection.AddScoped<IBotUserService, BotUserService>();
        serviceCollection.AddSingleton<BotListenerService>();
        serviceCollection.AddSingleton<MainContext>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var botListenerService = serviceProvider.GetRequiredService<BotListenerService>();
        await botListenerService.StartBot();

        Console.ReadKey();
    }
}
