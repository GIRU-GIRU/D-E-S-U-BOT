using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using DESUBot.Personality;
using System.Threading.Tasks;
using System.IO;

namespace DESUBot.Modules
{
    public class Emoji : ModuleBase<SocketCommandContext>
    {
        string emojiFileName;
        [Command("emoji")]
        private async Task GetInfo(string input)
        {
            Emote emote;
            if (Emote.TryParse(input, out emote))
            {
                await Context.Channel.SendMessageAsync(emote.Url);
                return;
            }          
            for (int i = 0; i < input.Length; i += Char.IsSurrogatePair(input, i) ? 2 : 1)
            {
                int x = Char.ConvertToUtf32(input, i);
                emojiFileName = string.Format("{0:X4}", x);
            }     
            try
            {
                await Context.Channel.SendFileAsync(@"redacted" + emojiFileName + ".png");
                return;
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("invalid emoji");
                return;   
            }
        }

    }
}
