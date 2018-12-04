using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using DESUBot.Personality;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DESUBot.Modules
{
    public class RollRandom : ModuleBase<SocketCommandContext>
    {
        [Command("roll")]
        private async Task RandomRoll([Remainder]string message)
        {
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 101);
            await Context.Channel.SendMessageAsync($"{rollVar}% chance of {message}");

        }

        [Command("roll")]
        private async Task RandomRoll()
        {
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 101);
            await Context.Channel.SendMessageAsync($"{rollVar}% chance of that happening");

        }

        [Command("rate")]
        private async Task Rate([Remainder]string message)
        {
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 11);
            await Context.Channel.SendMessageAsync($"🤔 i would rate {message} a {rollVar}/10");
        }

        [Command("rate")]
        private async Task Rate()
        {
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 11);
            await Context.Channel.SendMessageAsync($"🤔 i would rate {Context.User.Mention} a {rollVar}/10");
        }

      
    }
}
