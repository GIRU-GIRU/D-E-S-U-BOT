﻿using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Linq;
using DESUBot.Models;

namespace DESUBot.Modules
{
    public static class Helpers
    {
        public static bool IsRole(string role, SocketGuildUser user)
        {
            var result = from r in user.Guild.Roles
                         where r.Name.ToLower() == role.ToLower()
                         select r.Id;
            ulong roleID = result.FirstOrDefault();
            //first or default NEVER returns null
            if (roleID == 0)
            {
                return false;
            }
            else
            {
                var targetRole = user.Guild.GetRole(roleID);
                return user.Roles.Contains(targetRole);
            }
        }

        public static bool IsGiruOrOwner(SocketGuildUser user)
        {
            if(user.Id == user.Guild.OwnerId)
            {
                return true;
            }
            if (user.Id == 150764876258607105)
            {
                return true;
            }
            return false;
        }

        public static bool IsModAdminOwner(SocketGuildUser user)
        {
            if (user.Id == user.Guild.OwnerId)
            {
                return true;
            }

            bool isMod = user.Roles.Where(x => x.Name == UtilityRoles.Moderator).Any();
            bool isAdmin = user.Roles.Where(x => x.Name == UtilityRoles.Admin).Any();

            if (isMod || isAdmin)
            {
                return true;
            }
            return false;
        }

        public static bool isOwner(SocketGuildUser user)
        {
            if (user.Id == user.Guild.OwnerId)
            {
                return true;
            }
            return false;
        }

        public static bool IsAdminOwner(SocketGuildUser user)
        {
            if (user.Id == user.Guild.OwnerId)
            {
                return true;
            }
            bool isAdmin = user.Roles.Where(x => x.Name == UtilityRoles.Admin).Any();

            if (isAdmin)
            {
                return true;
            }
            return false;
        }







        public static IRole ReturnRole(SocketGuild guild, string role)
        {
            var result = from r in guild.Roles
                         where r.Name.ToLower() == role.ToLower()
                         select r.Id;
            ulong roleID = result.FirstOrDefault();
            var rolereturn = guild.GetRole(roleID) as IRole;
            return rolereturn;
        }
        public static IRole IsRoleReturn(string role, SocketGuildUser user)
        {
            //return user.Guild.GetRole(user.Guild.Roles.FirstOrDefault(x => x.Name == role).Id);     
            var result = from r in user.Roles
                         where r.Name.ToLower() == role.ToLower()
                         select r.Id;
            ulong roleID = result.FirstOrDefault();

            if (roleID == 0)
            {

                return user.Guild.GetRole(0);
            }
            else
            {

                return user.Guild.GetRole(roleID);
            }
        }

        public static string FindEmoji(SocketGuildUser user, string emojiName)
        {
            var result = from r in user.Guild.Emotes
                         where r.Name == emojiName
                         select r.Id;
            ulong emojiID = result.FirstOrDefault();
            if (emojiID == 0)
            {
                Console.WriteLine($"Could not find {emojiName} emoji");
                return "🤔";
            }
            else
            {
                string finalEmoji = $"<:{emojiName}:{emojiID}>";
                return finalEmoji;
            }
        }


        public static IRole FindRole(SocketGuildUser user, string roleName)
        {
            var result = from r in user.Guild.Roles
                         where r.Name == roleName
                         select r.Id;
            ulong roleID = result.FirstOrDefault();
            var roleIRole = user.Guild.GetRole(roleID);
            if (roleID == 0)
            {
                Console.WriteLine($"Could not find {roleName} role");
                return roleIRole;
            }

            return roleIRole;
        }
        public static void AddMany<T>(this List<T> list, params T[] elements)
        {
            list.AddRange(elements);
        }
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> sequence, T item)
        {
            return sequence.Concat(new[] { item });
        }

        public static bool OnOffExecution(IMessage msg)
        {
            if (OnOffUser.TurnedOffUsers == null)
            {
                return false;
            }
            var list = OnOffUser.TurnedOffUsers;
            var selectedList = list.Select(x => x.Id == msg.Author.Id).ToList();
            return list.Where(x => x.Id == msg.Author.Id).Any();
        }

        public static async Task<string> GetUsernameListFromIDs(IReadOnlyCollection<ulong> collection, IGuild guild)
        {
            List<string> MentionedUsers = new List<string>();
            foreach (var userID in collection)
            {
                var user = await guild.GetUserAsync(userID);

                MentionedUsers.Add(user.Username);
            }
            return String.Join(", ", MentionedUsers.ToArray());
        }
    }
}





//var result = from r in user.Guild.Emotes
//             where r.Name == emojiName
//             select r.Name;
//var emojiID = result.ToString();
//first or default NEVER returns null
//if (emojiID is null)
//{
//    Console.WriteLine($"Could not find {emojiName} emoji");
//    return "🤔";
//}
//else
//{
//    return emojiID.ToString();
//}
