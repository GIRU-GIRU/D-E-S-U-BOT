﻿using DESUBot.Personality;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DESUBot.Logging
{
    public static class ConsoleLog
    {
        public static async Task LogToConsole(IResult result, SocketCommandContext context)
        {
            switch (result.Error)
            {
                case CommandError.UnmetPrecondition:
                    if (result.ErrorReason != "DisableMessage")
                    {
                        await context.Channel.SendMessageAsync(await ErrorReturnStrings.GetNoPerm());                        
                    }
                    break;

                //case CommandError.ParseFailed:
                //    await context.Channel.SendMessageAsync(await ErrorReturnStrings.GetParseFailed());
                //    break;
                // deprecated due to spam

                default:
                    if (!String.IsNullOrWhiteSpace(result.ErrorReason))
                    {
                        Console.WriteLine(result.ErrorReason);
                        break;
                    }
                    break;

            }
        }

    }
}
