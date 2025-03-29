using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace HAPYKAX.commands.Slash
{
    public class BasicSlashCommands : ApplicationCommandModule

    {
        [SlashCommand("test", "a simple text command")]
        public async Task TestSlashCommand(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            var embedMessage = new DiscordEmbedBuilder()
            {
               Title = "Test Slash Command",
               Description = "This is a test slash command.",
               Color = DiscordColor.Azure
            };
            
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }

        [SlashCommand("parameter", "a simple text command with parameters")]
        public async Task TestSlashCommandParameter(InteractionContext ctx,
            [Option("testString", "test parameter")] string testParameter, [Option("testLong", "test long parameter")] long testLong)
        {
            await ctx.DeferAsync();

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "Test Slash Command With Parameter",
                Description = $"This is a test slash command with parameter: {testParameter}, and test long parameter: {testLong}", 
                Color = DiscordColor.Azure
            };
            
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }

        [SlashCommand("userDetail", "a simple text command with user detail")]
        public async Task TestUserCommand(InteractionContext ctx,
            [Option("user", "get info about selected user")] DiscordUser user)
        {
            await ctx.DeferAsync();

            var member = (DiscordMember)user;

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "User Detail",
                Description = $"User: {user.Username}, ID: {user.Id}",
                Color = DiscordColor.Azure
            };
            
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
    }
}