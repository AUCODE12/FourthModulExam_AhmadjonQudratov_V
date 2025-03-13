using ExamBot.Dal.Entities;

namespace ExamBot.Bll.Services;

public interface IUserInfoService
{
    Task<long> AddUserInfoAsync(UserInfo userInfo);
    Task<long> GetUserInfoIdByBotUserIdAsync(long botUserId);
    Task<UserInfo> GetUserInfoByBotUserIdAsync(long botUserId);
    Task DeleteUserInfoAsync(long userInfoId);
}