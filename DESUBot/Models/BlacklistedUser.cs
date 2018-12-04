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
    public class BlacklistedUser
    {
        public static List<SocketUser> BlackListedUser { get; } = new List<SocketUser>();
    }
}