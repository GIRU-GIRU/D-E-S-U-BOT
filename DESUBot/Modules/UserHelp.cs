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
using System.Linq;

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
   
           // await guildUser.AddRoleAsync(Helpers.ReturnRole(guildUser.Guild, UtilityRoles.Member));

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

        [Command("d")]
        public async Task UserSelfDeleteMessages(int amountToDelete = 0)
        {
            if (!Helpers.IsRole(UtilityRoles.Delete, Context.Message.Author as SocketGuildUser)) return;

            try
            {
                var amount = 100;

                var usersocket = Context.Message.Author as SocketGuildUser;             
                var msgsCollection = await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
                var result = from m in msgsCollection
                             where m.Author == Context.Message.Author
                             select m.Id;

                var chnl = Context.Channel as ITextChannel;
                var totalToDelete = amountToDelete == 0 ? result.Take(amount) : result;

                await chnl.DeleteMessagesAsync(totalToDelete);
                var cleanseUserEmbed = new EmbedBuilder();
                cleanseUserEmbed.WithTitle($"🗑      {Context.Message.Author.Username} self deleted {totalToDelete.Count()} messages");
                cleanseUserEmbed.WithColor(new Color(105, 105, 105));
                await Context.Channel.SendMessageAsync("", false, cleanseUserEmbed.Build());
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync("something went wrong uwu... " + ex.Message);
            }

        }

        
    }
}
