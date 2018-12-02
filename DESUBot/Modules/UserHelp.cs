using Discord.Commands;
using Discord.WebSocket;
using Discord;
using DESUBot.Personality;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using DESUBot.Models;

namespace DESUBot.Modules
{

    public class UserHelp : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task HelpAsync()
        {
            await Context.Channel.SendMessageAsync("dont be so fucking WEAK");
        }

        public static async Task UserJoined(SocketGuildUser guildUser)
        {
           
            var guildUserIGuildUser = guildUser as IGuildUser;
            var guildMainChannel = guildUser.Guild.GetChannel(Config.MainChannel);
            var chnl = guildMainChannel as ITextChannel;       
   
            await guildUser.AddRoleAsync(Helpers.ReturnRole(guildUser.Guild, UtilityRoles.Member));

            if (CommandToggles.WelcomeMessages)
            {
                // welcoming
                var insult = await Insults.GetInsult();
                Random rnd = new Random();
                string[] welcomeArray = new string[]
                {
               $"{guildUser.Mention} HIII {insult}",
               $"{guildUser.Mention} Hey there !! owo",
               $"{guildUser.Mention} uwu welcome to D E S U ",
               $"{guildUser.Mention}  😃😃😃, hiya ",
               $"some {insult} called {guildUser.Mention} has joined D E S U 😳",
               $"{guildUser.Mention}, 😳 hey there cutie ..",
               $"{guildUser.Mention} uwu welcome",
               $"{guildUser.Mention} has joined D E S U uwu",
               $"{guildUser.Mention} has joined D E S U 😳",
               $"{guildUser.Mention} 😳😳 hey cutie",
               $"{guildUser.Mention} has joined D E S U owo",

                };
                int pull = rnd.Next(welcomeArray.Length);
                string welcomeMessage = welcomeArray[pull].ToString();
                await chnl.SendMessageAsync(welcomeMessage);
            }
        }

        [Command("say")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        private async Task SayInMain([Remainder]string message)
        {
            var chnl = Context.Guild.GetTextChannel(Config.MainChannel);
            await chnl.SendMessageAsync(message);
        }

        [Command("bancleanse")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task BanUserAndCleanse()
        {
            var insult = await Insults.GetInsult();
            var embed = new EmbedBuilder();
            embed.WithTitle($"Bans & Cleanses a {insult} from this sacred place");
            embed.WithDescription("**Usage**: .ban \"user\" \"reason\"\n" +
                "**Target**: arrogant shitters \n" +
                "**Chat Purge**: 24 hours. \n" +
                "**Ban length:** Indefinite.");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        [Command("ban")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task BanUser()
        {
            var insult = await Insults.GetInsult();
            var embed = new EmbedBuilder();
            embed.WithTitle($"Permanently ends some {insult} from weeb territory");
            embed.WithDescription("**Usage**: .ban \"user\" \"reason\"\n" +
                "**Target**: arrogant shitters \n" +
                "**Length**: Indefinite.");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
