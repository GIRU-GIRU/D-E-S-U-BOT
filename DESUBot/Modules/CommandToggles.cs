using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Discord.WebSocket;
using DESUBot.Personality;
using System.Threading.Tasks;
using DESUBot.Preconditions;
using Discord.Net;
using System.Linq;
using DESUBot.Models;


namespace DESUBot.Modules
{

    public static class CommandToggles
    {
        public static bool WelcomeMessages = true;
        public static bool Memestore = true;
    }

    public class Toggles : ModuleBase<SocketCommandContext>
    {
        [Command("welcomemessages on")]
        private async Task WelcomeMessagesOn()
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;
            CommandToggles.WelcomeMessages = true;
            await Context.Channel.SendMessageAsync("Welcome messages turned on");
        }

        [Command("welcomemessages off")]
        private async Task WelcomeMessagesOff()
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;
            CommandToggles.WelcomeMessages = false;
            await Context.Channel.SendMessageAsync("Welcome messages turned off");
        }
        [Command("memestore on")]
        private async Task memestoreOn()
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;
            CommandToggles.Memestore = true;
            await Context.Channel.SendMessageAsync("Memestore turned on");
        }

        [Command("memestore off")]
        private async Task memestoreOff()
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;
            CommandToggles.Memestore = false;
            await Context.Channel.SendMessageAsync("Memestore turned off");
        }

    }

}


