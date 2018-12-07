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
    public class AdministrationRoles : ModuleBase<SocketCommandContext>
    {
        [Command("give")]
        [Alias("add")]
        private async Task AssignRoles(SocketGuildUser user, [Remainder]string inputRoles = null)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser))
            {
                return;
            }
            if (inputRoles == null)
            {
                return;
            }

            var guildRoles = user.Guild.Roles.Select(x => x.Name.ToLower()).Intersect(inputRoles.ToLower().Split(','));

            List<IRole> rolesToAdd = new List<IRole>();
            List<string> roleNames = new List<string>();
            foreach (var item in guildRoles)
            {
                var role = Helpers.ReturnRole(Context.Guild, item);
                rolesToAdd.Add(role);
                roleNames.Add(role.Name);
            }

            if (rolesToAdd.Count == 0)
            {
                await Context.Channel.SendMessageAsync($"what teh FUCK is \"{inputRoles}\" suppoed to mean to ME ... uwu ????");
                return;
            }

            try
            {
                await user.AddRolesAsync(rolesToAdd);
                var embed = new EmbedBuilder();
                embed.WithTitle($"✅   {Context.Message.Author.Username} granted {string.Join(',', roleNames)} to {user.Username}");
                embed.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
                return;
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("noo im not doing that uwu");
            }

        }

        [Command("del")]
        private async Task RemoveRoles(SocketGuildUser user, [Remainder]string inputRoles = null)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser))
            {
                return;
            }
            if (inputRoles == null)
            {
                await Context.Channel.SendMessageAsync("wat");
                return;
            }

            var guildRoles = user.Guild.Roles.Select(x => x.Name.ToLower()).Intersect(inputRoles.ToLower().Split(','));

            List<IRole> rolesToRemove = new List<IRole>();
            List<string> roleNames = new List<string>();
            foreach (var item in guildRoles)
            {
                var role = Helpers.ReturnRole(Context.Guild, item);
                rolesToRemove.Add(role);
                roleNames.Add(role.Name);

            }

            try
            {
                await user.RemoveRolesAsync(rolesToRemove);
                var embed = new EmbedBuilder();
                embed.WithTitle($"✅   {Context.Message.Author.Username} granted {string.Join(',', roleNames)} to {user.Username}");
                embed.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
                return;

            }
            catch (Exception)
            {

                await Context.Channel.SendMessageAsync("noo im not doing that uwu");
            }
        }


        [Command("mute")]
        private async Task Mute(SocketGuildUser user)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser))
            {
                return;
            }

            var mutedRole = Helpers.ReturnRole(Context.Guild, UtilityRoles.Muted);
            if (mutedRole is null)
            {
                await Context.Channel.SendMessageAsync("cant find muted role !");
                return;
            }
            var insult = await Insults.GetInsult();
            if (Helpers.IsRole(UtilityRoles.Muted, user))
            {
                await Context.Channel.SendMessageAsync("they already muted u dumbass");
                return;
            }
            if (Helpers.IsModAdminOwner(user))
            {
                await Context.Channel.SendMessageAsync("stop beefing with eachother fucking bastards");
                return;
            }

            try
            {
                var embedReplaceRemovedRole = new EmbedBuilder();
                embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} successfully muted {user.Username}");
                embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
                await user.AddRoleAsync(mutedRole);
                return;
            }
            catch (Exception)
            {

                await Context.Channel.SendMessageAsync("i wont do that to them uwu");
            }
        }

        [Command("unmute")]
        private async Task UnMute(SocketGuildUser user)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;
            

            var mutedRole = Helpers.FindRole(user, UtilityRoles.Muted);
            if (mutedRole is null)
            {
                await Context.Channel.SendMessageAsync("cant find muted role !");
                return;
            }
            var insult = await Insults.GetInsult();

            if (!Helpers.IsRole(UtilityRoles.Muted, user))
            {
                await Context.Channel.SendMessageAsync("theyre not even muted u " + insult);
                return;
            }
            if (Helpers.IsModAdminOwner(user))
            {
                await Context.Channel.SendMessageAsync("stop beefing with eachother fucking bastards");
                return;
            }

            try
            {
                var embedReplaceRemovedRole = new EmbedBuilder();
                embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} successfully unmuted {user.Username}");
                embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
                await user.RemoveRoleAsync(mutedRole);
                return;
            }
            catch (Exception)
            {

                await Context.Channel.SendMessageAsync("i wont do that to them uwu");
            }
        }


        [Command("cant")]
        private async Task CantPostPics(IGuildUser user)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;

            var userSocket = user as SocketGuildUser;
            var picsRole = Helpers.FindRole(userSocket, UtilityRoles.PicPermDisable);
            if (picsRole is null)
            {
                await Context.Channel.SendMessageAsync("cant find cpp role !");
                return;
            }
            var insult = await Insults.GetInsult();
            if (Helpers.IsRole(UtilityRoles.PicPermDisable, userSocket))
            {
                await Context.Channel.SendMessageAsync("they already cant u dumbass");
                return;
            }
            if (Helpers.IsModAdminOwner(user as SocketGuildUser))
            {
                await Context.Channel.SendMessageAsync("stop beefing with eachother fucking bastards");
                return;
            }

            try
            {
                var embedReplaceRemovedRole = new EmbedBuilder();
                embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} removed the pic perms of {user.Username}");
                embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
                await userSocket.AddRoleAsync(picsRole);
                return;
            }
            catch (Exception)
            {

                await Context.Channel.SendMessageAsync("i wont do that to them uwu");
            }
        }

        

        [Command("can")]
        private async Task CanPostPics(IGuildUser user)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;

            var userSocket = user as SocketGuildUser;
            var picsRole = Helpers.FindRole(userSocket, UtilityRoles.PicPermDisable);
            if (picsRole is null)
            {
                await Context.Channel.SendMessageAsync("cant find cant post pics role !");
                return;
            }

            var insult = await Insults.GetInsult();
            if (!Helpers.IsRole(UtilityRoles.PicPermDisable, userSocket))
            {
                await Context.Channel.SendMessageAsync("they can post pics u " + insult);
                return;
            }
            if (Helpers.IsModAdminOwner(user as SocketGuildUser))
            {
                await Context.Channel.SendMessageAsync("stop beefing with eachother fucking bastards");
                return;
            }

            try
            {
               
                var embedReplaceRemovedRole = new EmbedBuilder();
                embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} returned pic perms for {user.Username}");
                embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
                await userSocket.RemoveRoleAsync(picsRole);
                return;
            }
            catch (Exception)
            {

               await Context.Channel.SendMessageAsync("i wont do that to them uwu");
            }


        }



        [Command("storeroles")]
        private async Task StoreRoles(SocketGuildUser target)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;
            try
            {
                var storeRoles = new StoreRoleMethods();
                await storeRoles.StoreUserRoles(Context, target);
                await Context.Channel.SendMessageAsync($"{target.Username} successfully had their roles stored");
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"smth went wrong {ex.Message}");
            }
        }
        [Command("storeroles")]
        private async Task StoreRoles(ulong ID)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;

            try
            {
                SocketGuildUser target = Context.Guild.GetUser(ID);
                var storeRoles = new StoreRoleMethods();
                await storeRoles.StoreUserRoles(Context, target);
                await Context.Channel.SendMessageAsync($"{target.Username} successfully had their roles stored");
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"smth went wrong {ex.Message}");
            }
        }
        [Command("restoreroles")]
        private async Task RestoreRoles(SocketGuildUser target)
        {
            if (!Helpers.IsModAdminOwner(Context.Message.Author as SocketGuildUser)) return;

            try
            {
                var restoreRoles = new StoreRoleMethods();
                await restoreRoles.RestoreUserRoles(Context, target);
                await Context.Channel.SendMessageAsync($"FINE..  {target.Username} successfully had their roles restored");
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"smth went wrong {ex.Message}");
            }
        }

    }
}



