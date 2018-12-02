using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DESUBot.Models;
using DESUBot.Personality;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FaceApp;
using System.Collections.Generic;

namespace DESUBot.Modules
{
    public class OnMessage : ModuleBase<SocketCommandContext>
         
    {
        private static DiscordSocketClient _client;
        private FaceAppClient _FaceAppClient;
        public OnMessage(DiscordSocketClient client, FaceAppClient FaceAppClient)
        {
            _client = client;
            _FaceAppClient = FaceAppClient;
        }

        private static Regex regexNounTest = new Regex(@"^\![^ ]+test");
        private static Regex regexInviteLinkDiscord = new Regex(@"(https?:\/\/)?(www\.)?(discord\.(gg|io|me|li)|discordapp\.com\/invite)\/.+[a-z]");
        public async Task MessageContainsAsync(SocketMessage arg)
        {
            //ignore ourselves, check for null
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);

            if (message.Author.IsBot || Helpers.IsModAdminOwner(context.User as SocketGuildUser)) return;
            if (Helpers.OnOffExecution(context.Message) == true)
            {
                await context.Message.DeleteAsync();
            }
            if (message.Content.Contains("<:loki:448530823709327361>"))
            {
                var r = new Random();
                if (r.Next(1, 15) <= 2)
                {
                    await context.Channel.SendMessageAsync("<:loki:448530823709327361>");
                }
            }
            if (regexNounTest.Match(message.Content).Success)
            {
                var noun = regexNounTest.Match(message.Content).Groups[2].ToString();
                var nounTestTask = new RollRandom();
                await nounTestTask.NounTest(noun, message);
            }
            if (regexInviteLinkDiscord.Match(message.Content).Success & !Helpers.IsModAdminOwner(context.User as SocketGuildUser))
            {
                var insult = await Insults.GetInsult();
                await context.Message.DeleteAsync();
                await context.Channel.SendMessageAsync($"{context.User.Mention}, don't post invite links {insult}");
            }
        }
        public async Task UpdatedMessageContainsAsync(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            var messageAfter = after as SocketUserMessage;
            var context = new SocketCommandContext(_client, messageAfter);
            if (messageAfter.Author.IsBot || Helpers.IsModAdminOwner(context.User as SocketGuildUser)) return;
            if (regexInviteLinkDiscord.Match(messageAfter.Content).Success)
            {
                var insult = await Insults.GetInsult();
                await context.Message.DeleteAsync();
                await context.Channel.SendMessageAsync($"{context.User.Mention}, don't post invite links {insult}");
            }
            if (messageAfter.Content.Contains("<:loki:448530823709327361>"))
            {
                var r = new Random();
                if (r.Next(1, 15) <= 2)
                {
                    await context.Channel.SendMessageAsync("<:loki:448530823709327361>");
                }
            }
        }    
    }
}
