using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DESUBot.Modules
{
    public class Blacklist : ModuleBase<SocketCommandContext>
    {
        [Command("blacklist")]
        public async Task BlacklistUser(SocketUser user)
        {
            if (!Helpers.IsGiruOrOwner(Context.Message.Author as SocketGuildUser)) return; ;
            if (user.Id == 517856522047455232)
            {
                await Context.Channel.SendMessageAsync("i wont blacklist myself owo ");
                return;
            }

            if (BlacklistedUser.BlackListedUser.Contains(user))
            {
                BlacklistedUser.BlackListedUser.Remove(user);
                await Context.Channel.SendMessageAsync("unblacklisted " + user.Username);
            }
            else
            {
                BlacklistedUser.BlackListedUser.Add(user);
                await Context.Channel.SendMessageAsync("blacklisted " + user.Username);
            }
        }
    }
}
