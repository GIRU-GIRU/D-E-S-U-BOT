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

    public class OnExecutedCommand
    {

        CommandInfo _info;
        ICommandContext _context;
        IResult _result;
        ITextChannel adminlogchannel;

        private DiscordSocketClient _client;
        public OnExecutedCommand(DiscordSocketClient client)
        {
            _client = client;
        }
        public async Task AdminLog(CommandInfo info, ICommandContext context, IResult result)
        {
            _info = info;
            _context = context;
            _result = result;

           
        }
    }
}


