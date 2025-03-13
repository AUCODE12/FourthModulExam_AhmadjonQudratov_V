﻿using ExamBot.Dal.Entities;

namespace ExamBot.Bll.Services;

public interface IBotUserService
{
    Task AddUserAsync(BotUser user);
    Task<List<BotUser>> GetAllUsersAsync();
    Task<BotUser> GetBotUserByTelegramUserIdAsync(long telegramUserId);
    Task<long> GetBotUserIdByTelegramUserIdAsync(long telegramUserId);
}