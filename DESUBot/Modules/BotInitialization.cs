using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using DESUBot.Personality;
using System.Threading.Tasks;
using System.Linq;

namespace DESUBot.Modules
{
  public class BotInitialization : ModuleBase<SocketCommandContext>
    {
        private static DiscordSocketClient _client;

        public BotInitialization(DiscordSocketClient client)
        {
            _client = client;
        }

        public static async Task StartUpMessages()
        {
            var chnl = _client.GetChannel(Config.MainChannel) as ITextChannel;
            await chnl.SendMessageAsync("I'm being turned on 😳😳😳😳");

        }      
    }
    
}