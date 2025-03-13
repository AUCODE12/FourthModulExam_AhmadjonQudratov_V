using ExamBot.Dal;
using ExamBot.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamBot.Bll.Services;

public class UserInfoService : IUserInfoService
{
    private readonly MainContext mainContext;

    public UserInfoService(MainContext mainContext)
    {
        this.mainContext = mainContext;
    }

    public async Task<long> AddUserInfoAsync(UserInfo userInfo)
    {
        try
        {
            await mainContext.UserInfos.AddAsync(userInfo);
            await mainContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0l;
        }

        return userInfo.UserInfoId;
    }

    public async Task DeleteUserInfoAsync(long userInfoId)
    {
        var userInfo = await GetUserInfoByBotUserIdAsync(userInfoId);
        mainContext.UserInfos.Remove(userInfo);
        await mainContext.SaveChangesAsync();
    }

    public async Task<UserInfo> GetUserInfoByBotUserIdAsync(long botUserId)
    {
        var userInfo = await mainContext.UserInfos.FirstOrDefaultAsync(ui => ui.BotUserId == botUserId);
        return userInfo;
    }

    public async Task<long> GetUserInfoIdByBotUserIdAsync(long botUserId)
    {
        return (await GetUserInfoByBotUserIdAsync(botUserId))?.UserInfoId ?? 0L;
    }
}
