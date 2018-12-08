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
            if (!await ValidateAdminOrAbove(Context, Context.Message.Author as SocketGuildUser, reason)) return;
            if (!await ValidateAdminOrAbove(Context, user, reason)) return;
            try
            {
                var targetUser = user.Nickname;
                await user.KickAsync(reason);

                var embed = new EmbedBuilder();
                embed.WithTitle($"✅     {Context.User.Username} booted {targetUser}");
                embed.WithDescription($"reason: **{reason}**");
                embed.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            catch (Exception e)
            {
                await Context.Channel.SendMessageAsync("uhhh wouldnt let me ... " + e.Message);
            }
            try
            {
                var storeRoles = new StoreRoleMethods();
                await storeRoles.StoreUserRoles(Context, user as SocketGuildUser);
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("couldn't be arsed storing their roles b4 i kicked");
            }

        }

        [Command("ban")]
        private async Task BanUser(SocketGuildUser user, [Remainder]string reason)
        {
            if (!await ValidateAdminOrAbove(Context, Context.Message.Author as SocketGuildUser, reason)) return;
            if (!await ValidateAdminOrAbove(Context, user, reason)) return;

            try
            {
                var targetUser = user.Nickname;
                await user.Guild.AddBanAsync(user, 0, reason);

                var embed = new EmbedBuilder();
                embed.WithTitle($"✅     {Context.User.Username} banned {targetUser}");
                embed.WithDescription($"reason: _{reason}_");
                embed.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            catch (Exception e)
            {
                await Context.Channel.SendMessageAsync("uhhh wouldnt let me ... " + e.Message);
            }
            try
            {
                var storeRoles = new StoreRoleMethods();
                await storeRoles.StoreUserRoles(Context, user as SocketGuildUser);
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("couldn't be arsed storing their roles b4 i banned");
            }
        }

        [Command("searchban")]
        private async Task searchBan([Remainder]string input = null)
        {
            if (!Helpers.IsAdminOwner(Context.Message.Author as SocketGuildUser)) return;

            if (input == null)
            {
                return;
            }

            var bans = await Context.Guild.GetBansAsync();
            List<Discord.Rest.RestBan> matchedBans = new List<Discord.Rest.RestBan>(); 
            foreach (var ban in bans)
            {          
                if (ban.User.Username.ToLower().Contains(input.ToLower()))
                {
                    matchedBans.Add(ban);
                }               
            }

            if (bans.Count == 0 || matchedBans.Count == 0)
            {
                await Context.Channel.SendMessageAsync("couldn't find anyone sry owo");
                return;
            }

            var bannedUserNames = string.Join("\n", matchedBans.Select(x => x.User.Username).ToArray());

            var embed = new EmbedBuilder();
            embed.WithTitle($"Banned user names matching \"{input}\" ");
            embed.AddField("Username", bannedUserNames, true);
            embed.AddField("User ID: ", string.Join("\n", matchedBans.Select(x => x.User.Id)), true);
            embed.AddField("Reason for ban: ", string.Join("\n", matchedBans.Select(x => x.Reason)), true);
            await Context.Channel.SendMessageAsync("", false, embed.Build());

        }

        [Command("bancleanse")]
        private async Task BanUserAndClean(SocketGuildUser user, [Remainder]string reason = null)
        {
            if (!await ValidateAdminOrAbove(Context, Context.Message.Author as SocketGuildUser, reason)) return;
            if (!await ValidateAdminOrAbove(Context, user, reason)) return;

            try
            {
                await user.Guild.AddBanAsync(user, 1, reason);

                var embed = new EmbedBuilder();
                embed.WithTitle($"✅     {Context.User.Username} banned & cleansed  {user.Nickname}");
                embed.WithDescription($"reason: _{reason}_");
                embed.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            catch (Exception e)
            {
                await Context.Channel.SendMessageAsync("uhhh wouldnt let me ... " + e.Message);
            }
            try
            {
                var storeRoles = new StoreRoleMethods();
                await storeRoles.StoreUserRoles(Context, user as SocketGuildUser);
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("couldn't be arsed storing their roles b4 i bancleansed");
            }

        }
        
        string bannedUserName;
        [Command("unban")]
        private async Task UnbanUser(ulong userID)
        {
            if (!Helpers.IsAdminOwner(Context.Message.Author as SocketGuildUser)) return;

            bool existingBan = false;
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
            }

            if (existingBan == false)
            {
                await Context.Channel.SendMessageAsync("that's not a valid ID " + insult);
            }

            try
            {
                await Context.Guild.RemoveBanAsync(userID);
                await Context.Channel.SendMessageAsync($"✅    *** {bannedUserName} has been unbanned ***");
            }
            catch (Exception e)
            {
                await Context.Channel.SendMessageAsync("uhhh wouldnt let me ... " + e.Message);
            }
           
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

        [Command("name")]
        private async Task SetNick(SocketGuildUser user, [Remainder]string newName = null)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;

            var currentName = user.Nickname;
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



        [Command("say")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        private async Task SayInMain([Remainder]string message)
        {
            var chnl = Context.Guild.GetTextChannel(Config.MainChannel);
            await chnl.SendMessageAsync(message);
        }

        [Command("bancleanse")]
        private async Task BanUserAndCleanse()
        {
            if (!Helpers.IsAdminOwner(Context.Message.Author as SocketGuildUser)) return;
            var insult = await Insults.GetInsult();
            var embed = new EmbedBuilder();
            embed.WithTitle($"Bans & Cleanses a {insult} from weeb territory");
            embed.WithDescription("**Usage**: .ban \"user\" \"reason\"\n" +
                "**Target**: arrogant shitters \n" +
                "**Chat Purge**: 24 hours. \n" +
                "**Ban length:** Indefinite.");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        [Command("ban")]
        private async Task BanUser()
        {
            if (!Helpers.IsAdminOwner(Context.Message.Author as SocketGuildUser)) return;
            var insult = await Insults.GetInsult();
            var embed = new EmbedBuilder();
            embed.WithTitle($"Permanently ends some {insult} from weeb territory");
            embed.WithDescription("**Usage**: .ban \"user\" \"reason\"\n" +
                "**Target**: arrogant shitters \n" +
                "**Length**: Indefinite.");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("pin")]
        [Alias("p")]
        public async Task PinMessage(ulong messageId)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;

            try
            {
                var messageToPin = await Context.Channel.GetMessageAsync(messageId) as SocketUserMessage;
                await messageToPin.PinAsync();
                return;
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("how am i supposed to pin that BAKA");
            }
  
        }
    }
}



