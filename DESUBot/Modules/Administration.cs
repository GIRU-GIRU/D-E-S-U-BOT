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
using DESUBot.Data;

namespace DESUBot.Modules
{
    public class Administration : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        private async Task KickUser(SocketGuildUser user, [Remainder]string reason = null)
        {
            if (!await ValidateAdminOrAbove(Context, user, reason)) return;

            await user.KickAsync(reason);

            var embed = new EmbedBuilder();
            embed.WithTitle($"✅     {Context.User.Username} _booted_ {user.Nickname}");
            embed.WithDescription($"reason: **{reason}**");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("ban")]
        private async Task BanUser(SocketGuildUser user, [Remainder]string reason)
        {
            if (!await ValidateAdminOrAbove(Context, user, reason)) return;

            await user.KickAsync(reason);
            await user.Guild.AddBanAsync(user, 0, reason);

            var embed = new EmbedBuilder();
            embed.WithTitle($"✅     {Context.User.Username} banned {user.Nickname}");
            embed.WithDescription($"reason: _{reason}_");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }


        [Command("bancleanse")]
        private async Task BanUserAndClean(SocketGuildUser user, [Remainder]string reason = null)
        {
            if (!await ValidateAdminOrAbove(Context, user, reason)) return;

            await user.Guild.AddBanAsync(user, 1, reason);

            var embed = new EmbedBuilder();
            embed.WithTitle($"✅     {Context.User.Username} banned & cleansed  {user.Nickname}");
            embed.WithDescription($"reason: _{reason}_");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        bool existingBan;
        string bannedUserName;
        [Command("unban")]
        private async Task UnbanUser(ulong userID)
        {
            if (!Helpers.IsAdminOwner(Context.Message.Author as SocketGuildUser)) return;           

            var insult = await Insults.GetInsult();
            var allBans = await Context.Guild.GetBansAsync();
            foreach (var item in allBans)
            {
                if (item.User.Id == userID)
                {
                    existingBan = true;
                    bannedUserName = item.User.Username;
                    break;
                }
                else
                {
                    existingBan = false;
                }
            }

            if (existingBan == false)
            {
                await Context.Channel.SendMessageAsync("that's not a valid ID " + insult);
            }
            await Context.Guild.RemoveBanAsync(userID);
            await Context.Channel.SendMessageAsync($"✅    *** {bannedUserName} has been unbanned ***");
        }

        [Command("warn")]
        private async Task WarnUserCustom(SocketGuildUser user, [Remainder]string reason = null)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser))
            {
                await Context.Channel.SendMessageAsync("nah");
                return;
            }

            if (reason == null)
            {
                string warningMessage = await Insults.GetWarning();
                try
                {
                    await user.SendMessageAsync(warningMessage);
                    await Context.Channel.SendMessageAsync($"⚠      *** {user.Username} has received a warning.      ⚠***");
                }
                catch (HttpException ex)
                {
                    await Context.Channel.SendMessageAsync(user.Mention + ", " + warningMessage);
                }
            }
            else
            {
                try
                {
                    await user.SendMessageAsync("You have been warned in D E S U for: " + reason);
                    await Context.Channel.SendMessageAsync($"⚠      *** {user.Username} has received a warning.      ⚠***");
                }
                catch (HttpException ex)
                {
                    await Context.Channel.SendMessageAsync($"{user.Mention}, {reason}");
                }
            }
        }

        string currentName;
        [Command("name")]
        private async Task SetNick(SocketGuildUser user, [Remainder]string newName = null)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;

            currentName = user.Nickname;
            if (string.IsNullOrEmpty(user.Nickname))
            {
                currentName = user.Username;
            }
            await user.ModifyAsync(x => x.Nickname = newName);
            await Context.Message.DeleteAsync();

            var embedReplaceRemovedRole = new EmbedBuilder();
            embedReplaceRemovedRole.WithTitle($"✅ {currentName} had their name changed successfully");
            embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
        }
        [Command("resetname")]
        private async Task SetNick(SocketGuildUser user)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;

            var currentName = user.Nickname;
            await user.ModifyAsync(x => x.Nickname = user.Username);
            await Context.Channel.SendMessageAsync("name reset 👍");
        }

        [Command("off")]
        private async Task TurnOffUser(SocketGuildUser user)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;

            OnOffUser.TurnedOffUsers.Add(user);
            await Context.Channel.SendMessageAsync($"*turned off {user.Mention}*");
            return;
        }

        [Command("on")]
        private async Task TurnOnUserAsync(SocketGuildUser user)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;

            var userToRemove = OnOffUser.TurnedOffUsers.Find(x => x.Id == user.Id);
            OnOffUser.TurnedOffUsers.Remove(userToRemove);
            await Context.Channel.SendMessageAsync($"*{user.Mention} turned back on*");
            return;
        }


        private async Task<bool> ValidateModOrAbove(SocketCommandContext context, SocketGuildUser user, string reason)
        {
            if (Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser))
            {
                return true;
            }
            if (Helpers.IsModAdminOwner(user))
            {
                await context.Channel.SendMessageAsync("stop fighting urselves u retards");
                return true;
            }
            if (reason == null)
            {
                await context.Channel.SendMessageAsync("give me a reason to kick them maybe");
                return true;
            }
            return false;
        }

        private async Task<bool> ValidateAdminOrAbove(SocketCommandContext context, SocketGuildUser user, string reason)
        {
            if (Helpers.IsAdminOwner(Context.Message.Author as SocketGuildUser))
            {
                return true;
            }
            if (Helpers.IsAdminOwner(user))
            {
                await context.Channel.SendMessageAsync("stop fighting urselves u retards");
                return true;
            }
            if (reason == null)
            {
                await context.Channel.SendMessageAsync("give me a reason to kick them maybe");
                return true;
            }
            return false;
        }
    }
}



