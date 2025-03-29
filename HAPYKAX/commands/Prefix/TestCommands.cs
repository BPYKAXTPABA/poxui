using System.Security.Cryptography;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace HAPYKAX.commands
{
    public class TestCommands : BaseCommandModule

    {
        [Command("help")]
        public async Task Help(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync(content: "пошел нахуй");
        }
        
        
        [Command(name: "hello")]
        public async Task Hello(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("привет");
        }

        
        [Command(name: "random")]
        public async Task Random(CommandContext ctx, int min, int max)
        {
            var randomValue = new System.Random().Next(min, max);
            await ctx.Channel.SendMessageAsync( content:ctx.User.Mention + " - ваше число " + randomValue);
        }
    }
}