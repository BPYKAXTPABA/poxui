using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Linq;
using System.Threading.Tasks;

namespace HAPYKAX.Commands
{
    public class SlashCommands : ApplicationCommandModule
    {
        private static Dictionary<ulong, int> _warns = new Dictionary<ulong, int>();

        
        // 1. Мут в чате
        [SlashCommand("mutechat", "Мут пользователя в чате")]
        public async Task MuteChat(InteractionContext ctx, [Option("user", "Пользователь для мута")] DiscordUser user)
        {
            var member = await ctx.Guild.GetMemberAsync(user.Id);
            var muteRole = ctx.Guild.Roles.Values.FirstOrDefault(r => r.Name == "MutedFromChat");

            if (muteRole == null)
            {
                muteRole = await ctx.Guild.CreateRoleAsync("MutedFromChat", Permissions.None, DiscordColor.DarkGray, false, true);
                foreach (var channel in ctx.Guild.Channels.Values)
                {
                    await channel.AddOverwriteAsync(muteRole, Permissions.None, Permissions.SendMessages);
                }
            }

            await member.GrantRoleAsync(muteRole);
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, 
                new DiscordInteractionResponseBuilder().WithContent($"{member.DisplayName} замучен в чате."));
        }

        // 2. Полный мут в войсе (кик + запрет входа)
        [SlashCommand("mutevoice", "Отключить пользователя из войс-чата и запретить вход")]
        public async Task MuteVoice(InteractionContext ctx, [Option("user", "Пользователь для мута в войсе")] DiscordUser user)
        {
            var member = await ctx.Guild.GetMemberAsync(user.Id);
            var muteRole = ctx.Guild.Roles.Values.FirstOrDefault(r => r.Name == "MutedFromVoice");

            if (muteRole == null)
            {
                muteRole = await ctx.Guild.CreateRoleAsync("MutedFromVoice", Permissions.None, DiscordColor.DarkGray, false, true);

                foreach (var channel in ctx.Guild.Channels.Values)
                {
                    if (channel.Type == ChannelType.Voice)
                    {
                        await channel.AddOverwriteAsync(muteRole, DSharpPlus.Permissions.None, DSharpPlus.Permissions.UseVoice);
                    }
                }
            }

            await member.GrantRoleAsync(muteRole);

            if (member.VoiceState?.Channel != null)
            {
                await member.ModifyAsync(x => x.VoiceChannel = null);
            }

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent($"{member.DisplayName} замучен в войс-чате и не может подключаться."));
        }

        // 3. Бан ключевых слов (удаление + варн)
        [SlashCommand("banword", "Добавить запрещённое слово")]
        public async Task BanWord(InteractionContext ctx, [Option("word", "Слово для бана")] string word)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent($"Слово '{word}' добавлено в список запрещённых."));
        }

        // 4. Варн пользователя (счётчик + авто-мут/бан)
        [SlashCommand("warn", "Выдать варн пользователю")]
        public async Task Warn(InteractionContext ctx, [Option("user", "Пользователь для варна")] DiscordUser user)
        {
            if (!_warns.ContainsKey(user.Id))
                _warns[user.Id] = 0;

            _warns[user.Id]++;

            var member = await ctx.Guild.GetMemberAsync(user.Id);

            if (_warns[user.Id] == 3)
            {
                await MuteChat(ctx, user);
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent($"{member.DisplayName} получил 3 варна и был замучен в чате."));
            }
            else if (_warns[user.Id] >= 5)
            {
                await member.BanAsync();
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent($"{member.DisplayName} получил 5 варнов и был забанен."));
            }
            else
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent($"{member.DisplayName} получил варн. Всего варнов: {_warns[user.Id]}/5"));
            }
        }


        // 5. Бан пользователя
        [SlashCommand("ban", "Забанить пользователя")]
        public async Task Ban(InteractionContext ctx, [Option("user", "Пользователь для бана")] DiscordUser user)
        {
            var member = await ctx.Guild.GetMemberAsync(user.Id);
            await member.BanAsync();

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent($"{member.DisplayName} был забанен."));
        }

        // 6. Написание правил
        [SlashCommand("rules", "Вывести правила")]
        public async Task Rules(InteractionContext ctx)
        {
            var embed = new DiscordEmbedBuilder()
            {
                Title = "Правила сервера",
                Description = "1. Не флудить\n2. Не оскорблять участников\n3. Не использовать запрещённые слова\n4. Соблюдать правила Discord",
                Color = DiscordColor.Azure
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }
        // 7 Информация о боте
        [SlashCommand("Info", "Вывести информацию о боте")]
        public async Task Info(InteractionContext ctx)
        {
            var embed = new DiscordEmbedBuilder()
            {
                Title = "Информация о боте",
                Description = "Обычный модерирующий бот для Discord\nСоздатель бота - alfredo\nDiscord: 1255968122754699305\n Telegram: @TPABABPYKAX",
                Color = DiscordColor.Azure
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(embed));

        }
    }
}


