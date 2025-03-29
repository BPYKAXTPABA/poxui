using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using HAPYKAX.commands;
using HAPYKAX.Commands;
using HAPYKAX.commands.Slash;
using HAPYKAX.config;

namespace HAPYKAX
{
    internal class Program
    {
        public static DiscordClient Client { get; set; }
        public static CommandsNextExtension Commands { get; set; } 

        public static async Task Main(string[] args) 
        {
            var jsonReader = new JSONReader();
            await jsonReader.ReadJson();

            var discordConfig = new DiscordConfiguration()
            {
                Token = jsonReader.token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                AutoReconnect = true
            };

            Client = new DiscordClient(discordConfig);
            Client.Ready += ClientOnReady;

            var commandConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new [] { jsonReader.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            };
            
            var slashCommandsConfig = Client.UseSlashCommands();
            slashCommandsConfig.RegisterCommands<BasicSlashCommands>();
            
            slashCommandsConfig.RegisterCommands<SlashCommands>();

            Commands = Client.UseCommandsNext(commandConfig);
            Commands.RegisterCommands<TestCommands>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task ClientOnReady(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}