using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DESUBot.Personality;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DESUBot.Modules
{
    public class OnOffUser
    {
        public static List<IGuildUser> TurnedOffUsers { get; } = new List<IGuildUser>();
    }
}